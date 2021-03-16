using SharpGL;
using SharpGL.VertexBuffers;
using WorldMapper.WorldMath;

namespace WorldMapper
{
    public interface IRenderable
    {
        VertexBufferArray BufferArray { get; }
        int VertexCount { get; }
        Transform Transform { get; set; }
        void CreateBuffers(OpenGL gl);
    }
}
