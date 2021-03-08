using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace WorldMapper
{
    public class GameMemoryReader
    {
        public Vector3 Position;

        private const int PROCESS_WM_READ = 0x0010;
        private Process _process;
        private IntPtr _hProcess;

        private byte[] _vector3Buf = new byte[4 * 3];
        private int _posAddress;

        public GameMemoryReader(string processName, int posAddress)
        {
            SetProcess(processName);
            _posAddress = posAddress;
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

        public void ReadPositionInfo()
        {
            ReadVector3FromMemory(_posAddress, ref Position);
        }

        public static void Main_()
        {
            GameMemoryReader reader = new GameMemoryReader(
                "pcsx2", 0x20189EA0
            );
            reader.ReadPositionInfo();
        }
    }
}
