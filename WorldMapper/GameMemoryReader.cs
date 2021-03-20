using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace WorldMapper
{
    public class GameMemoryReader
    {
        public Vector3 CharacterPos => _characterPos;
        public Vector3 CameraPos => _cameraPos;
        public Quaternion CameraRot
        {
            get
            {
                var q = Quaternion.CreateFromRotationMatrix(_cameraRotMatrix);
                return new Quaternion(q.X, -q.Y, -q.Z, q.W);
            }
        }

        public Matrix4x4 CameraRotMatrix => _cameraRotMatrix;

        public char UpAxis
        {
            get => _upAxis;
            set => SetUpAxis(value);
        }

        public string ProcessName
        {
            get => _processName;
            set
            {
                _processName = value;
                SetProcess(value);
            }
        }

        public int CharacterPosAddress { get; set; }
        public int CameraPosAddress { get; set; }
        public int CameraRotMatrixAddress { get; set; }

        private Vector3 _characterPos;
        private Vector3 _cameraPos;
        private Matrix4x4 _cameraRotMatrix;
        private char _upAxis;
        private string _processName;
        private Matrix4x4 _axisOrder = Matrix4x4.Identity;

        private const int PROCESS_WM_READ = 0x0010;
        private Process _process;
        private IntPtr _hProcess;

        private readonly byte[] _buffer = new byte[4 * 16];

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        private void SetUpAxis(char axis)
        {
            switch (axis)
            {
                case 'y':
                    _axisOrder = Matrix4x4.Identity;
                    break;
                case 'z':
                    _axisOrder = new Matrix4x4(
                        1,  0,  0, 0,
                        0,  0, -1, 0,
                        0,  1,  0, 0,
                        0,  0,  0, 1
                    );
                    break;
                default:
                    throw new ArgumentException("Up axis must be 'y' or 'z'");
            }

            _upAxis = axis;
        }

        /// <summary>
        /// Sets the process used to access game memory. If multiple processes
        /// are found, the first one will be used.
        /// </summary>
        /// <param name="name">The name of the process to search for</param>
        /// <returns>The number of processes found</returns>
        private int SetProcess(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            {
                return 0;
            };
            _process = processes[0];
            _hProcess = OpenProcess(PROCESS_WM_READ, false, _process.Id);
            return processes.Length;
        }

        private void MemReadVector3(int address, ref Vector3 vector)
        {
            var bytesRead = 0;
            ReadProcessMemory((int)_hProcess, address, _buffer, 4 * 3, ref bytesRead);
            vector.X = BitConverter.ToSingle(_buffer, 4 * 0);
            vector.Y = BitConverter.ToSingle(_buffer, 4 * 1);
            vector.Z = BitConverter.ToSingle(_buffer, 4 * 2);
        }

        private void MemReadMatrix3x3(int address, ref Matrix4x4 mat)
        {
            var bytesRead = 0;
            ReadProcessMemory((int)_hProcess, address, _buffer, 4 * 9, ref bytesRead);

            mat.M11 = BitConverter.ToSingle(_buffer, 4 * 0);
            mat.M12 = BitConverter.ToSingle(_buffer, 4 * 1);
            mat.M13 = BitConverter.ToSingle(_buffer, 4 * 2);
            mat.M14 = 0;

            mat.M21 = BitConverter.ToSingle(_buffer, 4 * 3);
            mat.M22 = BitConverter.ToSingle(_buffer, 4 * 4);
            mat.M23 = BitConverter.ToSingle(_buffer, 4 * 5);
            mat.M24 = 0;

            mat.M31 = BitConverter.ToSingle(_buffer, 4 * 6);
            mat.M32 = BitConverter.ToSingle(_buffer, 4 * 7);
            mat.M33 = BitConverter.ToSingle(_buffer, 4 * 8);
            mat.M34 = 0;

            mat.M41 = 0f;
            mat.M42 = 0f;
            mat.M43 = 0f;
            mat.M44 = 1f;
        }

        private void MemReadMatrix3x4(int address, ref Matrix4x4 mat)
        {
            var bytesRead = 0;
            ReadProcessMemory((int)_hProcess, address, _buffer, 4 * 12, ref bytesRead);

            mat.M11 = BitConverter.ToSingle(_buffer, 4 * 0);
            mat.M12 = BitConverter.ToSingle(_buffer, 4 * 1);
            mat.M13 = BitConverter.ToSingle(_buffer, 4 * 2);
            mat.M14 = BitConverter.ToSingle(_buffer, 4 * 3);

            mat.M21 = BitConverter.ToSingle(_buffer, 4 * 4);
            mat.M22 = BitConverter.ToSingle(_buffer, 4 * 5);
            mat.M23 = BitConverter.ToSingle(_buffer, 4 * 6);
            mat.M24 = BitConverter.ToSingle(_buffer, 4 * 7);

            mat.M31 = BitConverter.ToSingle(_buffer, 4 * 8);
            mat.M32 = BitConverter.ToSingle(_buffer, 4 * 9);
            mat.M33 = BitConverter.ToSingle(_buffer, 4 * 10);
            mat.M34 = BitConverter.ToSingle(_buffer, 4 * 11);

            mat.M41 = 0f;
            mat.M42 = 0f;
            mat.M43 = 0f;
            mat.M44 = 1f;
        }

        private void MemReadMatrix4x4(int address, ref Matrix4x4 mat)
        {
            var bytesRead = 0;
            ReadProcessMemory((int)_hProcess, address, _buffer, 4 * 16, ref bytesRead);

            mat.M11 = BitConverter.ToSingle(_buffer, 4 * 0);
            mat.M12 = BitConverter.ToSingle(_buffer, 4 * 1);
            mat.M13 = BitConverter.ToSingle(_buffer, 4 * 2);
            mat.M14 = BitConverter.ToSingle(_buffer, 4 * 3);

            mat.M21 = BitConverter.ToSingle(_buffer, 4 * 4);
            mat.M22 = BitConverter.ToSingle(_buffer, 4 * 5);
            mat.M23 = BitConverter.ToSingle(_buffer, 4 * 6);
            mat.M24 = BitConverter.ToSingle(_buffer, 4 * 7);

            mat.M31 = BitConverter.ToSingle(_buffer, 4 * 8);
            mat.M32 = BitConverter.ToSingle(_buffer, 4 * 9);
            mat.M33 = BitConverter.ToSingle(_buffer, 4 * 10);
            mat.M34 = BitConverter.ToSingle(_buffer, 4 * 11);

            mat.M41 = BitConverter.ToSingle(_buffer, 4 * 12);
            mat.M42 = BitConverter.ToSingle(_buffer, 4 * 13);
            mat.M43 = BitConverter.ToSingle(_buffer, 4 * 14);
            mat.M44 = BitConverter.ToSingle(_buffer, 4 * 15);
        }

        public void ReadCharacterPos()
        {
            MemReadVector3(CharacterPosAddress, ref _characterPos);
            _characterPos = Vector3.Transform(_characterPos, _axisOrder);
        }

        public void ReadCameraPosition()
        {
            MemReadVector3(CameraPosAddress, ref _cameraPos);
            _cameraPos = Vector3.Transform(_cameraPos, _axisOrder);
        }

        public void ReadCameraRotation()
        {
            MemReadMatrix3x4(CameraRotMatrixAddress, ref _cameraRotMatrix);
            _cameraRotMatrix = _axisOrder * _cameraRotMatrix;
        }
    }
}
