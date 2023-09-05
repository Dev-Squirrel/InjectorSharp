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

                    // ���õ� ���� ������ ��Ʈ �� Ȯ��
                    bool is64Bit = Is64BitExe(openFileDialog.FileName);
                    if (IntPtr.Size == 8 && !is64Bit)
                    {
                        MessageBox.Show("������ ���α׷��� 32��Ʈ�Դϴ�. 64��Ʈ ���α׷��� �����ϼ���.", "���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ProcessPathTextBox.Text = "";
                    }
                    else if (IntPtr.Size == 4 && is64Bit)
                    {
                        MessageBox.Show("������ ���α׷��� 64��Ʈ�Դϴ�. 32��Ʈ ���α׷��� �����ϼ���.", "���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    // ���õ� ���� ������ ��Ʈ �� Ȯ��
                    bool is64Bit = Is64BitExe(openFileDialog.FileName);
                    if (IntPtr.Size == 8 && !is64Bit)
                    {
                        MessageBox.Show("������ DLL�� 32��Ʈ�Դϴ�. 64��Ʈ DLL�� �����ϼ���.", "���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        DllPathTextBox.Text = "";
                    }
                    else if (IntPtr.Size == 4 && is64Bit)
                    {
                        MessageBox.Show("������ DLL�� 64��Ʈ�Դϴ�. 32��Ʈ DLL�� �����ϼ���.", "���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    Verb = "runas", // ������ �������� �����ϵ��� ����
                    Arguments = parameters // ���� �ɼ� �߰�
                };

                // ���μ��� ����
                Process process = new();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForInputIdle();

                // ���μ��� �ڵ� ��������
                IntPtr processHandle = OpenProcess(0x1F0FFF, false, process.Id);

                // �ε��� DLL�� �ּ� ��������
                IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

                // ���� ���μ����� �޸� �Ҵ�
                IntPtr remoteMem = VirtualAllocEx(processHandle, IntPtr.Zero, (uint)dllPath.Length, 0x1000, 0x40);

                // DLL ��θ� ���� ���μ����� ����
                WriteProcessMemory(processHandle, remoteMem, Encoding.ASCII.GetBytes(dllPath), (uint)dllPath.Length, out int bytesWritten);

                // ���� ������ �����Ͽ� DLL ������ ����
                CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAddr, remoteMem, 0, IntPtr.Zero);

                // �ڵ� �� ���μ��� ���ҽ� ����
                CloseHandle(processHandle);
                process.Close();
            }
            else
            {
                MessageBox.Show("���α׷��� DLL ������ �������ּ���.", "���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show($"{targetProcessName} ���μ����� �������� �ʽ��ϴ�.", "���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("���α׷��� DLL ������ �������ּ���.", "���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                return peFormat == 0x8664; // 0x8664�� x64 ��Ű��ó�� ���Դϴ�.
            }
        }
    }
}