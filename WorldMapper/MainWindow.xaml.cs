using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using SharpGL;
using SharpGL.Version;
using SharpGL.WPF;

namespace WorldMapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Scene _scene;

        public MainWindow()
        {
            InitializeComponent();
            _scene = new Scene();
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            IntPtr hWnd = new WindowInteropHelper(this).Handle;
            var bb = new BlurBehind(hWnd);
            bb.SetBlurBehindRegion(new Region(new Rectangle(0, 0, -1, -1)));
            bb.SetBlurBehind(true);
            uint PFD_SUPPORT_COMPOSITION = 0x8000;

            var pfd = new Win32.PIXELFORMATDESCRIPTOR
            {
                nSize = 40,
                nVersion = 1,
                dwFlags =
                    Win32.PFD_DRAW_TO_WINDOW
                    | Win32.PFD_SUPPORT_OPENGL
                    | PFD_SUPPORT_COMPOSITION
                    | Win32.PFD_DOUBLEBUFFER,
                iPixelType = Win32.PFD_TYPE_RGBA,
                cColorBits = 32,
                cRedBits = 0,
                cRedShift = 0,
                cGreenBits = 0,
                cGreenShift = 0,
                cBlueBits = 0,
                cBlueShift = 0,
                cAlphaBits = 8,
                cAlphaShift = 0,
                cAccumBits = 0,
                cAccumRedBits = 0,
                cAccumGreenBits = 0,
                cAccumBlueBits = 0,
                cAccumAlphaBits = 0,
                cDepthBits = 24,
                cStencilBits = 8,
                cAuxBuffers = 0,
                iLayerType = Win32.PFD_MAIN_PLANE,
                bReserved = 0,
                dwLayerMask = 0,
                dwVisibleMask = 0,
                dwDamageMask = 0,
            };
            IntPtr hDC = GetDC(hWnd);
            var pixelFormat = Win32.ChoosePixelFormat(hDC, pfd);
            Win32.SetPixelFormat(hDC, pixelFormat, pfd);

            var ctrl = new OpenGLControl();
            ctrl.OpenGL.Create(
                OpenGLVersion.OpenGL2_1, RenderContextType.FBO,
                (int)Width, (int)Height, 8, hWnd
            );
            ctrl.OpenGLInitialized += OpenGLControl_OnOpenGLInitialized;
            ctrl.OpenGLDraw += OpenGLControl_OnOpenGLDraw;
            myGrid.Children.Add(ctrl);
        }

        private void OpenGLControl_OnOpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
        {
            var gl = args.OpenGL;
            gl.Enable(OpenGL.GL_ALPHA_TEST);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);

            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);

            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.ClearColor(0, 0, 0, 0);
            _scene.Initialize(gl, (float)Width, (float)Height);
        }

        private void OpenGLControl_OnOpenGLDraw(object sender, OpenGLRoutedEventArgs args)
        {
            _scene.Draw(args.OpenGL);
        }
    }
}
