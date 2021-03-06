using System;
using System.Numerics;
using SharpGL;
using SharpGL.VertexBuffers;
using WorldMapper.Shaders;

namespace WorldMapper.World
{
    public class MeshObject : IDrawable
    {
        public float[] Vertices { get; set; } = new float[0];
        public int VertexCount => Vertices.Length;
        public VertexBufferArray BufferArray { get; private set; }
        public Transform Transform { get; set; } = new Transform();
        public ShaderBase Shader { get; set; }

        protected VertexBuffer VertexDataBuffer;

        public MeshObject(OpenGL gl)
        {
            CreateBuffers(gl);
        }

        public MeshObject(OpenGL gl, float[] vertices) : this(gl)
        {
            Vertices = vertices;
            FlushMesh(gl);
        }

        public void CreateBuffers(OpenGL gl)
        {
            BufferArray = new VertexBufferArray();
            BufferArray.Create(gl);
            BufferArray.Bind(gl);

            VertexDataBuffer = new VertexBuffer();
            VertexDataBuffer.Create(gl);

            BufferArray.Unbind(gl);
        }

        public void FlushMesh(OpenGL gl)
        {
            BufferArray.Bind(gl);
            VertexDataBuffer.Bind(gl);
            VertexDataBuffer.SetData(gl, 0, Vertices, false, 3);
            VertexDataBuffer.Unbind(gl);
            BufferArray.Unbind(gl);
        }

        public void Draw(OpenGL gl)
        {
            if (Shader is null)
                throw new InvalidOperationException("Missing shader");

            Shader.Bind(gl);

            SetShaderUniforms(gl);
            Shader.SetModel(gl, Transform.Matrix);

            BufferArray.Bind(gl);
            gl.DrawArrays(OpenGL.GL_TRIANGLES, 0, VertexCount);
            BufferArray.Unbind(gl);

            Shader.Unbind(gl);
        }

        protected virtual void SetShaderUniforms(OpenGL gl) { }
    }
}
