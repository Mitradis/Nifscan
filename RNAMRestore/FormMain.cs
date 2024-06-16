using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RNAMRestore
{
    public partial class FormMain : Form
    {

        static string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        string pathOrig = Path.Combine(path, "Original");
        string[] masters = new string[] { "Skyrim.esm", "Update.esm", "Dawnguard.esm", "HearthFires.esm", "Dragonborn.esm" };
        byte[] bytesFile;

        public FormMain()
        {
            InitializeComponent();
        }

        void processFile(string file)
        {
            bool parse = false;
            if (File.Exists(Path.Combine(pathOrig, file)))
            {
                parse = true;
                file = Path.Combine(pathOrig, file);
            }
            else if (File.Exists(Path.Combine(path, file)))
            {
                file = Path.Combine(path, file);
            }
            else
            {
                return;
            }
            int gwrldStart = 0;
            bool wrldFound = false;
            bytesFile = File.ReadAllBytes(file);
            int fileSize = bytesFile.Length;
            for (int i = 0; i < fileSize; i++)
            {
                if (!wrldFound)
                {
                    if (bytesFile[i] == 71 && bytesFile[i + 1] == 82 && bytesFile[i + 2] == 85 && bytesFile[i + 3] == 80 && bytesFile[i + 8] == 87 && bytesFile[i + 9] == 82 && bytesFile[i + 10] == 76 && bytesFile[i + 11] == 68 && bytesFile[i + 24] == 87 && bytesFile[i + 25] == 82 && bytesFile[i + 26] == 76 && bytesFile[i + 27] == 68 && bytesFile[i + 48] == 69 && bytesFile[i + 49] == 68 && bytesFile[i + 50] == 73 && bytesFile[i + 51] == 68)
                    {
                        wrldFound = true;
                        gwrldStart = i + 4;
                    }
                }
                else
                {
                    if (bytesFile[i] == 87 && bytesFile[i + 1] == 82 && bytesFile[i + 2] == 76 && bytesFile[i + 3] == 68 && bytesFile[i + 24] == 69 && bytesFile[i + 25] == 68 && bytesFile[i + 26] == 73 && bytesFile[i + 27] == 68)
                    {
                        int wrldStart = i + 4;
                        int nameLength = BitConverter.ToUInt16(bytesFile, i + 28);
                        byte[] bytesName = new byte[nameLength - 1];
                        Buffer.BlockCopy(bytesFile, i + 30, bytesName, 0, nameLength - 1);
                        string worldName = Encoding.UTF8.GetString(bytesName);
                        i = i + 30 + nameLength;
                        if (parse)
                        {
                            List<byte> outBytes = new List<byte>();
                            while (i < fileSize)
                            {
                                if (bytesFile[i] == 82 && bytesFile[i + 1] == 78 && bytesFile[i + 2] == 65 && bytesFile[i + 3] == 77)
                                {
                                    int rnamLength = BitConverter.ToUInt16(bytesFile, i + 4);
                                    byte[] bytesBuffer = new byte[6 + rnamLength];
                                    Buffer.BlockCopy(bytesFile, i, bytesBuffer, 0, 6 + rnamLength);
                                    outBytes.AddRange(bytesBuffer);
                                    i = i + 6 + rnamLength;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (outBytes.Count > 0)
                            {
                                File.WriteAllBytes(Path.Combine(pathOrig, Path.GetFileNameWithoutExtension(file) + " " + worldName), outBytes.ToArray());
                            }
                            outBytes.Clear();
                        }
                        else if (bytesFile[i] != 82 && bytesFile[i + 1] != 78 && bytesFile[i + 2] != 65 && bytesFile[i + 3] != 77 && File.Exists(Path.Combine(pathOrig, Path.GetFileNameWithoutExtension(file) + " " + worldName)))
                        {
                            byte[] fileBytes = File.ReadAllBytes(Path.Combine(pathOrig, Path.GetFileNameWithoutExtension(file) + " " + worldName));
                            int fileBytesSize = fileBytes.Length;
                            byte[] bytesBuffer = new byte[fileSize + fileBytesSize];
                            Buffer.BlockCopy(bytesFile, 0, bytesBuffer, 0, i);
                            Buffer.BlockCopy(fileBytes, 0, bytesBuffer, i, fileBytesSize);
                            fileBytes = null;
                            Buffer.BlockCopy(bytesFile, i, bytesBuffer, i + fileBytesSize, fileSize - i);
                            bytesFile = bytesBuffer;
                            fileSize = fileSize + fileBytesSize;
                            bytesBuffer = null;
                            int length = BitConverter.ToInt32(bytesFile, gwrldStart) + fileBytesSize;
                            replaceBytesInFile(gwrldStart, BitConverter.GetBytes(length));
                            length = BitConverter.ToInt32(bytesFile, wrldStart) + fileBytesSize;
                            replaceBytesInFile(wrldStart, BitConverter.GetBytes(length));
                            i = i + fileBytesSize;
                        }
                    }
                }
            }
            File.WriteAllBytes(file, bytesFile);
        }

        void button1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(pathOrig))
            {
                foreach (string line in masters)
                {
                    processFile(line);
                }
            }
        }

        void replaceBytesInFile(int start, byte[] array)
        {
            bytesFile[start] = array[0];
            bytesFile[start + 1] = array[1];
            bytesFile[start + 2] = array[2];
            bytesFile[start + 3] = array[3];
        }
    }
}
