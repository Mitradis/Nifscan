using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Nifscan
{
    public partial class FormVersion : Form
    {
        static List<List<byte>> groupNames = new List<List<byte>>() { new List<byte> { 82, 69, 70, 82 }, new List<byte> { 73, 78, 70, 79 }, new List<byte> { 78, 65, 86, 77 }, new List<byte> { 76, 65, 78, 68 }, new List<byte> { 65, 67, 72, 82 }, new List<byte> { 65, 65, 67, 84 }, new List<byte> { 65, 67, 84, 73 }, new List<byte> { 65, 68, 68, 78 }, new List<byte> { 65, 76, 67, 72 }, new List<byte> { 65, 77, 77, 79 }, new List<byte> { 65, 78, 73, 79 }, new List<byte> { 65, 80, 80, 65 }, new List<byte> { 65, 82, 77, 65 }, new List<byte> { 65, 82, 77, 79 }, new List<byte> { 65, 82, 84, 79 }, new List<byte> { 65, 83, 80, 67 }, new List<byte> { 65, 83, 84, 80 }, new List<byte> { 65, 86, 73, 70 }, new List<byte> { 66, 79, 79, 75 }, new List<byte> { 66, 80, 84, 68 }, new List<byte> { 67, 65, 77, 83 }, new List<byte> { 67, 69, 76, 76 }, new List<byte> { 67, 76, 65, 83 }, new List<byte> { 67, 76, 68, 67 }, new List<byte> { 67, 76, 70, 77 }, new List<byte> { 67, 76, 77, 84 }, new List<byte> { 67, 79, 66, 74 }, new List<byte> { 67, 79, 76, 76 }, new List<byte> { 67, 79, 78, 84 }, new List<byte> { 67, 80, 84, 72 }, new List<byte> { 67, 83, 84, 89 }, new List<byte> { 68, 69, 66, 82 }, new List<byte> { 68, 73, 65, 76 }, new List<byte> { 68, 76, 66, 82 }, new List<byte> { 68, 76, 86, 87 }, new List<byte> { 68, 79, 66, 74 }, new List<byte> { 68, 79, 79, 82 }, new List<byte> { 68, 85, 65, 76 }, new List<byte> { 69, 67, 90, 78 }, new List<byte> { 69, 70, 83, 72 }, new List<byte> { 69, 78, 67, 72 }, new List<byte> { 69, 81, 85, 80 }, new List<byte> { 69, 88, 80, 76 }, new List<byte> { 69, 89, 69, 83 }, new List<byte> { 70, 65, 67, 84 }, new List<byte> { 70, 76, 79, 82 }, new List<byte> { 70, 76, 83, 84 }, new List<byte> { 70, 83, 84, 80 }, new List<byte> { 70, 83, 84, 83 }, new List<byte> { 70, 85, 82, 78 }, new List<byte> { 71, 76, 79, 66 }, new List<byte> { 71, 77, 83, 84 }, new List<byte> { 71, 82, 65, 83 }, new List<byte> { 72, 65, 73, 82 }, new List<byte> { 72, 65, 90, 68 }, new List<byte> { 72, 68, 80, 84 }, new List<byte> { 73, 68, 76, 69 }, new List<byte> { 73, 68, 76, 77 }, new List<byte> { 73, 77, 65, 68 }, new List<byte> { 73, 77, 71, 83 }, new List<byte> { 73, 78, 71, 82 }, new List<byte> { 73, 80, 67, 84 }, new List<byte> { 73, 80, 68, 83 }, new List<byte> { 75, 69, 89, 77 }, new List<byte> { 75, 89, 87, 68 }, new List<byte> { 76, 67, 82, 84 }, new List<byte> { 76, 67, 84, 78 }, new List<byte> { 76, 71, 84, 77 }, new List<byte> { 76, 73, 71, 72 }, new List<byte> { 76, 83, 67, 82 }, new List<byte> { 76, 84, 69, 88 }, new List<byte> { 76, 86, 76, 73 }, new List<byte> { 76, 86, 76, 78 }, new List<byte> { 76, 86, 83, 80 }, new List<byte> { 77, 65, 84, 79 }, new List<byte> { 77, 65, 84, 84 }, new List<byte> { 77, 69, 83, 71 }, new List<byte> { 77, 71, 69, 70 }, new List<byte> { 77, 73, 83, 67 }, new List<byte> { 77, 79, 86, 84 }, new List<byte> { 77, 83, 84, 84 }, new List<byte> { 77, 85, 83, 67 }, new List<byte> { 77, 85, 83, 84 }, new List<byte> { 78, 65, 86, 73 }, new List<byte> { 78, 80, 67, 95 }, new List<byte> { 79, 84, 70, 84 }, new List<byte> { 80, 65, 67, 75 }, new List<byte> { 80, 69, 82, 75 }, new List<byte> { 80, 82, 79, 74 }, new List<byte> { 80, 87, 65, 84 }, new List<byte> { 81, 85, 83, 84 }, new List<byte> { 82, 65, 67, 69 }, new List<byte> { 82, 69, 71, 78 }, new List<byte> { 82, 69, 76, 65 }, new List<byte> { 82, 69, 86, 66 }, new List<byte> { 82, 70, 67, 84 }, new List<byte> { 82, 71, 68, 76 }, new List<byte> { 83, 67, 69, 78 }, new List<byte> { 83, 67, 79, 76 }, new List<byte> { 83, 67, 80, 84 }, new List<byte> { 83, 67, 82, 76 }, new List<byte> { 83, 72, 79, 85 }, new List<byte> { 83, 76, 71, 77 }, new List<byte> { 83, 77, 66, 78 }, new List<byte> { 83, 77, 69, 78 }, new List<byte> { 83, 77, 81, 78 }, new List<byte> { 83, 78, 67, 84 }, new List<byte> { 83, 78, 68, 82 }, new List<byte> { 83, 79, 80, 77 }, new List<byte> { 83, 79, 85, 78 }, new List<byte> { 83, 80, 69, 76 }, new List<byte> { 83, 80, 71, 68 }, new List<byte> { 83, 84, 65, 84 }, new List<byte> { 84, 65, 67, 84 }, new List<byte> { 84, 82, 69, 69 }, new List<byte> { 84, 88, 83, 84 }, new List<byte> { 86, 84, 89, 80 }, new List<byte> { 87, 65, 84, 82 }, new List<byte> { 87, 69, 65, 80 }, new List<byte> { 87, 79, 79, 80 }, new List<byte> { 87, 82, 76, 68 }, new List<byte> { 87, 84, 72, 82 } };
        List<string> masterNames = new List<string>() { "Skyrim.esm", "Update.esm", "Dawnguard.esm", "HearthFires.esm", "Dragonborn.esm" };
        List<List<byte>> listID = new List<List<byte>>();
        List<List<byte>> listData = new List<List<byte>>();
        List<string> listMasters = new List<string>();
        static string pathFolder = pathAddSlash(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
        bool rootData = Directory.Exists(pathFolder + "Data");
        int gcount = groupNames.Count;
        int countFiles = 0;

        public FormVersion()
        {
            InitializeComponent();
            pathFolder = rootData ? pathAddSlash(Path.Combine(pathFolder + "Data")) : pathFolder;
            foreach (string line in masterNames)
            {
                if (!File.Exists(pathFolder + line))
                {
                    button1.Enabled = false;
                    break;
                }
            }
            label4.Text = button1.Enabled ? "found" : "not found";
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            button1.Text = "Working...";
            button1.Enabled = false;
            bool parsed = true;
            for (int i = 0; i < masterNames.Count; i++)
            {
                if (File.Exists(pathFolder + masterNames[i]))
                {
                    parseFile(pathFolder + masterNames[i], true, BitConverter.GetBytes(i)[0], masterNames[i]);
                }
                else
                {
                    parsed = false;
                    break;
                }
            }
            if (parsed)
            {
                foreach (string line in Directory.EnumerateFiles(pathFolder, "*.esp"))
                {
                    parseFile(line);
                    countFiles++;
                }
                foreach (string line in Directory.EnumerateFiles(pathFolder, "*.esm"))
                {
                    if (!masterNames.Exists(s => s.Equals(Path.GetFileName(line), StringComparison.OrdinalIgnoreCase)))
                    {
                        parseFile(line);
                        countFiles++;
                    }
                }
            }
            label5.Text = countFiles.ToString();
            listID.Clear();
            listMasters.Clear();
            listData.Clear();
            countFiles = 0;
            button1.Enabled = true;
            button1.Text = "Synchronize";
        }

        private void parseFile(string file, bool master = false, byte load = 0, string name = null)
        {
            List<string> headMasters = parseHead(file);
            byte[] bytesFile = File.ReadAllBytes(file);
            int fileSize = bytesFile.Length;
            bool write = false;
            for (int i = 46; i + 27 < fileSize; i++)
            {
                if (bytesFile[i - 8] != 71 && bytesFile[i - 7] != 82 && bytesFile[i - 6] != 85 && bytesFile[i - 5] != 80 && bytesFile[i] >= 65 && bytesFile[i] <= 95 && bytesFile[i + 1] >= 65 && bytesFile[i + 1] <= 95 && bytesFile[i + 2] >= 65 && bytesFile[i + 2] <= 95 && bytesFile[i + 3] >= 65 && bytesFile[i + 3] <= 95 && bytesFile[i + 20] >= 14 && bytesFile[i + 20] <= 43 && bytesFile[i + 21] == 0 && ((bytesFile[i + 24] >= 65 && bytesFile[i + 24] <= 95 && bytesFile[i + 25] >= 65 && bytesFile[i + 25] <= 95 && bytesFile[i + 26] >= 65 && bytesFile[i + 26] <= 95 && bytesFile[i + 27] >= 65 && bytesFile[i + 27] <= 95) || BitConverter.ToUInt32(bytesFile, i + 8) >> 18 >= 1) && groupNames.FindIndex(0, gcount, b => b[0] == bytesFile[i] && b[1] == bytesFile[i + 1] && b[2] == bytesFile[i + 2] && b[3] == bytesFile[i + 3]) >= 0)
                {
                    bool self = bytesFile[i + 15] >= headMasters.Count;
                    if (!self)
                    {
                        int index = masterNames.IndexOf(headMasters[bytesFile[i + 15]]);
                        if (index != -1)
                        {
                            int count = listID.Count;
                            byte bindex = BitConverter.GetBytes(index)[0];
                            index = listID.FindIndex(0, count, b => b[0] == bindex && b[1] == bytesFile[i + 14] && b[2] == bytesFile[i + 13] && b[3] == bytesFile[i + 12]);
                            if (index != -1)
                            {
                                if (headMasters[bytesFile[i + 15]].ToLower() == listMasters[index].ToLower())
                                {
                                    if (master)
                                    {
                                        listData[index] = new List<byte>() { bytesFile[i + 22], bytesFile[i + 23] };
                                    }
                                    else
                                    {
                                        bytesFile[i + 22] = listData[index][0];
                                        bytesFile[i + 23] = listData[index][1];
                                        write = true;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Master file not match with ID, file: " + file + "offset: " + i.ToString() + " ID: " + BitConverter.ToString(bytesFile, i + 12, 1) + BitConverter.ToString(bytesFile, i + 13, 1) + BitConverter.ToString(bytesFile, i + 14, 1));
                                }
                            }
                            else
                            {
                                self = master;
                            }
                        }
                    }
                    if (self && master)
                    {
                        int count = listID.Count;
                        int index = listID.FindIndex(0, count, b => b[0] == load && b[1] == bytesFile[i + 14] && b[2] == bytesFile[i + 13] && b[3] == bytesFile[i + 12]);
                        if (index != -1)
                        {
                            MessageBox.Show("Double FornID: " + Convert.ToChar(bytesFile[i]).ToString() + Convert.ToChar(bytesFile[i + 1]).ToString() + Convert.ToChar(bytesFile[i + 2]).ToString() + Convert.ToChar(bytesFile[i + 3]).ToString() + " " + load.ToString("X2") + bytesFile[i + 14].ToString("X2") + bytesFile[i + 13].ToString("X2") + bytesFile[i + 12].ToString("X2") + " at: " + (i + 14).ToString());
                        }
                        listID.Add(new List<byte> { load, bytesFile[i + 14], bytesFile[i + 13], bytesFile[i + 12] });
                        listData.Add(new List<byte>() { bytesFile[i + 22], bytesFile[i + 23] });
                        listMasters.Add(name);
                    }
                    i += 23;
                }
            }
            if (write)
            {
                File.WriteAllBytes(file, bytesFile);
            }
            bytesFile = null;
            headMasters.Clear();
        }

        private List<string> parseHead(string file)
        {
            List<string> outString = new List<string>();
            if (File.Exists(file) && new FileInfo(file).Length > 50)
            {
                try
                {
                    FileStream fs = File.OpenRead(file);
                    byte[] bytesFile = new byte[8];
                    fs.Read(bytesFile, 0, 8);
                    if (bytesFile[0] == 84 && bytesFile[1] == 69 && bytesFile[2] == 83 && bytesFile[3] == 52)
                    {
                        int length = BitConverter.ToInt32(bytesFile, 4);
                        bytesFile = new byte[length];
                        fs.Seek(28, SeekOrigin.Begin);
                        fs.Read(bytesFile, 0, length);
                        fs.Close();
                        int attempts = 0;
                        bool start = false;
                        for (int i = BitConverter.ToInt16(bytesFile, 0) + 2; (i + 6) < length; i++)
                        {
                            if (bytesFile[i] == 77 && bytesFile[i + 1] == 65 && bytesFile[i + 2] == 83 && bytesFile[i + 3] == 84)
                            {
                                start = true;
                                int count = BitConverter.ToInt16(bytesFile, i + 4) - 1;
                                byte[] bytesText = new byte[count];
                                Buffer.BlockCopy(bytesFile, i + 6, bytesText, 0, count);
                                outString.Add(Encoding.UTF8.GetString(bytesText));
                                i += count + 20;
                            }
                            else if (!start && attempts < 5)
                            {
                                attempts++;
                                i += 5 + BitConverter.ToInt16(bytesFile, i + 4);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        fs.Close();
                    }
                    bytesFile = null;
                }
                catch
                {
                    outString.Clear();
                }
            }
            return outString;
        }

        private static string pathAddSlash(string path)
        {
            if (!path.EndsWith("/") && !path.EndsWith(@"\"))
            {
                if (path.Contains("/"))
                {
                    path += "/";
                }
                else if (path.Contains(@"\"))
                {
                    path += @"\";
                }
            }
            return path;
        }
    }
}
