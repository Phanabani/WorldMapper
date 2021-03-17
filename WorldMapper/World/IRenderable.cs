using System.Numerics;
using SharpGL;
using SharpGL.VertexBuffers;
using WorldMapper.Shaders;

namespace WorldMapper.World
{
    public interface IRenderable
    {
        VertexBufferArray BufferArray { get; }
        int VertexCount { get; }
        Transform Transform { get; set; }
        ShaderBase Shader { get; set; }

        void CreateBuffers(OpenGL gl);
        void Draw(OpenGL gl, Matrix4x4 projection, Matrix4x4 view);
    }
}
