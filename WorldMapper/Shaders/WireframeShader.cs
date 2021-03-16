using System.Numerics;
using SharpGL;

namespace WorldMapper.Shaders
{
    public class WireframeShader : ShaderBase
    {
        public const int AttributeIndexPosition = 0;
        public const int AttributeIndexBarycentric = 1;

        protected override string VertexFileName => "Shaders/wireframe.vert";
        protected override string FragmentFileName => "Shaders/wireframe.frag";

        public WireframeShader(OpenGL gl)
        {
            CreateShader(gl);
        }

        protected override void DoBindings(OpenGL gl)
        {
            Shader.BindAttributeLocation(gl, AttributeIndexPosition, "in_Position");
            Shader.BindAttributeLocation(gl, AttributeIndexBarycentric, "barycentric");
        }

        // Uniforms

        public void InitializeAllUniforms(OpenGL gl)
        {
            SetFill(gl);
            SetStroke(gl);

            SetTime(gl);

            SetThickness(gl);
            SetSecondThickness(gl);

            SetDualStroke(gl);
            SetSeeThrough(gl);
            SetInsideAltColor(gl);

            SetDashed(gl);
            SetDashCount(gl);
            SetDashLength(gl);
            SetDashOverlap(gl);
            SetDashAnimate(gl);

            SetSqueeze(gl);
            SetSqueezeMin(gl);
            SetSqueezeMax(gl);
        }

        public void SetFill(OpenGL gl, float r = 0.5f, float g = 0.5f, float b = 0.5f)
        {
            gl.Uniform3(Shader.GetUniformLocation(gl, "fill"), r, g, b);
        }

        public void SetStroke(OpenGL gl, float r = 0.5f, float g = 0.5f, float b = 0.5f)
        {
            gl.Uniform3(Shader.GetUniformLocation(gl, "stroke"), r, g, b);
        }

        public void SetTime(OpenGL gl, float time = 0f) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "time"), time);
        }

        public void SetThickness(OpenGL gl, float thickness = 0.02f) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "thickness"), thickness);
        }

        public void SetSecondThickness(OpenGL gl, float thickness = 0.05f) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "secondThickness"), thickness);
        }

        public void SetDualStroke(OpenGL gl, bool enabled = false) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "dualStroke"), enabled ? 1 : 0);
        }

        public void SetSeeThrough(OpenGL gl, bool enabled = true) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "seeThrough"), enabled ? 1 : 0);
        }

        public void SetInsideAltColor(OpenGL gl, bool enabled = false) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "insideAltColor"), enabled ? 1 : 0);
        }

        public void SetDashed(OpenGL gl, bool enabled = false) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "dashed"), enabled ? 1 : 0);
        }

        public void SetDashCount(OpenGL gl, float count = 4f) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "dashCount"), count);
        }

        /// <param name="length">The length ratio from 0 (no dashes) to 1 (fully connected)</param>
        public void SetDashLength(OpenGL gl, float length = 0.5f) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "dashLength"), length);
        }

        public void SetDashOverlap(OpenGL gl, bool enabled = false) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "dashOverlap"), enabled ? 1 : 0);
        }

        public void SetDashAnimate(OpenGL gl, bool enabled = false) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "dashAnimate"), enabled ? 1 : 0);
        }

        public void SetSqueeze(OpenGL gl, bool enabled = false) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "squeeze"), enabled ? 1 : 0);
        }

        public void SetSqueezeMin(OpenGL gl, float ratio = 0.1f) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "squeezeMin"), ratio);
        }

        public void SetSqueezeMax(OpenGL gl, float ratio = 1f) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "squeezeMax"), ratio);
        }
    }
}
