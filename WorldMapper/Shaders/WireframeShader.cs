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
            SetDifferentBackfaceColor(gl);
            SetFillBackface(gl);
            SetStrokeBackface(gl);

            SetFixedWidthMix(gl);
            SetThickness(gl);
            SetDualStroke(gl);
            SetSecondThickness(gl);

            SetTime(gl);
            SetDashed(gl);
            SetDashCount(gl);
            SetDashLength(gl);
            SetDashOverlap(gl);
            SetDashAnimate(gl);

            SetSqueeze(gl);
            SetSqueezeMin(gl);
            SetSqueezeMax(gl);
        }

        public void SetFill(OpenGL gl, float r = 1f, float g = 1f, float b = 1f, float a = 0f)
        {
            gl.Uniform4(Shader.GetUniformLocation(gl, "fill"), r, g, b, a);
        }

        public void SetStroke(OpenGL gl, float r = 1f, float g = 1f, float b = 1f, float a = 1f)
        {
            gl.Uniform4(Shader.GetUniformLocation(gl, "stroke"), r, g, b, a);
        }

        public void SetDifferentBackfaceColor(OpenGL gl, bool enabled = false)
        {
            gl.Uniform1(Shader.GetUniformLocation(gl, "differentBackfaceColor"), enabled ? 1 : 0);
        }

        public void SetFillBackface(OpenGL gl, float r = 1f, float g = 1f, float b = 1f, float a = 0f)
        {
            gl.Uniform4(Shader.GetUniformLocation(gl, "fillBackface"), r, g, b, a);
        }

        public void SetStrokeBackface(OpenGL gl, float r = 1f, float g = 1f, float b = 1f, float a = 1f)
        {
            gl.Uniform4(Shader.GetUniformLocation(gl, "strokeBackface"), r, g, b, a);
        }

        /// <summary>
        /// Mix between dynamic (0.0) and fixed (1.0) width wireframes.
        /// Be aware that thickness units are different between these two
        /// systems: dynamic uses barycentric units where 1/3 is the maximum
        /// thickness, whereas fixed uses (what looks like) screen-space pixel
        /// units.
        /// </summary>
        public void SetFixedWidthMix(OpenGL gl, float mix = 0f) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "fixedWidthMix"), mix);
        }

        public void SetThickness(OpenGL gl, float thickness = 0.02f) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "thickness"), thickness);
        }

        public void SetDualStroke(OpenGL gl, bool enabled = false) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "dualStroke"), enabled ? 1 : 0);
        }

        public void SetSecondThickness(OpenGL gl, float thickness = 0.05f) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "secondThickness"), thickness);
        }

        public void SetTime(OpenGL gl, float time = 0f) {
            gl.Uniform1(Shader.GetUniformLocation(gl, "time"), time);
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
