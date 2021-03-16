using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace WorldMapper
{
    public class GameMemoryReader
    {
        public Vector3 CharacterPos;
        public Vector3 CameraPos;
        public Matrix4x4 CameraRotMatrix;

        public Matrix4x4 CameraMatrix =>
            Matrix4x4.CreateTranslation(CameraPos) * CameraRotMatrix;

        private const int PROCESS_WM_READ = 0x0010;
        private Process _process;
        private IntPtr _hProcess;

        private byte[] _vector3Buf = new byte[4 * 3];
        private byte[] _matrix4x4Buf = new byte[4 * 16];

        private int _characterPosAddress;
        private int _cameraPosAddress;
        private int _cameraMatrixAddress;

        public GameMemoryReader(
            string processName, int characterPosAddress, int cameraPosAddress,
            int cameraMatrixAddress
        )
        {
            SetProcess(processName);
            _characterPosAddress = characterPosAddress;
            _cameraPosAddress = cameraPosAddress;
            _cameraMatrixAddress = cameraMatrixAddress;
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        /// <summary>
        /// Sets the process used to access game memory. If multiple processes
        /// are found, the first one will be used.
        /// </summary>
        /// <param name="name">The name of the process to search for</param>
        /// <returns>The number of processes found</returns>
        public int SetProcess(string name)
        {
            Process[] processes = Process.GetProcessesByName("pcsx2");
            if (processes.Length == 0)
            {
                return 0;
            };
            _process = processes[0];
            _hProcess = OpenProcess(PROCESS_WM_READ, false, _process.Id);
            return processes.Length;
        }

        private void ReadVector3FromMemory(int address, ref Vector3 vector)
        {
            int bytesRead = 0;
            ReadProcessMemory((int)_hProcess, address, _vector3Buf, _vector3Buf.Length, ref bytesRead);
            vector.X = BitConverter.ToSingle(_vector3Buf, 0);
            vector.Y = BitConverter.ToSingle(_vector3Buf, 4);
            vector.Z = BitConverter.ToSingle(_vector3Buf, 8);
        }

        private void ReadMatrix3x3FromMemory(int address, ref Matrix4x4 mat)
        {
            int bytesRead = 0;
            ReadProcessMemory((int)_hProcess, address, _matrix4x4Buf, _matrix4x4Buf.Length, ref bytesRead);

            mat.M11 = BitConverter.ToSingle(_matrix4x4Buf, 4 * 0);
            mat.M12 = BitConverter.ToSingle(_matrix4x4Buf, 4 * 1);
            mat.M13 = BitConverter.ToSingle(_matrix4x4Buf, 4 * 2);
            mat.M14 = BitConverter.ToSingle(_matrix4x4Buf, 4 * 3);

            mat.M21 = BitConverter.ToSingle(_matrix4x4Buf, 4 * 4);
            mat.M22 = BitConverter.ToSingle(_matrix4x4Buf, 4 * 5);
            mat.M23 = BitConverter.ToSingle(_matrix4x4Buf, 4 * 6);
            mat.M24 = BitConverter.ToSingle(_matrix4x4Buf, 4 * 7);

            mat.M31 = BitConverter.ToSingle(_matrix4x4Buf, 4 * 8);
            mat.M32 = BitConverter.ToSingle(_matrix4x4Buf, 4 * 9);
            mat.M33 = BitConverter.ToSingle(_matrix4x4Buf, 4 * 10);
            mat.M34 = BitConverter.ToSingle(_matrix4x4Buf, 4 * 11);

            mat.M41 = 0f;
            mat.M42 = 0f;
            mat.M43 = 0f;
            mat.M44 = 1f;
        }

        public void ReadCharacterPos()
        {
            ReadVector3FromMemory(_characterPosAddress, ref CharacterPos);
        }

        public void ReadCameraPosition()
        {
            ReadVector3FromMemory(_cameraPosAddress, ref CameraPos);
        }

        public void ReadCameraRotation()
        {
            ReadMatrix3x3FromMemory(_cameraMatrixAddress, ref CameraRotMatrix);
        }

        public static void Main_()
        {
            GameMemoryReader reader = new GameMemoryReader(
                "pcsx2", 0x20189EA0, 0x201BBD70, 0x201BE100
            );
        }
    }
}
