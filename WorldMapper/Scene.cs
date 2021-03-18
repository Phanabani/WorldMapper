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

        public Scene()
        {
            _memoryReader = new GameMemoryReader
            {
                UpAxis = 'z',
                ProcessName = "pcsx2",
                CharacterPosAddress = 0x20189EA0,
                CameraPosAddress = 0x201B9980,
                CameraRotMatrixAddress = 0x201B9840
            };
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
            // Create the shader program.
            var wireframe = new WireframeShader(gl);
            wireframe.Bind(gl);

            wireframe.InitializeAllUniforms(gl);
            wireframe.SetFill(gl, 1, 0, 0, 0.5f);
            wireframe.SetStroke(gl, 0, 1, 1, 1);
            wireframe.SetDifferentBackfaceColor(gl, true);
            wireframe.SetFillBackface(gl, 1, 0, 1, 0.5f);
            wireframe.SetStrokeBackface(gl, 0, 1, 0, 1);

            // wireframe.SetFixedWidthMix(gl, 0.8f);
            // wireframe.SetThickness(gl, 0.3f);
            wireframe.SetFixedWidthMix(gl, 1f);
            wireframe.SetThickness(gl, 4f);

            wireframe.Unbind(gl);

            _world.Shaders.Add(wireframe);

            _world.Objects.AddRange(new []
            {
                new TerrainObject(gl)
                {
                    Shader = wireframe,
                    Transform = new Transform
                    {
                        Position = new Vector3(300, 128, -225)
                    }
                },
                // Ratchet's ship
                new TerrainObject(gl)
                {
                    Shader = wireframe,
                    Transform = new Transform
                    {
                        Position = new Vector3(300, 129, -208)
                    }
                },
            });

            var PI = (float) Math.PI;

            _world.Camera = new Camera(width, height, 90)
            {
                Transform =
                {
                    // Position = new Vector3(300, 160, -225),
                    Position = new Vector3(300, 140, -208),
                    Rotation = Quaternion.CreateFromYawPitchRoll(0, PI/2, 0)
                }
            };
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

            var trans = _world.Objects[0].Transform;
            trans.Matrix = _memoryReader.CameraMatrix;

            _world.Draw(gl);
        }
    }
}
