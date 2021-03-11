using SharpGL;

namespace WorldMapper
{
    public class TerrainObject : MeshObject
    {
        public TerrainObject()
        {
            Vertices = new[]
            {
                0f, 0f, 0f,
                1f, 0f, 0f,
                1f, 1f, 0f
            };
        }
    }
}
