using ICSharpCode.SharpZipLib.Zip;
using Serilog;
using System.IO;

namespace Sherlog.Service.Actions
{
    public class Compressor
    {
        public static void Compress(string[] inputfiles, string output)
        {
            //CreateZip(inputfiles, output);
            CreateSimpleZip(inputfiles, output);

            Log.Debug($"Compressing {inputfiles[0]} to {output}");
        }

        private static void CreateZip(string[] zipFileList, string output)
        {
            using (var writeStream = File.OpenWrite(output))
            {

                byte[] buffer = new byte[4096];

                ZipOutputStream zipOutputStream = new ZipOutputStream(writeStream);
                zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression
                

                foreach (string fileName in zipFileList)
                {
                    Stream fs = File.OpenRead(fileName);
                    
                    ZipEntry entry = new ZipEntry(Path.GetFileName(fileName));
                    entry.Size = fs.Length;

                    zipOutputStream.PutNextEntry(entry);

                    int count = fs.Read(buffer, 0, buffer.Length);
                    while (count > 0)
                    {
                        zipOutputStream.Write(buffer, 0, count);
                        count = fs.Read(buffer, 0, buffer.Length);
                        if (!writeStream.CanWrite)
                        {
                            break;
                        }
                        writeStream.Flush();
                    }
                    fs.Close();
                }
                zipOutputStream.Close();
                writeStream.Close();
            }
        }

        private static void CreateSimpleZip(string[] zipFileList, string output)
        {
            using (ZipFile zipFile = ZipFile.Create(output))
            {
                zipFile.BeginUpdate();
                foreach (string file in zipFileList)
                {
                    zipFile.Add(file, Path.GetFileName(file));
                }
                zipFile.CommitUpdate();
            }
        }
    }
}
