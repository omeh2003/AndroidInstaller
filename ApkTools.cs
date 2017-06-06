using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidInstaller
{
    class ApkTools
    {
        public  string ApkTool;
        public bool ApkToolIsFind ;
        public string ApkToolTMP;
        public bool ApkToolTMPisFind;
        public static string filename;
        public static string filedir;

        public static string fullfilename;
     

        public ApkTools()
        {
            ApkTool = Directory.GetCurrentDirectory() + @"\" + @"Utils\apktool.jar";
            ApkToolIsFind = File.Exists(ApkTool);
            ApkToolTMP = Directory.GetCurrentDirectory() + @"\" + "TMP";
            ApkToolTMPisFind = Directory.Exists(ApkToolTMP);
        }

       public void Extracktapk()
       {
           var file = ApkToolTMP + @"\"+filename;
           var apk = File.Exists(ApkToolTMP + @"\" + filename);
           var par = "/C java -jar "+ "\""+ApkTool + "\""+" d \""+file+"\""+" -o \""+ApkToolTMP+@"\"+filedir+"\"";
            if(apk)
            {
                var process = new Process {StartInfo = new ProcessStartInfo("cmd", par) {UseShellExecute = false, CreateNoWindow = true } };
            
                process.Start();

                process.WaitForExit();
              
                
                process.Close();

                // var process = Process.Start(@"C:\ProgramData\Oracle\Java\javapath\java.exe", par);
                //  process?.WaitForExit();

            }
       }

    }
}
