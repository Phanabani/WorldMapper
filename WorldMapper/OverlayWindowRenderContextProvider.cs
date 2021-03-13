using System;
using System.Drawing;
using SharpGL;
using SharpGL.RenderContextProviders;
using SharpGL.Version;

namespace WorldMapper
{
    public class OverlayWindowRenderContextProvider : RenderContextProvider
    {
        private const uint PFD_SUPPORT_COMPOSITION = 0x8000;

        private Win32.WNDCLASSEX wndClass;
        private static Win32.WndProc wndProcDelegate = WndProc;

        /// <summary>The window handle.</summary>
        protected IntPtr windowHandle = IntPtr.Zero;

        public IntPtr WindowHandle => windowHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpGL.RenderContextProviders.HiddenWindowRenderContextProvider" /> class.
        /// </summary>
        public OverlayWindowRenderContextProvider() => GDIDrawingEnabled = true;

        /// <summary>
        /// Creates the render context provider. Must also create the OpenGL extensions.
        /// </summary>
        /// <param name="openGLVersion">The desired OpenGL version.</param>
        /// <param name="gl">The OpenGL context.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bitDepth">The bit depth.</param>
        /// <param name="parameter">The parameter</param>
        /// <returns></returns>
        public override bool Create(
            OpenGLVersion openGLVersion,
            OpenGL gl,
            int width,
            int height,
            int bitDepth,
            object parameter)
        {
            base.Create(openGLVersion, gl, width, height, bitDepth, parameter);

            // Register window
            wndClass = new Win32.WNDCLASSEX();
            wndClass.Init();
            wndClass.style = Win32.ClassStyles.HorizontalRedraw | Win32.ClassStyles.OwnDC |
                             Win32.ClassStyles.VerticalRedraw;
            wndClass.lpfnWndProc = wndProcDelegate;
            wndClass.cbClsExtra = 0;
            wndClass.cbWndExtra = 0;
            wndClass.hInstance = IntPtr.Zero;
            wndClass.hIcon = IntPtr.Zero;
            wndClass.hCursor = IntPtr.Zero;
            wndClass.hbrBackground = IntPtr.Zero;
            wndClass.lpszMenuName = null;
            wndClass.lpszClassName = "SharpGLRenderWindow";
            wndClass.hIconSm = IntPtr.Zero;
            int num = Win32.RegisterClassEx(ref wndClass);

            // Create window
            windowHandle = Win32.CreateWindowEx(Win32.WindowStylesEx.WS_EX_LEFT, "SharpGLRenderWindow", "",
                Win32.WindowStyles.WS_CLIPCHILDREN | Win32.WindowStyles.WS_CLIPSIBLINGS | Win32.WindowStyles.WS_POPUP,
                0, 0, width, height, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            // Set blur behind with invalid region to make transparent without blur
            var blurBehind = new BlurBehind(windowHandle);
            blurBehind.SetBlurBehindRegion(new Region(new Rectangle(0, 0, -1, -1)));
            blurBehind.SetBlurBehind(true);

            deviceContextHandle = Win32.GetDC(windowHandle);

            // Set pixel format descriptor
            Win32.PIXELFORMATDESCRIPTOR ppfd = new Win32.PIXELFORMATDESCRIPTOR();
            ppfd.Init();
            ppfd.nVersion = 1;
            ppfd.dwFlags =
                Win32.PFD_DRAW_TO_WINDOW
                | Win32.PFD_SUPPORT_OPENGL
                | PFD_SUPPORT_COMPOSITION
                | Win32.PFD_DOUBLEBUFFER;
            ppfd.iPixelType = Win32.PFD_TYPE_RGBA;
            ppfd.cColorBits = 32;
            ppfd.cAlphaBits = 8;
            ppfd.cDepthBits = 24;
            ppfd.cStencilBits = 8;
            ppfd.iLayerType = Win32.PFD_MAIN_PLANE;
            int iPixelFormat;
            if ((iPixelFormat = Win32.ChoosePixelFormat(deviceContextHandle, ppfd)) == 0 ||
                Win32.SetPixelFormat(deviceContextHandle, iPixelFormat, ppfd) == 0)
                return false;

            // Create render context
            renderContextHandle = Win32.wglCreateContext(deviceContextHandle);

            // I think these calls might be unnecessary
            MakeCurrent();
            UpdateContextVersion(gl);
            return true;
        }

        public static OverlayWindowRenderContextProvider CustomCreate(OpenGL gl, OpenGLVersion version, int width, int height)
        {
            var overlayRCP = new OverlayWindowRenderContextProvider();
            overlayRCP.Create(version, gl, width, height, 1, null);
            gl.CreateFromExternalContext(
                version, width, height, 1, overlayRCP.windowHandle,
                overlayRCP.renderContextHandle, overlayRCP.deviceContextHandle
            );
            return overlayRCP;
        }

        private static IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) =>
            Win32.DefWindowProc(hWnd, msg, wParam, lParam);

        // NOTE: I don't think we need the following methods since the context
        // provider gets wrapped in an ExternalRenderContextProvider... but
        // we'll just have to see

        /// <summary>Destroys the render context provider instance.</summary>
        public override void Destroy()
        {
            Win32.ReleaseDC(windowHandle, deviceContextHandle);
            Win32.DestroyWindow(windowHandle);
            Win32.UnregisterClass(wndClass.lpszClassName, wndClass.hInstance);
            base.Destroy();
        }

        /// <summary>Sets the dimensions of the render context provider.</summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public override void SetDimensions(int width, int height)
        {
            base.SetDimensions(width, height);
            Win32.SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, Width, Height,
                Win32.SetWindowPosFlags.SWP_NOACTIVATE | Win32.SetWindowPosFlags.SWP_NOCOPYBITS |
                Win32.SetWindowPosFlags.SWP_NOMOVE | Win32.SetWindowPosFlags.SWP_NOOWNERZORDER);
        }

        /// <summary>
        /// Blit the rendered data to the supplied device context.
        /// </summary>
        /// <param name="hdc">The HDC.</param>
        public override void Blit(IntPtr hdc)
        {
            if (!(deviceContextHandle != IntPtr.Zero) && !(windowHandle != IntPtr.Zero))
                return;
            Win32.SwapBuffers(deviceContextHandle);
            Win32.BitBlt(hdc, 0, 0, Width, Height, deviceContextHandle, 0, 0, 13369376U);
        }

        /// <summary>Makes the render context current.</summary>
        public override void MakeCurrent()
        {
            if (!(renderContextHandle != IntPtr.Zero))
                return;
            Win32.wglMakeCurrent(deviceContextHandle, renderContextHandle);
        }
    }
}
