using System;
using SharpGL;
using SharpGL.Shaders;
using System.Numerics;

using static WorldMapper.Utils;

namespace WorldMapper
{
    /// <summary>
    /// Terrain model scene
    /// </summary>
    public class Scene
    {
        private Matrix4x4 _projectionMatrix;
        private Matrix4x4 _viewMatrix;

        private ShaderProgram _shaderProgram;
        private const uint AttributeIndexPosition = 0;
        private const uint AttributeIndexColor = 1;

        private IRenderable[] _objects;
        private long _lastTime;
        private float _deltaTime;

        private const float PI = (float)Math.PI;
        private readonly float _distance = 5;
        private float _rotation;

        public Scene()
        {
            _objects = new IRenderable[]
            {
                new TerrainObject(),
                new TerrainObject
                {
                    Transform = Matrix4x4.CreateTranslation(-2, 0, 0)
                },
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
            //  Create the shader program.
            var vertexShaderSource = ManifestResourceLoader.LoadTextFile("Shader.vert");
            var fragmentShaderSource = ManifestResourceLoader.LoadTextFile("Shader.frag");
            _shaderProgram = new ShaderProgram();
            _shaderProgram.Create(gl, vertexShaderSource, fragmentShaderSource, null);
            _shaderProgram.BindAttributeLocation(gl, AttributeIndexPosition, "in_Position");
            _shaderProgram.BindAttributeLocation(gl, AttributeIndexColor, "in_Color");
            _shaderProgram.AssertValid(gl);

            //  Create a perspective projection matrix.
            const float rads = (60f / 180f) * (float)Math.PI;
            _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(
                rads, width/height, 0.1f, 100f
            );

            // Let each object set up its data
            foreach (var obj in _objects)
            {
                obj.BindData(gl);
            }
        }

        /// <summary>
        /// Draws the scene.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void Draw(OpenGL gl)
        {
            SampleTime();
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);

            _shaderProgram.Bind(gl);

            _shaderProgram.SetUniformMatrix4(gl, "projectionMatrix", MatrixToArray(_projectionMatrix));

            TransformViewMatrix();
            _shaderProgram.SetUniformMatrix4(gl, "viewMatrix", MatrixToArray(_viewMatrix));

            // Draw each object
            foreach (var obj in _objects)
            {
                _shaderProgram.SetUniformMatrix4(gl, "modelMatrix", MatrixToArray(obj.Transform));
                obj.BufferArray.Bind(gl);
                gl.DrawArrays(OpenGL.GL_TRIANGLES, 0, obj.VertexCount);
                obj.BufferArray.Unbind(gl);
            }

            _shaderProgram.Unbind(gl);
        }

        private void TransformViewMatrix()
        {
            _rotation += _deltaTime * PI;
            _viewMatrix = Matrix4x4.CreateFromYawPitchRoll(_rotation, 0f, 0f)
                * Matrix4x4.CreateTranslation(0f, 0f, -_distance);
        }
    }
}
