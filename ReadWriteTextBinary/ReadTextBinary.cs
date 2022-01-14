using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace ReadWriteTextBinary
{
    internal class ReadTextBinary
    {
        const int BUFF_SIZE = 256;

        const int TextBlock = 80;

        public static string Process(string filePath, bool compress)
        {
            string retText = "";

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            using (var ms = new MemoryStream())
            {
                if (fs.Length < int.MaxValue)
                {
                    byte[] buffer = new byte[BUFF_SIZE];
                    int readed = 0;

                    if (compress)
                    {
                        //  圧縮する場合
                        using (var gs = new GZipStream(ms, CompressionMode.Compress))
                        {        
                            while ((readed = br.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                gs.Write(buffer, 0, readed);
                            }
                        }
                    }
                    else
                    {
                        //  非圧縮
                        while ((readed = br.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, readed);
                        }
                    }

                    retText =  BitConverter.ToString(ms.ToArray()).Replace("-", "");
                }
                else
                {
                    Console.Error.WriteLine("Size Over.");
                    return null;
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
