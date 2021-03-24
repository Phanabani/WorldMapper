using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WorldMapper.Pages
{
    public class ControlsData : INotifyPropertyChanged
    {
        public int PlayerPositionAddress { get; set; }
        public int CameraPositionAddress { get; set; }
        public int CameraRotationAddress { get; set; }

        public float FieldOfView
        {
            get => _fieldOfView;
            set
            {
                if (_fieldOfView == value) return;
                _fieldOfView = value;
                OnPropertyChanged();
                OnFieldOfViewChanged(value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<FieldOfViewEventArgs> FieldOfViewChanged;

        private float _fieldOfView = 60;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, e);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected void OnFieldOfViewChanged(FieldOfViewEventArgs e)
        {
            var handler = FieldOfViewChanged;
            handler?.Invoke(this, e);
        }

        protected void OnFieldOfViewChanged(float fieldOfView)
        {
            OnFieldOfViewChanged(new FieldOfViewEventArgs(fieldOfView));
        }
    }

    public class FieldOfViewEventArgs : EventArgs
    {
        /// <summary>
        /// The vertical field of view in degrees
        /// </summary>
        public float FieldOfView { get; set; }

        public FieldOfViewEventArgs(float fieldOfView)
        {
            FieldOfView = fieldOfView;
        }
    }
}
