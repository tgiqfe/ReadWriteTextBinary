using System;
using System.IO;
using System.Text;

namespace ReadWriteTextBinary
{
    class Program
    {
        public static void Main(string[] args)
        {
            string sourcePath = Path.Combine(
                Environment.ExpandEnvironmentVariables("%USERPROFILE%"), "Desktop", "Sample.yml");

            string text = ReadTextBinary.Process(sourcePath, compress: true);
            Console.WriteLine(text);

            WriteTextBinary.OutputFile = Path.Combine(
                Environment.ExpandEnvironmentVariables("%USERPROFILE%"), "Desktop", "Example.yml");
            WriteTextBinary.Process(text, expand: true);

            Console.ReadLine();
        }
    }
}
