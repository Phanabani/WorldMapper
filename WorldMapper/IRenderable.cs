using System.Numerics;
using SharpGL;
using SharpGL.VertexBuffers;

namespace WorldMapper
{
    public interface IRenderable
    {
        VertexBufferArray BufferArray { get; }
        int VertexCount { get; }
        Matrix4x4 Transform { get; set; }
        void BindData(OpenGL gl);
    }
}
