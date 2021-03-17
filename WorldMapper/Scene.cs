using System;
using SharpGL;
using System.Numerics;
using WorldMapper.Shaders;
using WorldMapper.World;

namespace WorldMapper
{
    /// <summary>
    /// Terrain model scene
    /// </summary>
    public class Scene
    {
        private World.World _world = new World.World();
        private GameMemoryReader _memoryReader;

        private long _lastTime;
        private float _deltaTime;

        private readonly Matrix4x4 _camAdjust = Matrix4x4.CreateScale(-1, 1, 1);

        public Scene()
        {
            _memoryReader = new GameMemoryReader(
                "pcsx2", 0x20189EA0, 0x201BBD70, 0x201B9840
            );
        }

        private void SampleTime()
        {
            var now = DateTime.UtcNow;
            long nowTicks = now.Ticks;
            if (_lastTime != 0)
                // Ticks equal 100ns AKA 10,000,000ths of a second
                _deltaTime = (nowTicks - _lastTime) / 10_000_000f;
            _lastTime = nowTicks;
        }

        /// <summary>
        /// Initialises the scene.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        public void Initialize(OpenGL gl, float width, float height)
        {
            //  Create the shader program.
            var wireframe = new WireframeShader(gl);
            wireframe.Bind(gl);

            wireframe.InitializeAllUniforms(gl);
            wireframe.SetFill(gl, 1, 0, 0, 0.5f);
            wireframe.SetStroke(gl, 0, 1, 1, 1);
            wireframe.SetDifferentBackfaceColor(gl, true);
            wireframe.SetFillBackface(gl, 1, 0, 1, 0.5f);
            wireframe.SetStrokeBackface(gl, 0, 1, 0, 1);

            wireframe.SetFixedWidthMix(gl, 0.8f);
            wireframe.SetThickness(gl, 0.3f);

            wireframe.Unbind(gl);

            _world.Shaders.Add(wireframe);

            _world.Objects.AddRange(new []
            {
                new TerrainObject(gl)
                {
                    Shader = wireframe,
                    Transform = new Transform
                    {
                        // Position = new Vector3(300, 226, 128)
                        Position = new Vector3(0, 0, 0),
                    }
                },
            });

            _world.Camera = new Camera(width, height, 90);
            _world.Camera.Transform.Position = new Vector3(0, 0, 4);
        }

        /// <summary>
        /// Draws the scene.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void Draw(OpenGL gl)
        {
            _memoryReader.ReadCameraPosition();
            _memoryReader.ReadCameraRotation();

            SampleTime();

            gl.Clear(
                OpenGL.GL_COLOR_BUFFER_BIT
                | OpenGL.GL_DEPTH_BUFFER_BIT
                | OpenGL.GL_STENCIL_BUFFER_BIT
            );

            _world.Objects[0].Transform.Matrix = _memoryReader.CameraRotMatrix * _camAdjust;

            _world.Draw(gl);
        }
    }
}
