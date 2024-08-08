using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NifScan
{
    public partial class FormES : Form
    {
        static string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        string pathData = Path.Combine(path, "Data");
        string pathOrig = Path.Combine(path, "Original");
        string[] masters = new string[] { "Skyrim.esm", "Update.esm", "Dawnguard.esm", "HearthFires.esm", "Dragonborn.esm" };
        byte[] bytesFile;

        public FormES()
        {
            InitializeComponent();
        }

        void button1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(pathOrig))
            {
                foreach (string line in masters)
                {
                    processRNAM(line);
                }
            }
        }

        void button2_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(pathOrig))
            {
                foreach (string line in masters)
                {
                    if (File.Exists(Path.Combine(pathOrig, line)))
                    {
                        processCTDA(Path.Combine(pathOrig, line), true);
                    }
                }
            }
            if (Directory.Exists(pathData))
            {
                foreach (string line in Directory.EnumerateFiles(pathData))
                {
                    if (line.EndsWith(".esm", StringComparison.OrdinalIgnoreCase) || line.EndsWith(".esp", StringComparison.OrdinalIgnoreCase))
                    {
                        processCTDA(line);
                    }
                }
            }
        }

        void button3_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(pathData))
            {
                foreach (string line in Directory.EnumerateFiles(pathData))
                {
                    if (line.EndsWith(".esm", StringComparison.OrdinalIgnoreCase) || line.EndsWith(".esp", StringComparison.OrdinalIgnoreCase))
                    {
                        processResort(line);
                    }
                }
            }
        }

        void processRNAM(string file)
        {
            bool parse = false;
            if (File.Exists(Path.Combine(pathOrig, file)))
            {
                parse = true;
                file = Path.Combine(pathOrig, file);
            }
            else if (File.Exists(Path.Combine(pathData, file)))
            {
                file = Path.Combine(pathData, file);
            }
            else
            {
                return;
            }
            int gwrldStart = 0;
            bool wrldFound = false;
            bool changed = false;
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
                        bytesName = null;
                        i += 30 + nameLength;
                        if (parse)
                        {
                            List<byte> outBytes = new List<byte>();
                            while (i < fileSize)
                            {
                                if (bytesFile[i] == 82 && bytesFile[i + 1] == 78 && bytesFile[i + 2] == 65 && bytesFile[i + 3] == 77)
                                {
                                    int length = BitConverter.ToUInt16(bytesFile, i + 4);
                                    byte[] bytesBuffer = new byte[6 + length];
                                    Buffer.BlockCopy(bytesFile, i, bytesBuffer, 0, 6 + length);
                                    outBytes.AddRange(bytesBuffer);
                                    bytesBuffer = null;
                                    i += 6 + length;
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
                            byte[] bytesRead = File.ReadAllBytes(Path.Combine(pathOrig, Path.GetFileNameWithoutExtension(file) + " " + worldName));
                            int readSize = bytesRead.Length;
                            byte[] bytesBuffer = new byte[fileSize + readSize];
                            Buffer.BlockCopy(bytesFile, 0, bytesBuffer, 0, i);
                            Buffer.BlockCopy(bytesRead, 0, bytesBuffer, i, readSize);
                            bytesRead = null;
                            Buffer.BlockCopy(bytesFile, i, bytesBuffer, i + readSize, fileSize - i);
                            bytesFile = bytesBuffer;
                            bytesBuffer = null;
                            fileSize += readSize;
                            replaceBytesInFile(gwrldStart, BitConverter.GetBytes(BitConverter.ToInt32(bytesFile, gwrldStart) + readSize));
                            replaceBytesInFile(wrldStart, BitConverter.GetBytes(BitConverter.ToInt32(bytesFile, wrldStart) + readSize));
                            i += readSize - 1;
                            changed = true;
                        }
                    }
                }
            }
            if (changed)
            {
                File.WriteAllBytes(file, bytesFile);
            }
            bytesFile = null;
        }

        void processCTDA(string file, bool parse = false)
        {
            int gpackStart = 0;
            bool packFound = false;
            bool changed = false;
            bytesFile = File.ReadAllBytes(file);
            int fileSize = bytesFile.Length;
            for (int i = 0; i < fileSize; i++)
            {
                if (!packFound)
                {
                    if (bytesFile[i] == 71 && bytesFile[i + 1] == 82 && bytesFile[i + 2] == 85 && bytesFile[i + 3] == 80 && bytesFile[i + 8] == 80 && bytesFile[i + 9] == 65 && bytesFile[i + 10] == 67 && bytesFile[i + 11] == 75 && bytesFile[i + 24] == 80 && bytesFile[i + 25] == 65 && bytesFile[i + 26] == 67 && bytesFile[i + 27] == 75 && bytesFile[i + 48] == 69 && bytesFile[i + 49] == 68 && bytesFile[i + 50] == 73 && bytesFile[i + 51] == 68)
                    {
                        packFound = true;
                        gpackStart = i + 4;
                    }
                }
                else
                {
                    if (bytesFile[i] == 80 && bytesFile[i + 1] == 65 && bytesFile[i + 2] == 67 && bytesFile[i + 3] == 75 && bytesFile[i + 24] == 69 && bytesFile[i + 25] == 68 && bytesFile[i + 26] == 73 && bytesFile[i + 27] == 68)
                    {
                        string ID = bytesFile[i + 15].ToString("X2") + bytesFile[i + 14].ToString("X2") + bytesFile[i + 13].ToString("X2") + bytesFile[i + 12].ToString("X2");
                        int ctda = 1;
                        int length = BitConverter.ToInt32(bytesFile, i + 4);
                        int count = i + length + 24;
                        for (int j = i + 24; j < count; j++)
                        {
                            bool eighteen = bytesFile[j + 14] == 163 && bytesFile[j + 15] == 2 && bytesFile[j + 16] == 0 && bytesFile[j + 17] == 0;
                            if (bytesFile[j] == 67 && bytesFile[j + 1] == 84 && bytesFile[j + 2] == 68 && bytesFile[j + 3] == 65 && (eighteen || (bytesFile[j + 14] == 127 && bytesFile[j + 15] == 2 && bytesFile[j + 16] == 0 && bytesFile[j + 17] == 0) || (bytesFile[j + 14] == 118 && bytesFile[j + 15] == 2 && bytesFile[j + 16] == 0 && bytesFile[j + 17] == 0) || (bytesFile[j + 14] == 117 && bytesFile[j + 15] == 2 && bytesFile[j + 16] == 0 && bytesFile[j + 17] == 0) || (bytesFile[j + 14] == 117 && bytesFile[j + 15] == 2 && bytesFile[j + 16] == 253 && bytesFile[j + 17] == 4) || (bytesFile[j + 14] == 79 && bytesFile[j + 15] == 0 && bytesFile[j + 16] == 0 && bytesFile[j + 17] == 0)))
                            {
                                int ctdalength = BitConverter.ToInt16(bytesFile, j + 4);
                                if (parse)
                                {
                                    byte[] bytesBuffer = new byte[4];
                                    if (eighteen)
                                    {
                                        Buffer.BlockCopy(bytesFile, j + 18, bytesBuffer, 0, 4);
                                    }
                                    else
                                    {
                                        Buffer.BlockCopy(bytesFile, j + 22, bytesBuffer, 0, 4);
                                    }
                                    File.WriteAllBytes(Path.Combine(pathOrig, "CTDA " + ID + " " + length + " " + ctda), bytesBuffer);
                                    bytesBuffer = null;
                                }
                                else if (bytesFile[j + 22] == 0 && bytesFile[j + 23] == 0 && bytesFile[j + 24] == 0 && bytesFile[j + 25] == 0 || (eighteen && (bytesFile[j + 18] == 0 && bytesFile[j + 19] == 0 && bytesFile[j + 20] == 0 && bytesFile[j + 21] == 0)))
                                {
                                    if (bytesFile[j + 14] == 79 && bytesFile[j + 15] == 0 && bytesFile[j + 16] == 0 && bytesFile[j + 17] == 0 && bytesFile[j + 38] == 67 && bytesFile[j + 39] == 73 && bytesFile[j + 40] == 83 && bytesFile[j + 41] == 50)
                                    {
                                        int cis2 = 6 + BitConverter.ToInt16(bytesFile, j + 42);
                                        if (File.Exists(Path.Combine(pathOrig, "CTDA " + ID + " " + (length - cis2) + " " + ctda)))
                                        {
                                            byte[] bytesBuffer = new byte[fileSize - cis2];
                                            Buffer.BlockCopy(bytesFile, 0, bytesBuffer, 0, j + 38);
                                            Buffer.BlockCopy(bytesFile, j + 38 + cis2, bytesBuffer, j + 38, fileSize - cis2 - (j + 38));
                                            bytesFile = bytesBuffer;
                                            bytesBuffer = null;
                                            fileSize -= cis2;
                                            length -= cis2;
                                            count -= cis2;
                                            replaceBytesInFile(i + 4, BitConverter.GetBytes(length));
                                            replaceBytesInFile(gpackStart, BitConverter.GetBytes(BitConverter.ToInt32(bytesFile, gpackStart) - cis2));
                                        }
                                    }
                                    if (File.Exists(Path.Combine(pathOrig, "CTDA " + ID + " " + length + " " + ctda)))
                                    {
                                        byte[] bytesRead = File.ReadAllBytes(Path.Combine(pathOrig, "CTDA " + ID + " " + length + " " + ctda));
                                        if (eighteen)
                                        {
                                            replaceBytesInFile(j + 18, bytesRead);
                                        }
                                        else
                                        {
                                            replaceBytesInFile(j + 22, bytesRead);
                                        }
                                        bytesRead = null;
                                        changed = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("File: " + file + " have broken CTDA in form: " + ID + " and not found original data: " + Path.Combine(pathOrig, "CTDA " + ID + " " + length + " " + ctda));
                                    }
                                }
                                j += ctdalength - 1;
                                ctda++;
                            }
                        }
                        i += length - 1;
                    }
                }
            }
            if (changed)
            {
                File.WriteAllBytes(file, bytesFile);
            }
            bytesFile = null;
        }

        void processResort(string file)
        {
            bytesFile = File.ReadAllBytes(file);
            int fileSize = bytesFile.Length;
            for (int i = 0; i < fileSize; i++)
            {
                if (bytesFile[i] == 71 && bytesFile[i + 1] == 82 && bytesFile[i + 2] == 85 && bytesFile[i + 3] == 80 && !(bytesFile[i + 8] == 87 && bytesFile[i + 9] == 82 && bytesFile[i + 10] == 76 && bytesFile[i + 11] == 68) && !(bytesFile[i + 8] == 68 && bytesFile[i + 9] == 73 && bytesFile[i + 10] == 65 && bytesFile[i + 11] == 76) && bytesFile[i + 8] == bytesFile[i + 24] && bytesFile[i + 9] == bytesFile[i + 25] && bytesFile[i + 10] == bytesFile[i + 26] && bytesFile[i + 11] == bytesFile[i + 27] && bytesFile[i + 48] == 69 && bytesFile[i + 49] == 68 && bytesFile[i + 50] == 73 && bytesFile[i + 51] == 68)
                {
                    int lengthGroup = BitConverter.ToInt32(bytesFile, i + 4);
                    byte[] bytesGroup = new byte[lengthGroup];
                    Buffer.BlockCopy(bytesFile, i, bytesGroup, 0, lengthGroup);
                    List<byte[]> formsList = new List<byte[]>();
                    for (int j = 24; j < lengthGroup; j++)
                    {
                        int length = BitConverter.ToInt32(bytesGroup, j + 4) + 24;
                        byte[] bytesForm = new byte[length];
                        Buffer.BlockCopy(bytesGroup, j, bytesForm, 0, length);
                        formsList.Add(bytesForm);
                        j += length - 1;
                    }
                    byte[] bytesBuffer = new byte[lengthGroup];
                    Buffer.BlockCopy(bytesGroup, 0, bytesBuffer, 0, 24);
                    bytesGroup = null;
                    int offset = 24;
                    int count = formsList.Count - 1;
                    for (int j = count; 0 <= j; j--)
                    {
                        int length = formsList[j].Length;
                        Buffer.BlockCopy(formsList[j], 0, bytesBuffer, offset, length);
                        offset += length;
                    }
                    for (int j = 0; j < lengthGroup; j++)
                    {
                        bytesFile[i + j] = bytesBuffer[j];
                    }
                    i += lengthGroup - 1;
                }
            }
            File.WriteAllBytes(file, bytesFile);
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
