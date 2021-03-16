using System;
using SharpGL;
using System.Numerics;
using WorldMapper.Shaders;

namespace WorldMapper
{
    /// <summary>
    /// Terrain model scene
    /// </summary>
    public class Scene
    {
        private Matrix4x4 _projectionMatrix;
        private Matrix4x4 _viewMatrix;

        private WireframeShader _shader;

        private IRenderable[] _objects;
        private long _lastTime;
        private float _deltaTime;

        private const float PI = (float)Math.PI;
        private readonly float _distance = 5;
        private float _rotation;
        private Matrix4x4 _transMat;

        public Scene()
        {
            _transMat = Matrix4x4.CreateTranslation(0, 0, -_distance);
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
            _shader = new WireframeShader(gl);
            _shader.Bind(gl);
            _shader.InitializeAllUniforms(gl);
            _shader.SetFill(gl, 0.5f, 0, 0);
            _shader.SetStroke(gl, 0, 1, 1);
            _shader.SetThickness(gl, 0.04f);
            _shader.Unbind(gl);

            //  Create a perspective projection matrix.
            const float rads = (60f / 180f) * (float)Math.PI;
            _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(
                rads, width/height, 0.1f, 100f
            );

            // Let each object set up its data
            foreach (var obj in _objects)
            {
                obj.CreateBuffers(gl);
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

            _shader.Bind(gl);

            TransformViewMatrix();
            _shader.SetMatrix(gl, "projectionMatrix", _projectionMatrix);
            _shader.SetMatrix(gl, "viewMatrix", _viewMatrix);

            // Draw each object
            foreach (var obj in _objects)
            {
                _shader.SetMatrix(gl, "modelMatrix", obj.Transform);
                obj.BufferArray.Bind(gl);
                gl.DrawArrays(OpenGL.GL_TRIANGLES, 0, obj.VertexCount);
                obj.BufferArray.Unbind(gl);
            }

            _shader.Unbind(gl);
        }

        private void TransformViewMatrix()
        {
            _rotation += _deltaTime * PI / 4;
            _viewMatrix = Matrix4x4.CreateFromYawPitchRoll(_rotation, 0f, 0f)
                * Matrix4x4.CreateFromYawPitchRoll(0f, _rotation/1.2f, 0f)
                * _transMat;
        }
    }
}
