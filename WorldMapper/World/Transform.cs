using System.Numerics;

namespace WorldMapper.World
{
    public class Transform
    {
        public Vector3 Position
        {
            get => _position;
            set => SetPosition(value);
        }

        public Quaternion Rotation
        {
            get => _rotation;
            set => SetRotation(value);
        }

        public Vector3 Scale
        {
            get => _scale;
            set => SetScale(value);
        }

        public Matrix4x4 Matrix
        {
            get => GetMatrix();
            set => SetMatrix(value);
        }

        private Vector3 _position = Vector3.Zero;
        private Quaternion _rotation = Quaternion.Identity;
        private Vector3 _scale = Vector3.One;
        private Matrix4x4 _matrix = Matrix4x4.Identity;

        private bool _dirty;

        /// <summary>
        /// Modifying a property of one of the vector/quaternion properties
        /// will not mark it as dirty, so the matrix will not be updated.
        /// This method will force it to update.
        /// </summary>
        public Matrix4x4 ForceUpdateMatrix()
        {
            var trans = Matrix4x4.CreateTranslation(_position);
            var rot = Matrix4x4.CreateFromQuaternion(_rotation);
            var scale = Matrix4x4.CreateScale(_scale);
            _matrix = trans * rot * scale;
            _dirty = false;
            return _matrix;
        }

        private Matrix4x4 GetMatrix()
        {
            return _dirty ? ForceUpdateMatrix() : _matrix;
        }

        private bool SetMatrix(Matrix4x4 matrix)
        {
            _matrix = matrix;
            return Matrix4x4.Decompose(matrix, out _scale, out _rotation, out _position);
        }

        public void SetPosition(Vector3 position)
        {
            if (position == _position)
                return;
            _position = position;
            _dirty = true;
        }

        public void SetRotation(Quaternion rotation)
        {
            if (rotation == _rotation)
                return;
            _rotation = rotation;
            _dirty = true;
        }

        public void SetScale(Vector3 scale)
        {
            if (scale == _scale)
                return;
            _scale = scale;
            _dirty = true;
        }
    }
}
