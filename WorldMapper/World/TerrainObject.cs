using System;
using SharpGL;
using SharpGL.VertexBuffers;
using WorldMapper.Shaders;

namespace WorldMapper.World
{
    public class TerrainObject : MeshObjectBase
    {
        private float[] _barycentric;
        protected VertexBuffer BarycentricBuffer;

        public TerrainObject()
        {
            Vertices = new[]
            {
                0f, 0f, 0f,
                0f, 0f, -1f,
                0f, 1f, -1f,

                -0.5f, 0f, 0f,
                0.5f, 0f, 0f,
                0f, 1f, 0f,
            };
        }

        protected override void CreateCustomBuffers(OpenGL gl)
        {
            BarycentricBuffer = new VertexBuffer();
            BarycentricBuffer.Create(gl);
            UpdateBarycentricData(gl);
        }

        protected void UpdateBarycentricData(OpenGL gl, bool removeEdge = false)
        {
            _barycentric = new float[VertexCount * 3];
            var Q = removeEdge ? 1f : 0f;

            var even = true;
            for (var i = 0; i < VertexCount * 3; i += 9)
            {
                float[] baryCoords;
                if (even)
                {
                    baryCoords = new[]
                    {
                        0f, 0f, 1f,
                        0f, 1f, 0f,
                        1f, 0f, Q,
                    };
                }
                else
                {
                    baryCoords = new[]
                    {
                        0f, 1f, 0f,
                        0f, 0f, 1f,
                        1f, 0f, Q,
                    };
                }

                Array.Copy(baryCoords, 0, _barycentric, i, 9);
                even = !even;
            }

            BarycentricBuffer.Bind(gl);
            BarycentricBuffer.SetData(gl, WireframeShader.AttributeIndexBarycentric, _barycentric, false, 3);
        }
    }
}
