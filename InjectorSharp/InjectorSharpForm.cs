using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace InjectorSharp
{
    public partial class InjectorSharpForm : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        public InjectorSharpForm()
        {
            InitializeComponent();
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Executable Files|*.exe";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ProcessPathTextBox.Text = openFileDialog.FileName;

                    // 선택된 실행 파일의 비트 수 확인
                    bool is64Bit = Is64BitExe(openFileDialog.FileName);
                    if (IntPtr.Size == 8 && !is64Bit)
                    {
                        MessageBox.Show("선택한 프로그램은 32비트입니다. 64비트 프로그램을 선택하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ProcessPathTextBox.Text = "";
                    }
                    else if (IntPtr.Size == 4 && is64Bit)
                    {
                        MessageBox.Show("선택한 프로그램은 64비트입니다. 32비트 프로그램을 선택하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ProcessPathTextBox.Text = "";
                    }
                }
            }
        }

        private void DllButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "DLL Files|*.dll";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DllPathTextBox.Text = openFileDialog.FileName;

                    // 선택된 실행 파일의 비트 수 확인
                    bool is64Bit = Is64BitExe(openFileDialog.FileName);
                    if (IntPtr.Size == 8 && !is64Bit)
                    {
                        MessageBox.Show("선택한 DLL은 32비트입니다. 64비트 DLL을 선택하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        DllPathTextBox.Text = "";
                    }
                    else if (IntPtr.Size == 4 && is64Bit)
                    {
                        MessageBox.Show("선택한 DLL은 64비트입니다. 32비트 DLL을 선택하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        DllPathTextBox.Text = "";
                    }
                }
            }
        }

        private void RunAndInjectButton_Click(object sender, EventArgs e)
        {
            string processPath = ProcessPathTextBox.Text;
            string dllPath = DllPathTextBox.Text;

            if (!string.IsNullOrEmpty(processPath) && !string.IsNullOrEmpty(dllPath))
            {
                string parameters = ParametersTextBox.Text;
                ProcessStartInfo startInfo = new()
                {
                    FileName = processPath,
                    Verb = "runas", // 관리자 권한으로 실행하도록 설정
                    Arguments = parameters // 실행 옵션 추가
                };

                // 프로세스 실행
                Process process = new();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForInputIdle();

                // 프로세스 핸들 가져오기
                IntPtr processHandle = OpenProcess(0x1F0FFF, false, process.Id);

                // 로드할 DLL의 주소 가져오기
                IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

                // 원격 프로세스에 메모리 할당
                IntPtr remoteMem = VirtualAllocEx(processHandle, IntPtr.Zero, (uint)dllPath.Length, 0x1000, 0x40);

                // DLL 경로를 원격 프로세스에 쓰기
                WriteProcessMemory(processHandle, remoteMem, Encoding.ASCII.GetBytes(dllPath), (uint)dllPath.Length, out int bytesWritten);

                // 원격 스레드 생성하여 DLL 인젝션 수행
                CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAddr, remoteMem, 0, IntPtr.Zero);

                // 핸들 및 프로세스 리소스 정리
                CloseHandle(processHandle);
                process.Close();
            }
            else
            {
                MessageBox.Show("프로그램과 DLL 파일을 선택해주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ProcessInjectButton_Click(object sender, EventArgs e)
        {
            string processPath = ProcessPathTextBox.Text;
            string dllPath = DllPathTextBox.Text;
            if (!string.IsNullOrEmpty(processPath) && !string.IsNullOrEmpty(dllPath))
            {
                string targetProcessName = Path.GetFileNameWithoutExtension(processPath);
                Process targetProcess = Process.GetProcessesByName(targetProcessName)[0];

                if (targetProcess != null)
                {
                    IntPtr targetProcessHandle = OpenProcess(0x1F0FFF, false, targetProcess.Id);

                    IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                    IntPtr remoteMem = VirtualAllocEx(targetProcessHandle, IntPtr.Zero, (uint)dllPath.Length, 0x1000, 0x40);
                    WriteProcessMemory(targetProcessHandle, remoteMem, Encoding.ASCII.GetBytes(dllPath), (uint)dllPath.Length, out int bytesWritten);
                    CreateRemoteThread(targetProcessHandle, IntPtr.Zero, 0, loadLibraryAddr, remoteMem, 0, IntPtr.Zero);

                    CloseHandle(targetProcessHandle);
                }
                else
                {
                    MessageBox.Show($"{targetProcessName} 프로세스가 존재하지 않습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("프로그램과 DLL 파일을 선택해주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool Is64BitExe(string exeFilePath)
        {
            using (BinaryReader reader = new(File.OpenRead(exeFilePath)))
            {
                reader.BaseStream.Seek(0x3C, SeekOrigin.Begin);
                ushort peHeaderOffset = reader.ReadUInt16();

                reader.BaseStream.Seek(peHeaderOffset + 0x4, SeekOrigin.Begin);
                ushort peFormat = reader.ReadUInt16();

                return peFormat == 0x8664; // 0x8664는 x64 아키텍처의 값입니다.
            }
        }
    }
}