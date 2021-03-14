using System.Net.Configuration;
using System.Windows;
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
        private OverlayWindowRenderContextProvider overlayRCP;

        public MainWindow()
        {
            InitializeComponent();
            _scene = new Scene();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void OpenGLControl_OnOpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
        {
            var gl = args.OpenGL;
            // overlayRCP = OverlayWindowRenderContextProvider.CustomCreate(
            //     gl, OpenGLVersion.OpenGL2_1, (int)Width, (int)Height
            // );

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
