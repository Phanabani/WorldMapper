using System;
using SharpGL;
using SharpGL.VertexBuffers;
using WorldMapper.Shaders;

namespace WorldMapper.World
{
    public class WireframeMeshObject : MeshObject
    {
        private float[] _barycentric;
        private VertexBuffer _barycentricBuffer;

        public WireframeMeshObject(OpenGL gl) : base(gl)
        {
            CreateCustomBuffers(gl);
        }

        public WireframeMeshObject(OpenGL gl, float[] vertices) : this(gl)
        {
            Vertices = vertices;
            FlushMesh(gl);
            UpdateBarycentricData(gl);
        }

        private void CreateCustomBuffers(OpenGL gl)
        {
            BufferArray.Bind(gl);
            _barycentricBuffer = new VertexBuffer();
            _barycentricBuffer.Create(gl);
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

            BufferArray.Bind(gl);
            _barycentricBuffer.Bind(gl);
            _barycentricBuffer.SetData(
                gl, WireframeShader.AttributeIndexBarycentric, _barycentric,
                false, 3
            );
            _barycentricBuffer.Unbind(gl);
            BufferArray.Unbind(gl);
        }
    }
}
