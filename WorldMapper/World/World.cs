using System;
using System.Collections.Generic;
using SharpGL;
using WorldMapper.Shaders;

namespace WorldMapper.World
{
    public class World
    {
        public List<IRenderable> Objects { get; set; } = new List<IRenderable>();
        public List<ShaderBase> Shaders { get; set; } = new List<ShaderBase>();
        public Camera Camera { get; set; } = null;

        public void Draw(OpenGL gl)
        {
            if (Camera is null)
                throw new InvalidOperationException("No camera in the world");
            foreach (var shader in Shaders)
            {
                shader.Bind(gl);
                shader.SetProjection(gl, Camera.ProjectionMatrix);
                shader.SetView(gl, Camera.Transform.Matrix);
                shader.Unbind(gl);
            }
            foreach (var obj in Objects)
            {
                obj.Draw(gl, Camera.ProjectionMatrix, Camera.Transform.Matrix);
            }
        }
    }
}
