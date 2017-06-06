using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using SharpAdbClient;
using SharpAdbClient.DeviceCommands;

namespace AndroidInstaller
{
    public partial class AndroidInstaller : Form
    {
        private readonly ApkTools _apk;
        private string _adbPath = @"C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe";

        public AndroidInstaller()
        {
            InitializeComponent();
            _apk = new ApkTools();
            var result = AdbServer.Instance.StartServer(_adbPath, true);
            richTextBox1.AppendText("Статус ADB сервера -  " + result + "\n");
            richTextBox1.AppendText("Apk утилиты найдены - " + _apk.ApkToolIsFind + "\n");
            richTextBox1.AppendText("Временный каталог найден - " + _apk.ApkToolTMPisFind + "\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var devices = AdbClient.Instance.GetDevices();
            foreach (var device in devices)
            {
                richTextBox1.AppendText("Устройство : " + device.Model + " " + device.Name + " " + device.Product +
                                        " " + device.State + "\n" + "\n");

                var dic = device.GetProperties().ToList();
                richTextBox1.AppendText("Свойства:" + "\n");
                foreach (var kePair in dic)
                    richTextBox1.AppendText(kePair.Key + " " + kePair.Value + "\n");
                richTextBox1.AppendText("\n");
                richTextBox1.AppendText("\n");
                richTextBox1.AppendText("\n");
                var dic1 = device.GetEnvironmentVariables().ToList();
                richTextBox1.AppendText("Окружение:" + "\n");
                foreach (var kePair in dic1)
                    richTextBox1.AppendText(kePair.Key + " " + kePair.Value + "\n");

                var dic2 = device.ListProcesses().ToList();

                richTextBox1.AppendText("\n");
                richTextBox1.AppendText("\n");
                richTextBox1.AppendText("\n");
                richTextBox1.AppendText("Процессы:" + "\n");
                foreach (var kePair in dic2)
                    richTextBox1.AppendText(kePair.Name + " " + kePair.State + " " + kePair.VirtualSize / 1024 +
                                            " Kb. " + kePair.ProcessId + "\n");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _apk.Extracktapk();
            richTextBox1.AppendText("Все ");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.FileName = " ";
            openFileDialog1.ShowDialog();
            ApkTools.filename = openFileDialog1.SafeFileName;
            if (ApkTools.filename == null) return;
            ApkTools.filedir = ApkTools.filename.Remove(ApkTools.filename.IndexOf(".", StringComparison.Ordinal));
            if (openFileDialog1.SafeFileName != null)
               // ApkTools.fullfilename = Path.GetFullPath(openFileDialog1.FileName);
            ApkTools.fullfilename = new DirectoryInfo(openFileDialog1.FileName).ToString();
            richTextBox1.AppendText("Имя файла: " + ApkTools.filename + "\n");
            richTextBox1.AppendText("Имя файла: " + ApkTools.fullfilename + "\n");
        }

        private void button4_Click(object sender, EventArgs e)
        {
          //  var devices = AdbClient.Instance.GetDevices().First();
         //   var manager = new PackageManager(devices);
            var file = ApkTools.fullfilename;
            InstallABD_File_For_Android(file);
          //  manager.InstallPackage(file, true);
        }

        private void InstallABD_File_For_Android(string file)
        {
            var process = new Process { StartInfo = new ProcessStartInfo(Path.GetFullPath(_adbPath), " install "+'"'+file+'"') { UseShellExecute = true, CreateNoWindow = true } };

            process.Start();
       
            process.WaitForExit();

            
            process.Close();
        }

        private void UploadFile()
        {
            var device = AdbClient.Instance.GetDevices().First();


            using (var service = new SyncService(device))
            using (Stream stream = File.OpenRead(ApkTools.fullfilename))
            {
                service.Push(stream, "/storage/extSdCard/MyFile.apk", 644, DateTime.Now, null, CancellationToken.None);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var devices = AdbClient.Instance.GetDevices().First();
             var manager = new PackageManager(devices);
            var list= new ArrayList();
            foreach (var package in manager.Packages)
            {
                list.Add(package.Key);               
                //comboBox1.Items.Add(package.Key);
            }
            list.Sort();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(list.ToArray());
            //   UploadFile();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var pack = comboBox1.SelectedItem.ToString();
            richTextBox1.AppendText("Удаляю пакет: " + pack + "\n");
            var process = new Process { StartInfo = new ProcessStartInfo(Path.GetFullPath(_adbPath), " uninstall " + pack) { UseShellExecute = true, CreateNoWindow = true } };

            process.Start();

            process.WaitForExit();


            process.Close();
        }
    }
}