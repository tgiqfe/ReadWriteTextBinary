using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.IO.Compression;

namespace ReadWriteTextBinary
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class ReadRegBinary
    {
        const int BUFF_SIZE = 256;

        const int TextBlock = 80;

        public static string Process(string key, string name, bool compress)
        {
            string retText = "";
            using (RegistryKey regKey = RegistryControl.GetRegistryKey(key, false, false))
            {
                var valueKind = regKey.GetValueKind(name);
                if (valueKind == RegistryValueKind.Binary)
                {
                    if (compress)
                    {
                        using (var msS = new MemoryStream(regKey.GetValue(name) as byte[]))
                        using (var msD = new MemoryStream())
                        {
                            using (var gs = new GZipStream(msD, CompressionMode.Compress))
                            {
                                byte[] buffer = new byte[BUFF_SIZE];
                                int readed = 0;
                                while ((readed = msS.Read(buffer, 0, BUFF_SIZE)) > 0)
                                {
                                    gs.Write(buffer, 0, readed);
                                }
                            }
                            retText = BitConverter.ToString(msD.ToArray()).Replace("-", "");
                        }
                    }
                    else
                    {
                        retText = BitConverter.ToString(regKey.GetValue(name) as byte[]).Replace("-", "").ToUpper();
                    }
                }
            }

            //  テキストブロック化
            if (TextBlock > 0)
            {
                int count = TextBlock;
                using (var sr = new StringReader(retText))
                {
                    int readed = 0;
                    StringBuilder sb = new StringBuilder();
                    char[] buffer = new char[count];
                    while ((readed = sr.Read(buffer, 0, count)) > 0)
                    {
                        sb.AppendLine(new string(buffer, 0, readed));
                    }
                    retText = sb.ToString();
                }
            }

            return retText;
        }
    }
}
