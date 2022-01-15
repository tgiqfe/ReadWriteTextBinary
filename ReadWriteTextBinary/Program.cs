using System;
using System.IO;
using System.Text;

namespace ReadWriteTextBinary
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    class Program
    {
        private static string text = null;

        public static void Main(string[] args)
        {
            string text = TestReadReg();

            TestWriteReg(text);

            Console.ReadLine();
        }

        private static void TestReadFile()
        {
            string sourcePath = Path.Combine(
                Environment.ExpandEnvironmentVariables("%USERPROFILE%"), "Desktop", "Sample.yml");

            text = ReadTextBinary.Process(sourcePath, compress: true);
            Console.WriteLine(text);
        }

        private static void TestWriteFile()
        {
            WriteTextBinary.OutputFile = Path.Combine(
                Environment.ExpandEnvironmentVariables("%USERPROFILE%"), "Desktop", "Example.yml");
            WriteTextBinary.Process(text, expand: true);
        }

        private static string TestReadReg()
        {
            string key = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\Connections";
            string name = "DefaultConnectionSettings";

            string compressText = ReadRegBinary.Process(key, name, compress: false);
            Console.WriteLine(compressText);

            return ReadRegBinary.Process(key, name, compress: true);
        }

        private static void TestWriteReg(string text)
        {
            string key = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\Connections";
            string name = "DefaultConnectionSettings2";

            WriteRegBinary.Process(text, key, name, expand: true);
        }
    }
}
