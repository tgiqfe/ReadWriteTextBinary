using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;

namespace ReadWriteTextBinary
{
    internal class WriteTextBinary
    {
        const int BUFF_SIZE = 4096;

        //public static string ImportFile = "";
        public static string OutputFile = "";

        public static void Process(string text, bool expand)
        {
            if (text.Contains("\n"))
            {
                text = Regex.Replace(text, "\r?\n", "");
            }

            byte[] bytes = new byte[0] { };
            if (Regex.IsMatch(text, @"^[0-9a-fA-F]+$"))
            {
                var tempBytes = new List<byte>();
                for (int i = 0; i < text.Length / 2; i++)
                {
                    tempBytes.Add(Convert.ToByte(text.Substring(i * 2, 2), 16));
                }
                bytes = tempBytes.ToArray();
            }

            if (expand)
            {
                using (var fs = new FileStream(OutputFile, FileMode.Create, FileAccess.Write))
                using (var bw = new BinaryWriter(fs))
                {
                    if (expand)
                    {
                        using (var ms = new MemoryStream(bytes))
                        using (var gs = new GZipStream(ms, CompressionMode.Decompress))
                        {
                            byte[] buffer = new byte[BUFF_SIZE];
                            int readed = 0;
                            while ((readed = gs.Read(buffer, 0, BUFF_SIZE)) > 0)
                            {
                                bw.Write(buffer, 0, readed);
                            }
                        }
                    }
                    else
                    {
                        bw.Write(bytes);
                    }
                }
            }
        }
    }
}
