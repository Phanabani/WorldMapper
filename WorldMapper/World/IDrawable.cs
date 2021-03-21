using System.Numerics;
using SharpGL;
using SharpGL.VertexBuffers;
using WorldMapper.Shaders;

namespace WorldMapper.World
{
    public interface IDrawable : ITransformable
    {
        VertexBufferArray BufferArray { get; }
        int VertexCount { get; }
        ShaderBase Shader { get; set; }

        void CreateBuffers(OpenGL gl);
        void Draw(OpenGL gl);
    }
}
