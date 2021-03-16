﻿using SharpGL;
using SharpGL.VertexBuffers;
using WorldMapper.WorldMath;

namespace WorldMapper
{
    public abstract class MeshObjectBase : IRenderable
    {
        protected float[] Vertices;

        public int VertexCount => Vertices.Length;
        public VertexBufferArray BufferArray { get; private set; }
        public Transform Transform { get; set; }

        protected VertexBuffer VertexDataBuffer;

        public MeshObjectBase()
        {
            Transform = new Transform();
        }

        public void CreateBuffers(OpenGL gl)
        {
            //  Create the vertex array object.
            BufferArray = new VertexBufferArray();
            BufferArray.Create(gl);
            BufferArray.Bind(gl);

            //  Create a vertex buffer for the vertex data.
            VertexDataBuffer = new VertexBuffer();
            VertexDataBuffer.Create(gl);
            VertexDataBuffer.Bind(gl);
            VertexDataBuffer.SetData(gl, 0, Vertices, false, 3);

            CreateCustomBuffers(gl);

            //  Unbind the vertex array, we've finished specifying data for it.
            BufferArray.Unbind(gl);
        }

        protected virtual void CreateCustomBuffers(OpenGL gl) { }
    }
}
