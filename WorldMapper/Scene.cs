using System;
using SharpGL;
using SharpGL.Shaders;
using System.Numerics;

using static WorldMapper.Utils;

namespace WorldMapper
{
    /// <summary>
    /// Terrain model scene
    /// </summary>
    public class Scene
    {
        Matrix4x4 projectionMatrix;
        Matrix4x4 viewMatrix;

        //  Constants that specify the attribute indexes.
        const uint attributeIndexPosition = 0;
        const uint attributeIndexColor = 1;

        //  The shader program for our vertex and fragment shader.
        private ShaderProgram shaderProgram;

        private IRenderable[] objects;

        public Scene()
        {
            objects = new IRenderable[]
            {
                new TerrainObject(),
                new TerrainObject
                {
                    Transform = Matrix4x4.CreateTranslation(-2, 0, 0)
                },
            };
        }

        /// <summary>
        /// Initialises the scene.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        public void Initialize(OpenGL gl, float width, float height)
        {
            //  Set a blue clear colour.
            gl.ClearColor(0.4f, 0.6f, 0.9f, 0.0f);

            //  Create the shader program.
            var vertexShaderSource = ManifestResourceLoader.LoadTextFile("Shader.vert");
            var fragmentShaderSource = ManifestResourceLoader.LoadTextFile("Shader.frag");
            shaderProgram = new ShaderProgram();
            shaderProgram.Create(gl, vertexShaderSource, fragmentShaderSource, null);
            shaderProgram.BindAttributeLocation(gl, attributeIndexPosition, "in_Position");
            shaderProgram.BindAttributeLocation(gl, attributeIndexColor, "in_Color");
            shaderProgram.AssertValid(gl);

            //  Create a perspective projection matrix.
            const float rads = (60f / 180f) * (float)Math.PI;
            projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(
                rads, width/height, 0.1f, 100f
            );

            //  Create a view matrix to move us back a bit.
            viewMatrix = Matrix4x4.CreateTranslation(0, 0, -5);

            // Let each object set up its data
            foreach (var obj in objects)
            {
                obj.BindData(gl);
            }
        }

        /// <summary>
        /// Draws the scene.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);

            shaderProgram.Bind(gl);

            // Set unchanging matrix transformations
            shaderProgram.SetUniformMatrix4(gl, "projectionMatrix", MatrixToArray(projectionMatrix));
            shaderProgram.SetUniformMatrix4(gl, "viewMatrix", MatrixToArray(viewMatrix));

            // Draw each object
            foreach (var obj in objects)
            {
                shaderProgram.SetUniformMatrix4(gl, "modelMatrix", MatrixToArray(obj.Transform));
                obj.BufferArray.Bind(gl);
                gl.DrawArrays(OpenGL.GL_TRIANGLES, 0, obj.VertexCount);
                obj.BufferArray.Unbind(gl);
            }

            shaderProgram.Unbind(gl);
        }
    }
}
