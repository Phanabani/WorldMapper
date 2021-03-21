using System;
using SharpGL;
using System.Numerics;
using WorldMapper.Shaders;
using WorldMapper.World;
using static WorldMapper.MathUtils;

namespace WorldMapper
{
    /// <summary>
    /// Terrain model scene
    /// </summary>
    public class Scene
    {
        private const float PI = (float) Math.PI;

        private World.World _world = new World.World();
        private GameMemoryReader _memoryReader;

        public Scene()
        {
            _memoryReader = new GameMemoryReader
            {
                UpAxis = 'z',
                ProcessName = "pcsx2",
                CharacterPosAddress = 0x20189EA0,
                CameraPosAddress = 0x201BA500,
                CameraRotAddress = 0x201BA510,
                CameraRotMatrixAddress = 0x201BA730,
                CameraMatrixAddress = 0x201BB270
            };
        }

        /// <summary>
        /// Initialises the scene.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        public void Initialize(OpenGL gl, float width, float height)
        {
            #region Shader
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
            #endregion

            #region Geometry
            // Add objects to the world
            var verts = new[]
            {
                // YZ plane (pointing forward)
                0f, 0f,  0f,
                0f, 0f, -1f,
                0f, 1f, -1f,

                // XY plane (pointing up)
                -0.5f, 0f, 0f,
                0.5f, 0f, 0f,
                0f,  1f, 0f,

                // XZ plane (ground, pointing forward)
                -1f, 0f, 0f,
                1f, 0f, 0f,
                0f, 0f, -0.5f,
            };
            #endregion

            #region Main world
            _world.Objects.AddRange(new []
            {
                // Origin
                new WireframeMeshObject(gl, verts)
                {
                    Shader = wireframe,
                    Transform = new Transform
                    {
                        Position = new Vector3(0, 128, 0)
                    }
                },

                // Ratchet's ship
                new WireframeMeshObject(gl, verts)
                {
                    Shader = wireframe,
                    Transform = new Transform
                    {
                        Position = new Vector3(510, 120, -312.5f)
                    }
                },

                // Maktar center circle near light bridge
                new WireframeMeshObject(gl, verts)
                {
                    Shader = wireframe,
                    Transform = new Transform
                    {
                        Position = new Vector3(500, 110, -358)
                    }
                },

                // Maktar circle thing right
                new WireframeMeshObject(gl, verts)
                {
                    Shader = wireframe,
                    Transform = new Transform
                    {
                        Position = new Vector3(508, 110, -352)
                    }
                },

                // Maktar circle thing left
                new WireframeMeshObject(gl, verts)
                {
                    Shader = wireframe,
                    Transform = new Transform
                    {
                        Position = new Vector3(492f, 110, -352)
                    }
                },
            });

            // Add a camera
            _world.Camera = new Camera(width, height, 45, clipFar: 300f)
            {
                Transform =
                {
                    // Position = new Vector3(300, 160, -225),
                    Position = new Vector3(300, 140, -208),
                    Rotation = Quaternion.CreateFromYawPitchRoll(0, 0, 0)
                }
            };
            #endregion
        }

        /// <summary>
        /// Draws the scene.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void Draw(OpenGL gl)
        {
            gl.Clear(
                OpenGL.GL_COLOR_BUFFER_BIT
                | OpenGL.GL_DEPTH_BUFFER_BIT
                | OpenGL.GL_STENCIL_BUFFER_BIT
            );

            _memoryReader.ReadCameraPosition();
            _memoryReader.ReadCameraRotation();

            var trans = _world.Camera.Transform;
            trans.Rotation = _memoryReader.CameraRot;
            trans.Position = _memoryReader.CameraPos;

            var euler = MatrixToEuler(trans.Matrix) * 180f / PI;
            gl.DrawText(20, 50, 1f, 0f, 0f, "Courier New", 24f, trans.Position.ToString("F03"));
            gl.DrawText(20, 20, 1f, 0f, 0f, "Courier New", 24f, euler.ToString("F03"));
            _world.Draw(gl);
        }
    }
}
