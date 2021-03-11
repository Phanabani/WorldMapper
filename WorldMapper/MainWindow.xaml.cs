using SharpGL;
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
            _scene = new Scene();
            InitializeComponent();
        }

        private void OpenGLControl_OnOpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
        {
            var gl = args.OpenGL;
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            _scene.Initialize(gl, (float)Width, (float)Height);
        }

        private void OpenGLControl_OnOpenGLDraw(object sender, OpenGLRoutedEventArgs args)
        {
            _scene.Draw(args.OpenGL);
        }
    }
}
