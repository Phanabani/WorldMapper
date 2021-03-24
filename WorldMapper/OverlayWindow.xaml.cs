using System;
using System.Windows;
using SharpGL;
using SharpGL.WPF;
using WorldMapper.Pages;

namespace WorldMapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class OverlayWindow
    {
        public ControlsData ControlsData
        {
            get => _controlsData;
            set => _controlsData = value;
        }

        private readonly Scene _scene;

        private ControlsData _controlsData;

        public OverlayWindow()
        {
            OpenGLTransparencyPatcher.DoPatching();
            InitializeComponent();
            _scene = new Scene();
        }

        private void MainWindow_OnSourceInitialized(object sender, EventArgs e)
        {
            WindowsServices.SetWindowExTransparent(this);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

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
