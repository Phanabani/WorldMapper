using System.Numerics;
using SharpGL;
using SharpGL.Shaders;
using static WorldMapper.Utils;

namespace WorldMapper.Shaders
{
    public abstract class ShaderBase
    {
        protected abstract string VertexFileName { get; }
        protected abstract string FragmentFileName { get; }

        public ShaderProgram Shader { get; private set; }

        protected abstract void DoBindings(OpenGL gl);

        protected void CreateShader(OpenGL gl)
        {
            var vertexShaderSource = ManifestResourceLoader.LoadTextFile(VertexFileName);
            var fragmentShaderSource = ManifestResourceLoader.LoadTextFile(FragmentFileName);
            Shader = new ShaderProgram();
            Shader.Create(gl, vertexShaderSource, fragmentShaderSource, null);
            DoBindings(gl);
            Shader.AssertValid(gl);
        }

        public void Bind(OpenGL gl) => Shader.Bind(gl);
        public void Unbind(OpenGL gl) => Shader.Unbind(gl);

        public void SetMatrix(OpenGL gl, string uniformName, Matrix4x4 mat)
        {
            Shader.SetUniformMatrix4(gl, uniformName, MatrixToArray(mat));
        }
    }
}
