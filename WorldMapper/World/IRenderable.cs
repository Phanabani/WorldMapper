using SharpGL;
using SharpGL.VertexBuffers;

namespace WorldMapper.World
{
    public interface IRenderable
    {
        VertexBufferArray BufferArray { get; }
        int VertexCount { get; }
        Transform Transform { get; set; }
        void CreateBuffers(OpenGL gl);
    }
}
