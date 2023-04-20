using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows;

namespace Lab2
{
    public partial class MainWindow : Window
    {
        private string fileName;
        private bool hasFile = false;

        private string zipProcessFileName = "ZipProcess.exe";
        private string hashCodeProcessFileName = "HashCodeProcess.exe";
        private string pngProcessFileName = "PngProcess.exe";

        public MainWindow()
        {
            InitializeComponent();

            if (IsAdmin())
            {
                MessageBox.Show("Приложение запущено с правами администратора.\nЗапустите приложение без прав администратора");
                Application.Current.Shutdown();
            }
        }

        private bool IsAdmin()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void BtnOpenFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (openFileDialog.ShowDialog() == true)
            {
                txtBoxFileName.Text = openFileDialog.FileName;
            }
        }

        private int StartProcess(string processName, string arguments, bool runAsAdmin)
        {
            Process process = new Process();
            process.StartInfo.FileName = processName;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Arguments = string.Format("\"{0}\"", arguments);
            if (runAsAdmin)
            {
                process.StartInfo.Verb = "runas";
            }
            else
            {
                process.StartInfo.Verb = "open";
            }

            process.Start();

            process.WaitForExit();
            return process.ExitCode;
        }

        private bool PrepareAndStartProcess(string processFileName)
        {
            GetFileName();
            int exitCode = -1;
            if (hasFile)
            {
                exitCode = StartProcess(processFileName, fileName, false);
                if (exitCode == -5)
                {
                    exitCode = StartProcess(processFileName, fileName, true);
                }
            }
            else
            {
                MessageBox.Show("Выберите файл");
            }
            return exitCode == 0;
        }

        private void GetFileName()
        {
            fileName = txtBoxFileName.Text;
            if (File.Exists(fileName))
            {
                hasFile = true;
            }
            else
            {
                hasFile = false;
            }
        }

        private void BtnConvertToZipClick(object sender, RoutedEventArgs e)
        {
            bool processEnded = PrepareAndStartProcess(zipProcessFileName);
            if (processEnded)
            {
                MessageBox.Show("Zip-архив успешно создан");
            }
        }

        private void BtnGetHashCodeClick(object sender, RoutedEventArgs e)
        {
            bool processEnded = PrepareAndStartProcess(hashCodeProcessFileName);
            if (processEnded)
            {
                MessageBox.Show("Хеш файла успешно подсчитан");
            }
        }

        private void BtnConvertToPngClick(object sender, RoutedEventArgs e)
        {
            bool processEnded = PrepareAndStartProcess(pngProcessFileName);
            if (processEnded)
            {
                MessageBox.Show("Изображение успешно преобразовано в png");
            }
        }
    }
}
