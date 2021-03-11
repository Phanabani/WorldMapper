using System.Numerics;
using SharpGL;
using SharpGL.VertexBuffers;

namespace WorldMapper
{
    public class MeshObject : IRenderable
    {
        protected float[] Vertices, Colors;

        public VertexBufferArray BufferArray { get; private set; }
        public Matrix4x4 Transform { get; private set; }

        public MeshObject()
        {
            Transform = Matrix4x4.Identity;
        }

        public void BindData(OpenGL gl)
        {
            //  Create the vertex array object.
            BufferArray = new VertexBufferArray();
            BufferArray.Create(gl);
            BufferArray.Bind(gl);

            //  Create a vertex buffer for the vertex data.
            var vertexDataBuffer = new VertexBuffer();
            vertexDataBuffer.Create(gl);
            vertexDataBuffer.Bind(gl);
            vertexDataBuffer.SetData(gl, 0, Vertices, false, 3);

            // var colorDataBuffer = new VertexBuffer();
            // colorDataBuffer.Create(gl);
            // colorDataBuffer.Bind(gl);
            // colorDataBuffer.SetData(gl, 1, Colors, false, 3);

            //  Unbind the vertex array, we've finished specifying data for it.
            BufferArray.Unbind(gl);
        }
    }
}
