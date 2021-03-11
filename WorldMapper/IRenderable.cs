using System.Numerics;
using SharpGL;
using SharpGL.VertexBuffers;

namespace WorldMapper
{
    public interface IRenderable
    {
        VertexBufferArray BufferArray { get; }
        Matrix4x4 Transform { get; }
        void BindData(OpenGL gl);
    }
}
