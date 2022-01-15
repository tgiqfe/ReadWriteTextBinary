using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace ReadWriteTextBinary
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class WriteRegBinary
    {
        const int BUFF_SIZE = 4096;

        public static void Process(string text, string key, string name, bool expand)
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

            using (RegistryKey regKey = RegistryControl.GetRegistryKey(key, true, true))
            {
                if (expand)
                {
                    using (var msS = new MemoryStream(bytes))
                    using (var msD = new MemoryStream())
                    {
                        using (var gs = new GZipStream(msS, CompressionMode.Decompress))
                        {
                            byte[] buffer = new byte[BUFF_SIZE];
                            int readed = 0;
                            while ((readed = gs.Read(buffer, 0, BUFF_SIZE)) > 0)
                            {
                                msD.Write(buffer, 0, readed);
                            }
                        }
                        regKey.SetValue(name, msD.ToArray(), RegistryValueKind.Binary);
                    }
                }
                else
                {
                    regKey.SetValue(name, bytes, RegistryValueKind.Binary);
                }
            }
        }
    }
}
