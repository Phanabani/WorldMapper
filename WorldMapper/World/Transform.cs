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
        private bool _isCamera;

        public Transform(bool isCamera = false)
        {
            _isCamera = isCamera;
        }

        /// <summary>
        /// Modifying a property of one of the vector/quaternion properties
        /// will not mark it as dirty, so the matrix will not be updated.
        /// This method will force it to update.
        /// </summary>
        public void ForceUpdateMatrix()
        {
            Matrix4x4 trans, rot, scale;
            trans = Matrix4x4.CreateTranslation(-_position);
            rot = Matrix4x4.CreateFromQuaternion(Quaternion.Inverse(_rotation));
            scale = Matrix4x4.CreateScale(_scale);
            if (_isCamera)
                Matrix4x4.Invert(trans * rot, out _matrix);
            else
                _matrix = trans * rot * scale;
            _dirty = false;
        }

        private Matrix4x4 GetMatrix()
        {
            if (_dirty)
                ForceUpdateMatrix();
            return _matrix;
        }

        private bool SetMatrix(Matrix4x4 matrix)
        {
            var success = Matrix4x4.Decompose(
                matrix, out _scale, out _rotation, out _position
            );
            ForceUpdateMatrix();
            return success;
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
