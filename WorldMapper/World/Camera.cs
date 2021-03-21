using System;
using System.Numerics;

namespace WorldMapper.World
{
    public class Camera : ITransformable
    {
        /// <summary>
        /// Vertical field of view angle in degrees.
        /// </summary>
        /// <exception cref="ArgumentException">the angle is out of range (0, 180)</exception>
        public float FieldOfView
        {
            get => _fieldOfView;
            set
            {
                if (value <= 0f || value >= 180f)
                    throw new ArgumentException(
                        "Field of view must be between 0 and 180 degrees, exclusive"
                    );
                _fieldOfView = value;
            }
        }

        public float Width { get; set; }
        public float Height { get; set; }
        public float ClipNear { get; set; } = 0.1f;
        public float ClipFar { get; set; } = 100f;
        public Matrix4x4 ProjectionMatrix => _projectionMatrix;
        public Matrix4x4 ViewMatrix => Transform.Matrix;
        public Transform Transform { get; set; } = new Transform(true);

        private float _fieldOfView;
        private Matrix4x4 _projectionMatrix;

        public Camera(float width, float height, float fieldOfView = 75,
            float clipNear = 0.1f, float clipFar = 100f)
        {
            Width = width;
            Height = height;
            FieldOfView = fieldOfView;
            ClipNear = clipNear;
            ClipFar = clipFar;
            UpdateProjectionMatrix();
        }

        public void UpdateProjectionMatrix()
        {
            _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(
                _fieldOfView / 180f * (float) Math.PI, Width/Height,
                ClipNear, ClipFar
            );
        }
    }
}
