using System;
using SharpGL;
using SharpGL.VertexBuffers;
using WorldMapper.Shaders;

namespace WorldMapper.World
{
    public class TerrainObject : MeshObjectBase
    {
        private float[] _barycentric;
        private VertexBuffer _barycentricBuffer;

        public TerrainObject(OpenGL gl)
        {
            GenerateGeometry();
            CreateBuffers(gl);
            CreateCustomBuffers(gl);
        }

        private void GenerateGeometry()
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

        private void CreateCustomBuffers(OpenGL gl)
        {
            BufferArray.Bind(gl);

            _barycentricBuffer = new VertexBuffer();
            _barycentricBuffer.Create(gl);
            UpdateBarycentricData(gl);

            BufferArray.Unbind(gl);
        }

        private void UpdateBarycentricData(OpenGL gl, bool removeEdge = false)
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

            _barycentricBuffer.Bind(gl);
            _barycentricBuffer.SetData(gl, WireframeShader.AttributeIndexBarycentric, _barycentric, false, 3);
            _barycentricBuffer.Unbind(gl);
        }
    }
}
