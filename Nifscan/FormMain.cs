using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Nifscan
{
    public partial class FormMain : Form
    {
        public static List<string> texturesList = new List<string>();
        public static bool fixesOn = false;
        public static bool parallaxInvert = false;
        public static bool parallaxCompare = false;
        public static bool parallaxRemove = false;
        public static bool parallaxAdd = false;
        public static int ecShaderType = 21;
        public static bool ecColor = false;
        public static bool ecMultiple = false;
        public static float ecR = 0;
        public static float ecG = 0;
        public static float ecB = 0;
        public static float ecM = 0;
        public static bool vcColor = false;
        public static bool vcAlpha = false;
        public static bool vcRemove = false;
        public static float vcR = 0;
        public static float vcG = 0;
        public static float vcB = 0;
        public static float vcA = 0;
        public static bool verX = false;
        public static bool verY = false;
        public static bool verZ = false;
        public static bool verXHard = false;
        public static bool verYHard = false;
        public static bool verZHard = false;
        public static double verXval = 0;
        public static double verYval = 0;
        public static double verZval = 0;
        public static bool verTrim = false;
        public static int verTrimNum = 3;
        public static bool specAvailable = false;
        public static List<string> specTextures = new List<string>();
        public static List<float> specGlossiness = new List<float>();
        public static List<float> specR = new List<float>();
        public static List<float> specG = new List<float>();
        public static List<float> specB = new List<float>();
        public static List<float> specStrength = new List<float>();
        internal static FormMain formMain = null;
        List<string> outLog = new List<string>();
        List<int> blocksSizeList = new List<int>();
        List<int> blocksStartList = new List<int>();
        List<int> sizeIndex = new List<int>();
        byte[] bytesFile = null;
        string tempPath = null;
        string fileName = null;
        bool fileChanged = false;
        bool debugMode = false;
        int blocksCount = 0;
        int totalFiles = 0;
        int successFiles = 0;
        [Flags]
        enum ShadeFlags1 : long
        {
            SLSF1_Specular = 1,
            SLSF1_Skinned = 1 << 1,
            SLSF1_Temp_Refraction = 1 << 2,
            SLSF1_Vertex_Alpha = 1 << 3,
            SLSF1_Greyscale_To_PaletteColor = 1 << 4,
            SLSF1_Greyscale_To_PaletteAlpha = 1 << 5,
            SLSF1_Use_Falloff = 1 << 6,
            SLSF1_Environment_Mapping = 1 << 7,
            SLSF1_Recieve_Shadows = 1 << 8,
            SLSF1_Cast_Shadows = 1 << 9,
            SLSF1_Facegen_Detail_Map = 1 << 10,
            SLSF1_Parallax = 1 << 11,
            SLSF1_Model_Space_Normals = 1 << 12,
            SLSF1_Non_Projective_Shadows = 1 << 13,
            SLSF1_Landscape = 1 << 14,
            SLSF1_Refraction = 1 << 15,
            SLSF1_Fire_Refraction = 1 << 16,
            SLSF1_Eye_Environment_Mapping = 1 << 17,
            SLSF1_Hair_Soft_Lighting = 1 << 18,
            SLSF1_Screendoor_Alpha_Fade = 1 << 19,
            SLSF1_Localmap_Hide_Secret = 1 << 20,
            SLSF1_FaceGen_RGB_Tint = 1 << 21,
            SLSF1_Own_Emit = 1 << 22,
            SLSF1_Projected_UV = 1 << 23,
            SLSF1_Multiple_Textures = 1 << 24,
            SLSF1_Remappable_Textures = 1 << 25,
            SLSF1_Decal = 1 << 26,
            SLSF1_Dynamic_Decal = 1 << 27,
            SLSF1_Parallax_Occlusion = 1 << 28,
            SLSF1_External_Emittance = 1 << 29,
            SLSF1_Soft_Effect = 1 << 30,
            SLSF1_ZBuffer_Test = (long)(1 << 31)
        };
        [Flags]
        enum ShadeFlags2 : long
        {
            SLSF2_ZBuffer_Write = 1,
            SLSF2_LOD_Landscape = 1 << 1,
            SLSF2_LOD_Objects = 1 << 2,
            SLSF2_No_Fade = 1 << 3,
            SLSF2_Double_Sided = 1 << 4,
            SLSF2_Vertex_Colors = 1 << 5,
            SLSF2_Glow_Map = 1 << 6,
            SLSF2_Assume_Shadowmask = 1 << 7,
            SLSF2_Packed_Tangent = 1 << 8,
            SLSF2_Multi_Index_Snow = 1 << 9,
            SLSF2_Vertex_Lighting = 1 << 10,
            SLSF2_Uniform_Scale = 1 << 11,
            SLSF2_Fit_Slope = 1 << 12,
            SLSF2_Billboard = 1 << 13,
            SLSF2_No_LOD_Land_Blend = 1 << 14,
            SLSF2_EnvMap_Light_Fade = 1 << 15,
            SLSF2_Wireframe = 1 << 16,
            SLSF2_Weapon_Blood = 1 << 17,
            SLSF2_Hide_On_Local_Map = 1 << 18,
            SLSF2_Premult_Alpha = 1 << 19,
            SLSF2_Cloud_LOD = 1 << 20,
            SLSF2_Anisotropic_Lighting = 1 << 21,
            SLSF2_No_Transparency_Multisampling = 1 << 22,
            SLSF2_Unused01 = 1 << 23,
            SLSF2_Multi_Layer_Parallax = 1 << 24,
            SLSF2_Soft_Lighting = 1 << 25,
            SLSF2_Rim_Lighting = 1 << 26,
            SLSF2_Back_Lighting = 1 << 27,
            SLSF2_Unused02 = 1 << 28,
            SLSF2_Tree_Anim = 1 << 29,
            SLSF2_Effect_Lighting = 1 << 30,
            SLSF2_HD_LOD_Objects = (long)(1 << 31)
        }
        [Flags]
        enum VectorFlags : uint
        {
            Has_UV = 1,
            Unk2 = 1 << 1,
            Unk4 = 1 << 2,
            Unk8 = 1 << 3,
            Unk16 = 1 << 4,
            Unk32 = 1 << 5,
            Unk64 = 1 << 6,
            Unk128 = 1 << 7,
            Unk256 = 1 << 8,
            Unk512 = 1 << 9,
            Unk1024 = 1 << 10,
            Unk2048 = 1 << 11,
            Has_Tangents = 1 << 12,
            Unk8192 = 1 << 13,
            Unk16384 = 1 << 14,
            Unk32768 = 1 << 15
        }

        public FormMain()
        {
            InitializeComponent();
            formMain = this;
        }

        void button1_Click(object sender, EventArgs e)
        {
            if (tempPath == null)
            {
                folderBrowserDialog1.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            else
            {
                folderBrowserDialog1.SelectedPath = tempPath;
            }
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tempPath = folderBrowserDialog1.SelectedPath;
                textBox3.Text = tempPath;
            }
        }

        void button2_Click(object sender, System.EventArgs e)
        {
            if (Directory.Exists(tempPath))
            {
                button2.Enabled = false;
                foreach (string line in Directory.EnumerateFiles(tempPath, "*.nif", SearchOption.AllDirectories))
                {
                    currentFile(line);
                    totalFiles++;
                    blocksSizeList.Clear();
                    blocksStartList.Clear();
                    sizeIndex.Clear();
                    blocksCount = 0;
                    bytesFile = null;
                    fileName = null;
                    fileChanged = false;
                }
                outLog.Insert(0, "TOTAL FOUND " + totalFiles + " FILES, " + "SUCCESS PROCESSING " + successFiles + " FILES");
                File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "NifScan.log", outLog, new UTF8Encoding(false));
                outLog.Clear();
                totalFiles = 0;
                successFiles = 0;
                button2.Enabled = true;
            }
        }

        void currentFile(string path)
        {
            if (File.Exists(path))
            {
                List<string> blocksTypesList = new List<string>();
                List<string> blocksNamesList = new List<string>();
                List<string> stringsList = new List<string>();
                List<uint> groupsList = new List<uint>();
                outLog.Add("");
                outLog.Add("PROCESS FILE: " + path);
                fileName = Path.GetFileName(path);
                bytesFile = File.ReadAllBytes(path);
                int fileSize = bytesFile.Length;
                if (fileSize < 200)
                {
                    outLog.Add("WARNING! FILE TO SMALL: " + fileName);
                    return;
                }
                string headerError = null;
                bool exportInfo = false;
                bool blocksType = false;
                bool blocksListFull = false;
                bool blocksCountFull = false;
                bool blocksSizeFull = false;
                bool stringsCount = false;
                bool maxStringLength = false;
                bool stringsFull = false;
                bool groupsCount = false;
                bool groupsFull = false;
                bool faceGenFile = path.IndexOf("facegendata", StringComparison.OrdinalIgnoreCase) != -1 && path.IndexOf("facegeom", StringComparison.OrdinalIgnoreCase) != -1;
                int exportInfoLines = 0;
                int blocksTypeCount = 0;
                int maxLengthString = 0;
                int numGroups = 0;
                int numStrings = 0;
                int blockStart = 0;
                if (bytesFile[39] != 7 || bytesFile[41] != 2 || bytesFile[42] != 20 || bytesFile[44] != 12 || (bytesFile[52] != 83 && bytesFile[52] != 32))
                {
                    outLog.Add("WARNING! FILE VERSION UNSUPPORTED: " + fileName);
                    return;
                }
                try
                {
                    blocksCount = (int)BitConverter.ToUInt32(bytesFile, 48);
                    if (blocksCount < 1)
                    {
                        outLog.Add("WARNING! FILE NOT HAVE BLOCKS OR FILE ERROR");
                        return;
                    }
                    for (int i = 56; i < fileSize; i++)
                    {
                        if (!exportInfo)
                        {
                            i += bytesFile[i];
                            exportInfoLines++;
                            if (exportInfoLines == 3)
                            {
                                exportInfo = true;
                            }
                        }
                        else if (!blocksType && exportInfo)
                        {
                            blocksTypeCount = BitConverter.ToUInt16(bytesFile, i);
                            i++;
                            blocksType = true;
                        }
                        else if (!blocksListFull && blocksType)
                        {
                            int length = (int)BitConverter.ToUInt32(bytesFile, i);
                            i += 3;
                            string word = null;
                            while (length != 0)
                            {
                                i++;
                                word += Convert.ToChar(bytesFile[i]);
                                length--;
                            }
                            blocksTypesList.Add(word);
                            if (blocksTypeCount == blocksTypesList.Count)
                            {
                                blocksListFull = true;
                            }
                        }
                        else if (!blocksCountFull && blocksListFull)
                        {
                            blocksNamesList.Add(blocksTypesList[(BitConverter.ToUInt16(bytesFile, i))]);
                            i++;
                            if (blocksCount == blocksNamesList.Count)
                            {
                                blocksCountFull = true;
                            }
                        }
                        else if (!blocksSizeFull && blocksCountFull)
                        {
                            int size = (int)BitConverter.ToUInt32(bytesFile, i);
                            sizeIndex.Add(i);
                            i += 3;
                            blocksSizeList.Add(size);
                            blocksStartList.Add(blockStart);
                            blockStart += size;
                            if (blocksCount == blocksSizeList.Count)
                            {
                                blocksSizeFull = true;
                            }
                        }
                        else if (!stringsCount && blocksSizeFull)
                        {
                            numStrings = (int)BitConverter.ToUInt32(bytesFile, i);
                            i += 3;
                            stringsCount = true;
                        }
                        else if (!maxStringLength && stringsCount)
                        {
                            maxLengthString = (int)BitConverter.ToUInt32(bytesFile, i);
                            i += 3;
                            maxStringLength = true;
                        }
                        else if (!stringsFull && maxStringLength)
                        {
                            int length = (int)BitConverter.ToUInt32(bytesFile, i);
                            i += 3;
                            string word = null;
                            while (length != 0)
                            {
                                i++;
                                word += Convert.ToChar(bytesFile[i]);
                                length--;
                            }
                            stringsList.Add(word);
                            if (numStrings == stringsList.Count)
                            {
                                stringsFull = true;
                            }
                        }
                        else if (!groupsCount && stringsFull)
                        {
                            numGroups = (int)BitConverter.ToUInt32(bytesFile, i);
                            i += 3;
                            groupsCount = true;
                            if (numGroups == 0)
                            {
                                groupsFull = true;
                            }
                        }
                        else if (!groupsFull && groupsCount)
                        {
                            groupsList.Add(BitConverter.ToUInt32(bytesFile, i));
                            i += 3;
                            if (numGroups == groupsList.Count)
                            {
                                groupsFull = true;
                            }
                        }
                        else if (groupsFull)
                        {
                            for (int j = 0; j < blocksStartList.Count; j++)
                            {
                                blocksStartList[j] = blocksStartList[j] + i;
                            }
                            if (debugMode)
                            {
                                outLog.Add("blocksTypeCount " + blocksTypeCount);
                                outLog.Add("blocksCount " + blocksCount);
                                outLog.Add("maxLengthString " + maxLengthString);
                                outLog.Add("numStrings " + numStrings);
                                outLog.Add("numGroups " + numGroups);
                                outLog.Add("");
                                outLog.Add("BLOCKS TYPE:");
                                outLog.AddRange(blocksTypesList);
                                outLog.Add("");
                                outLog.Add("BLOCKS LIST:");
                                for (int l = 0; l < blocksNamesList.Count; l++)
                                {
                                    outLog.Add(blocksNamesList[l] + " START: " + blocksStartList[l] + " SIZE: " + blocksSizeList[l]);

                                }
                                outLog.Add("");
                                outLog.Add("STRINGS LIST:");
                                outLog.AddRange(stringsList);
                                outLog.Add("");
                                outLog.Add("GROUP LIST:");
                                if (numGroups != 0)
                                {
                                    for (int l = 0; l < groupsList.Count; l++)
                                    {
                                        outLog.Add(groupsList[l].ToString());
                                    }
                                }
                                outLog.Add("");
                            }
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    headerError = e.Message;
                }
                if (headerError == null)
                {
                    if (blocksNamesList.Contains("NiTriStrips"))
                    {
                        outLog.Add("WARNING! MESH HAVE NITRISTRIPS: " + fileName);
                    }
                    int numBSX = 0;
                    int numBSFade = 0;
                    for (int i = 0; i < blocksNamesList.Count; i++)
                    {
                        int realBlockStart = blocksStartList[i];
                        if (blocksNamesList[i] == "BSXFlags")
                        {
                            int flag = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            outLog.Add("Have BSXFlags: " + flag + " " + fileName);
                            numBSX++;
                        }
                        else if (blocksNamesList[i] == "BSFadeNode")
                        {
                            numBSFade++;
                        }
                        else if (blocksNamesList[i] == "NiNode" || blocksNamesList[i] == "AvoidNode" || blocksNamesList[i] == "BSFadeNode" || blocksNamesList[i] == "BSLeafAnimNode" || blocksNamesList[i] == "RootCollisionNode" || blocksNamesList[i] == "NiBSAnimationNode" || blocksNamesList[i] == "NiBSParticleNode" || blocksNamesList[i] == "NiBone")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4);
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump3, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "BSBlastNode" || blocksNamesList[i] == "BSDamageStage" || blocksNamesList[i] == "BSDebrisNode" || blocksNamesList[i] == "BSRangeNode")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4) + 3;
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump3, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "BSMasterParticleSystem")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4) + 2;
                            int jump4 = jump3 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump3) * 4);
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump4, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "BSMultiBoundNode")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4) + 8;
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump3, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "BSOrderedNode")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4) + 17;
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump3, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "BSTreeNode")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4);
                            int jump4 = jump3 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump3) * 4);
                            int jump5 = jump4 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump4) * 4);
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump5, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "BSValueNode")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4) + 5;
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump3, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "FxButton" || blocksNamesList[i] == "FxWidget")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4) + 293;
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump3, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "FxRadioButton")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4) + 305;
                            int jump4 = jump3 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump3) * 4);
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump4, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "NiBillboardNode")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4) + 2;
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump3, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "NiLODNode")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4) + 10;
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump3, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "NiRoom")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4);
                            int jump4 = jump3 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump3) * 16);
                            int jump5 = jump4 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump4) * 4);
                            int jump6 = jump5 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump5) * 4);
                            int jump7 = jump6 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump6) * 4);
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump7, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "NiRoomGroup")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4) + 4;
                            int jump4 = jump3 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump3) * 4);
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump4, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "NiSortAdjustNode")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4) + 4;
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump3, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i] == "NiSwitchNode")
                        {
                            int extra = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extra, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extra * 4);
                            int childs = (int)BitConverter.ToUInt32(bytesFile, jump);
                            int jump2 = jump + 4 + (childs * 4);
                            int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4) + 6;
                            sortBlocks(realBlockStart, jump, childs, realBlockStart + blocksSizeList[i], jump3, i, blocksNamesList[i]);
                        }
                        else if (blocksNamesList[i].StartsWith("bhk"))
                        {
                            if (blocksNamesList[i] == "bhkRigidBody" || blocksNamesList[i] == "bhkRigidBodyT")
                            {
                                if (bytesFile[realBlockStart + 4] != bytesFile[realBlockStart + 36])
                                {
                                    outLog.Add("WARNING! COLLISION LAYERS NOT MATCH: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                }
                                if (BitConverter.ToSingle(bytesFile, realBlockStart + 192) != 1 || BitConverter.ToSingle(bytesFile, realBlockStart + 196) != 1)
                                {
                                    outLog.Add("WARNING! COLLISION FACTORS NOT EQUAL ONE: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                }
                            }
                            if (blocksNamesList[i] == "bhkConvexVerticesShape")
                            {
                                int jump = realBlockStart + 32;
                                int vertices = (int)BitConverter.ToUInt32(bytesFile, jump);
                                int jump2 = jump + 4 + (vertices * 16);
                                int normals = (int)BitConverter.ToUInt32(bytesFile, jump2);
                                bool zero = true;
                                jump2 += 4;
                                for (int j = 0; j < normals; j++)
                                {
                                    float n1 = BitConverter.ToSingle(bytesFile, jump2);
                                    jump2 += 4;
                                    float n2 = BitConverter.ToSingle(bytesFile, jump2);
                                    jump2 += 4;
                                    float n3 = BitConverter.ToSingle(bytesFile, jump2);
                                    jump2 += 8;
                                    if (n1 != 0 || n2 != 0 || n3 != 0)
                                    {
                                        zero = false;
                                        break;
                                    }
                                }
                                if (zero)
                                {
                                    outLog.Add("WARNING! ALL COLLISION NORMALS EQUAL ZERO: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                }
                            }
                            if (checkBox2.Checked || checkBox5.Checked)
                            {
                                if (blocksNamesList[i] == "bhkBoxShape" || blocksNamesList[i] == "bhkNiTriStripsShape" || blocksNamesList[i] == "bhkSphereShape" || blocksNamesList[i] == "bhkConvexVerticesShape" || blocksNamesList[i] == "bhkCapsuleShape")
                                {
                                    checkCollisions(realBlockStart, BitConverter.ToUInt32(bytesFile, realBlockStart));
                                }
                                else if (blocksNamesList[i] == "bhkConvexTransformShape" || blocksNamesList[i] == "bhkConvexSweepShape" || blocksNamesList[i] == "bhkTransformShape")
                                {
                                    checkCollisions(realBlockStart + 4, BitConverter.ToUInt32(bytesFile, realBlockStart + 4));
                                }
                                else if (blocksNamesList[i] == "bhkListShape" || blocksNamesList[i] == "bhkConvexListShape")
                                {
                                    int jump = realBlockStart + 4 + ((int)BitConverter.ToUInt32(bytesFile, realBlockStart) * 4);
                                    checkCollisions(jump, BitConverter.ToUInt32(bytesFile, jump));
                                }
                                else if (blocksNamesList[i] == "bhkCompressedMeshShapeData")
                                {
                                    int jump1 = realBlockStart + 58 + ((int)BitConverter.ToUInt32(bytesFile, realBlockStart + 54) * 4);
                                    int jump2 = jump1 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump1) * 4);
                                    int jump3 = jump2 + 4 + ((int)BitConverter.ToUInt32(bytesFile, jump2) * 4);
                                    int count = bytesFile[jump3];
                                    jump3 += 4;
                                    for (int j = 0; j < count; j++)
                                    {
                                        checkCollisions(jump3, BitConverter.ToUInt32(bytesFile, jump3));
                                        jump3 += 8;
                                    }
                                }
                            }
                        }
                        else if (blocksNamesList[i] == "NiTriShape" || blocksNamesList[i] == "BSLODTriShape")
                        {
                            int nameindex = (int)BitConverter.ToUInt32(bytesFile, realBlockStart);
                            string shapename = nameindex >= 0 ? stringsList[nameindex] : "";
                            int extraBlocks = (int)BitConverter.ToUInt32(bytesFile, realBlockStart + 4);
                            extraCheck(realBlockStart + 8, extraBlocks, i, blocksNamesList[i]);
                            int jump = realBlockStart + 72 + (extraBlocks * 4);
                            int dataBlock = (int)BitConverter.ToUInt32(bytesFile, jump);
                            jump += 4;
                            int skinBlock = (int)BitConverter.ToUInt32(bytesFile, jump);
                            jump += 4;
                            int jump2 = jump + 9 + ((int)BitConverter.ToUInt32(bytesFile, jump) * 8);
                            int shaderBlock = (int)BitConverter.ToUInt32(bytesFile, jump2);
                            int alphaBlock = (int)BitConverter.ToUInt32(bytesFile, jump2 + 4);
                            int lods = 0;
                            if (blocksNamesList[i] == "BSLODTriShape")
                            {
                                lods += (int)BitConverter.ToUInt32(bytesFile, jump2 + 8);
                                lods += (int)BitConverter.ToUInt32(bytesFile, jump2 + 12);
                                lods += (int)BitConverter.ToUInt32(bytesFile, jump2 + 16);
                            }
                            long vectorFlags = 0;
                            int vFlags = 0;
                            int normalsStart = 0;
                            int tangentsStart = 0;
                            int bitangentsStart = 0;
                            int numVertices = 0;
                            int vcStart = 0;
                            int vcYesNo = 0;
                            bool hasTangents = false;
                            bool hasUV = false;
                            bool hasNormals = false;
                            bool hasVColors = false;
                            bool vaEmpty = false;
                            bool vcEmpty = vcRemove;
                            if (((blocksNamesList[i] == "NiTriShape" && (realBlockStart + blocksSizeList[i]) == (jump2 + 8)) || (blocksNamesList[i] == "BSLODTriShape" && (realBlockStart + blocksSizeList[i]) == (jump2 + 20))) && (dataBlock >= -1 && dataBlock < blocksCount) && (skinBlock >= -1 && skinBlock < blocksCount) && (shaderBlock >= -1 && shaderBlock < blocksCount) && (alphaBlock >= -1 && alphaBlock < blocksCount))
                            {
                                if ((dataBlock != -1 && blocksNamesList[dataBlock] != "NiTriShapeData" && blocksNamesList[dataBlock] != "NiTriStripsData") || (skinBlock != -1 && blocksNamesList[skinBlock] != "NiSkinInstance" && blocksNamesList[skinBlock] != "BSDismemberSkinInstance") || (shaderBlock != -1 && blocksNamesList[shaderBlock] != "BSLightingShaderProperty" && blocksNamesList[shaderBlock] != "BSEffectShaderProperty" && blocksNamesList[shaderBlock] != "BSWaterShaderProperty" && blocksNamesList[shaderBlock] != "BSSkyShaderProperty") || (alphaBlock != -1 && blocksNamesList[alphaBlock] != "NiAlphaProperty"))
                                {
                                    outLog.Add("WARNING! INCORRECT BLOCKS LINKS: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                }
                                if (dataBlock != -1)
                                {
                                    if ((i + 1 + extraBlocks) != dataBlock)
                                    {
                                        outLog.Add("WARNING! NEED REORDER: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                    }
                                    if (blocksNamesList[dataBlock] == "NiTriStripsData")
                                    {
                                        outLog.Add("WARNING! MESH HAVE NITRISTRIPSDATA: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                    }
                                    if (blocksNamesList[dataBlock] == "NiTriShapeData")
                                    {
                                        realBlockStart = blocksStartList[dataBlock];
                                        int jump3 = realBlockStart + 4;
                                        numVertices = BitConverter.ToUInt16(bytesFile, jump3);
                                        if (numVertices >= 65536)
                                        {
                                            outLog.Add("WARNING! OUT OF RANGE MAX VERTICES: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                        }
                                        jump3 += 4;
                                        if (bytesFile[jump3] == 1 && (verTrim || verX || verY || verZ))
                                        {
                                            jump3++;
                                            for (int j = 0; j < numVertices; j++)
                                            {
                                                if (verTrim)
                                                {
                                                    double vertice = (double)BitConverter.ToSingle(bytesFile, jump3);
                                                    verticesTrim(jump3, (double)Math.Round(vertice, 6));
                                                }
                                                if (verXHard)
                                                {
                                                    replaceBytesInFile(jump3, BitConverter.GetBytes((float)verXval));
                                                }
                                                else if (verX)
                                                {
                                                    replaceBytesInFile(jump3, BitConverter.GetBytes(BitConverter.ToSingle(bytesFile, jump3) + (float)verXval));
                                                }
                                                jump3 += 4;
                                                if (verTrim)
                                                {
                                                    double vertice = (double)BitConverter.ToSingle(bytesFile, jump3);
                                                    verticesTrim(jump3, (double)Math.Round(vertice, 6));
                                                }
                                                if (verYHard)
                                                {
                                                    replaceBytesInFile(jump3, BitConverter.GetBytes((float)verYval));
                                                }
                                                else if (verY)
                                                {
                                                    replaceBytesInFile(jump3, BitConverter.GetBytes(BitConverter.ToSingle(bytesFile, jump3) + (float)verYval));
                                                }
                                                jump3 += 4;
                                                if (verTrim)
                                                {
                                                    double vertice = (double)BitConverter.ToSingle(bytesFile, jump3);
                                                    verticesTrim(jump3, (double)Math.Round(vertice, 6));
                                                }
                                                if (verZHard)
                                                {
                                                    replaceBytesInFile(jump3, BitConverter.GetBytes((float)verZval));
                                                }
                                                else if (verZ)
                                                {
                                                    replaceBytesInFile(jump3, BitConverter.GetBytes(BitConverter.ToSingle(bytesFile, jump3) + (float)verZval));
                                                }
                                                jump3 += 4;
                                            }
                                        }
                                        else
                                        {
                                            jump3++;
                                            jump3 += numVertices * 12;
                                        }
                                        vFlags = jump3;
                                        vectorFlags = BitConverter.ToUInt16(bytesFile, vFlags);
                                        hasTangents = ((VectorFlags)vectorFlags & VectorFlags.Has_Tangents) != 0;
                                        hasUV = ((VectorFlags)vectorFlags & VectorFlags.Has_UV) != 0;
                                        jump3 += 6;
                                        if (bytesFile[jump3] == 1)
                                        {
                                            hasNormals = true;
                                            normalsStart = jump3;
                                            jump3 += numVertices * 12;
                                            if (hasTangents)
                                            {
                                                tangentsStart = jump3;
                                                jump3 += numVertices * 12;
                                                bitangentsStart = jump3;
                                                jump3 += numVertices * 12;
                                            }
                                        }
                                        if (shaderBlock != -1)
                                        {
                                            if (shapename == "EditorMarker" || (blocksNamesList[shaderBlock] != "BSLightingShaderProperty"))
                                            {
                                                if (hasTangents)
                                                {
                                                    if (checkBox4.Checked)
                                                    {
                                                        vectorFlags -= 4096;
                                                        replaceBytesInFile(vFlags, BitConverter.GetBytes(vectorFlags));
                                                        hasTangents = false;
                                                        resizeByteArray(tangentsStart, numVertices * 24, dataBlock);
                                                        if (jump3 > tangentsStart)
                                                        {
                                                            jump3 -= numVertices * 24;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        outLog.Add("WARNING! MARKERS/EFFECTS NO NEED TANGENTS: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                                    }
                                                }
                                                if (!hasNormals)
                                                {
                                                    outLog.Add("WARNING! MARKERS/EFFECTS WITHOUT NORMALS: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                                }
                                            }
                                            else if ((hasTangents && !hasNormals) || (!hasTangents && hasNormals))
                                            {
                                                outLog.Add("WARNING! TANGENTS NOT WORK: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                            }
                                        }
                                        jump3 += 17;
                                        if (bytesFile[jump3] == 1)
                                        {
                                            vcYesNo = jump3;
                                            jump3 = jump3 + (numVertices * 16);
                                            hasVColors = true;
                                            if (checkBox4.Checked || vcRemove || vcColor || vcAlpha)
                                            {
                                                int jump4 = vcYesNo + 1;
                                                bool va = true;
                                                vcStart = jump4;
                                                for (int j = 0; j < numVertices; j++)
                                                {
                                                    if (checkBox4.Checked || vcRemove)
                                                    {
                                                        float c1 = floatRoundUp(jump4);
                                                        jump4 += 4;
                                                        float c2 = floatRoundUp(jump4);
                                                        jump4 += 4;
                                                        float c3 = floatRoundUp(jump4);
                                                        jump4 += 4;
                                                        float c4 = floatRoundUp(jump4);
                                                        jump4 += 4;
                                                        if (va)
                                                        {
                                                            if (c4 > 0.9)
                                                            {
                                                                vaEmpty = true;
                                                            }
                                                            else
                                                            {
                                                                va = false;
                                                                vaEmpty = false;
                                                            }
                                                        }
                                                        if (vcRemove && (c1 < 0.9 || c2 < 0.9 || c3 < 0.9 || c4 < 0.9))
                                                        {
                                                            vcEmpty = false;
                                                            if (!checkBox4.Checked)
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (vcColor)
                                                        {
                                                            replaceBytesInFile(jump4, BitConverter.GetBytes(vcR));
                                                            jump4 += 4;
                                                            replaceBytesInFile(jump4, BitConverter.GetBytes(vcG));
                                                            jump4 += 4;
                                                            replaceBytesInFile(jump4, BitConverter.GetBytes(vcB));
                                                            jump4 += 4;
                                                        }
                                                        else
                                                        {
                                                            jump4 += 12;
                                                        }
                                                        if (vcAlpha)
                                                        {
                                                            replaceBytesInFile(jump4, BitConverter.GetBytes(vcA));
                                                        }
                                                        jump4 += 4;
                                                    }
                                                }
                                            }
                                        }
                                        jump3++;
                                        if (hasUV)
                                        {
                                            jump3 += numVertices * 8;
                                        }
                                        int numTriangles = BitConverter.ToUInt16(bytesFile, jump3 + 6);
                                        if (numTriangles >= 65536)
                                        {
                                            outLog.Add("WARNING! OUT OF RANGE MAX TRIANGLES: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                        }
                                        if (numVertices > 0 && numTriangles < 1)
                                        {
                                            outLog.Add("WARNING! NO TRIANGLES: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                        }
                                        if (blocksNamesList[i] == "BSLODTriShape" && numTriangles < lods)
                                        {
                                            outLog.Add("WARNING! LODS MORE TRIANGLES: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                        }
                                        jump3 = jump3 + 12;
                                        if (bytesFile[jump3] == 1)
                                        {
                                            jump3 = jump3 + (numTriangles * 6);
                                        }
                                        jump3++;
                                        int numMatch = BitConverter.ToUInt16(bytesFile, jump3);
                                        jump3 = jump3 + 2;
                                        if (numMatch > 0)
                                        {
                                            for (int j = 0; j < numMatch; j++)
                                            {
                                                jump3 = jump3 + 2 + (BitConverter.ToUInt16(bytesFile, jump3) * 2);
                                            }
                                        }
                                        if ((realBlockStart + blocksSizeList[dataBlock]) != jump3)
                                        {
                                            outLog.Add("WARNING! ERROR PARS: " + blocksNamesList[dataBlock] + " (" + dataBlock + ") " + fileName);
                                        }
                                    }
                                }
                                else
                                {
                                    outLog.Add("WARNING! NO DATA: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                }
                                if (shaderBlock != -1 && blocksNamesList[shaderBlock] == "BSLightingShaderProperty")
                                {
                                    if (i > shaderBlock || (dataBlock != -1 && dataBlock > shaderBlock))
                                    {
                                        outLog.Add("WARNING! NEED REORDER: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                    }
                                    realBlockStart = blocksStartList[shaderBlock];
                                    int jump3 = realBlockStart + 16 + ((int)BitConverter.ToUInt32(bytesFile, realBlockStart + 8) * 4);
                                    if (jump3 > realBlockStart && (realBlockStart + blocksSizeList[shaderBlock]) > jump3)
                                    {
                                        long shaderFlags1 = BitConverter.ToUInt32(bytesFile, jump3);
                                        bool hasCastShadowFlag = ((ShadeFlags1)shaderFlags1 & ShadeFlags1.SLSF1_Cast_Shadows) != 0;
                                        bool hasDecalFlag = ((ShadeFlags1)shaderFlags1 & ShadeFlags1.SLSF1_Decal) != 0;
                                        bool hasEnvMapFlag = ((ShadeFlags1)shaderFlags1 & ShadeFlags1.SLSF1_Environment_Mapping) != 0;
                                        bool hasEyeEnvMapFlag = ((ShadeFlags1)shaderFlags1 & ShadeFlags1.SLSF1_Eye_Environment_Mapping) != 0;
                                        bool hasParallaxFlag = ((ShadeFlags1)shaderFlags1 & ShadeFlags1.SLSF1_Parallax) != 0;
                                        bool hasRefractionFlag = ((ShadeFlags1)shaderFlags1 & ShadeFlags1.SLSF1_Refraction) != 0;
                                        bool hasSkinFlag = ((ShadeFlags1)shaderFlags1 & ShadeFlags1.SLSF1_Skinned) != 0;
                                        bool hasSpaceNormalsFlag = ((ShadeFlags1)shaderFlags1 & ShadeFlags1.SLSF1_Model_Space_Normals) != 0;
                                        bool hasSpecular = ((ShadeFlags1)shaderFlags1 & ShadeFlags1.SLSF1_Specular) != 0;
                                        bool hasVAlphaFlag = ((ShadeFlags1)shaderFlags1 & ShadeFlags1.SLSF1_Vertex_Alpha) != 0;
                                        int textureSetBlock = (int)BitConverter.ToUInt32(bytesFile, jump3 + 24);
                                        int shader1 = jump3;
                                        int specIndex = -1;
                                        bool checkEye = false;
                                        bool checkHair = false;
                                        bool checkMouth = false;
                                        bool hasPTexture = false;
                                        bool hasSTexture = false;
                                        bool hasSKTexture = false;
                                        bool overWrite = false;
                                        string normal = null;
                                        if (textureSetBlock != -1)
                                        {
                                            int jump4 = blocksStartList[textureSetBlock];
                                            int num = (int)BitConverter.ToUInt32(bytesFile, jump4);
                                            if (num != 9)
                                            {
                                                outLog.Add("WARNING! COUNT OF TEXTURES: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                            }
                                            jump4 += 4;
                                            if (debugMode)
                                            {
                                                outLog.Add("");
                                                outLog.Add("TEXTURES LIST:");
                                            }
                                            for (int j = 0; j < num; j++)
                                            {
                                                int length = (int)BitConverter.ToUInt32(bytesFile, jump4);
                                                jump4 += 4;
                                                string word = null;
                                                while (length != 0)
                                                {
                                                    word += Convert.ToChar(bytesFile[jump4]);
                                                    jump4++;
                                                    length--;
                                                }
                                                if (!String.IsNullOrEmpty(word))
                                                {
                                                    if (j == 0 && faceGenFile)
                                                    {
                                                        if (word.IndexOf("eyes", StringComparison.OrdinalIgnoreCase) != -1)
                                                        {
                                                            checkEye = true;
                                                        }
                                                        else if (word.IndexOf("mouth", StringComparison.OrdinalIgnoreCase) != -1)
                                                        {
                                                            checkMouth = true;
                                                        }
                                                        else if (word.IndexOf("hair", StringComparison.OrdinalIgnoreCase) != -1)
                                                        {
                                                            checkHair = true;
                                                        }
                                                    }
                                                    else if (j == 1)
                                                    {
                                                        normal = word;
                                                        int count = specTextures.Count;
                                                        for (int l = 0; l < count; l++)
                                                        {
                                                            if (String.Equals(normal, specTextures[l].Replace("!", ""), StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                overWrite = specTextures[l].StartsWith("!");
                                                                specIndex = l;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else if (j == 2)
                                                    {
                                                        hasSKTexture = true;
                                                    }
                                                    else if (j == 3 && word.EndsWith("_p.dds", StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        if (parallaxCompare && !word.Remove(word.Length - 6).Equals(normal.Remove(normal.Length - 6), StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            outLog.Add("WARNING! _P TEXTURE NOT EQUAL _N: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                                        }
                                                        hasPTexture = true;
                                                    }
                                                    else if (j == 7)
                                                    {
                                                        hasSTexture = true;
                                                    }
                                                    if (debugMode)
                                                    {
                                                        outLog.Add((j + 1) + " " + word);
                                                    }
                                                }
                                            }
                                        }
                                        if (parallaxAdd && normal != null && texturesList.Exists(s => s.Equals(normal, StringComparison.OrdinalIgnoreCase)))
                                        {
                                            if (!parallaxInvert && bytesFile[realBlockStart] == 0 && !hasParallaxFlag && skinBlock == -1)
                                            {
                                                bytesFile[realBlockStart] = 3;
                                                shaderFlags1 += 2048;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags1));
                                                hasParallaxFlag = true;
                                            }
                                            else if (parallaxInvert && bytesFile[realBlockStart] == 3 && hasParallaxFlag)
                                            {
                                                bytesFile[realBlockStart] = 0;
                                                shaderFlags1 -= 2048;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags1));
                                                hasParallaxFlag = false;
                                            }
                                        }
                                        else if (parallaxRemove && (bytesFile[realBlockStart] == 3 || hasParallaxFlag))
                                        {
                                            bytesFile[realBlockStart] = 0;
                                            shaderFlags1 -= 2048;
                                            replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags1));
                                            hasParallaxFlag = false;
                                        }
                                        else if (((bytesFile[realBlockStart] == 3 && !hasParallaxFlag) || (bytesFile[realBlockStart] != 3 && hasParallaxFlag)) || ((bytesFile[realBlockStart] == 3 && hasParallaxFlag && (!hasPTexture || !hasVColors || !hasNormals || !hasTangents || skinBlock != -1))) || (hasPTexture && (bytesFile[realBlockStart] != 3 || !hasParallaxFlag)))
                                        {
                                            outLog.Add("WARNING! PARALLAX NOT WORK: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                        }
                                        if (hasCastShadowFlag && ((checkEye || checkMouth) || (alphaBlock != -1 && hasDecalFlag)))
                                        {
                                            if (checkBox4.Checked)
                                            {
                                                shaderFlags1 -= 512;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags1));
                                                hasCastShadowFlag = false;
                                            }
                                            else
                                            {
                                                outLog.Add("WARNING! UNNECESSARY CAST SHADOW FLAG: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                            }
                                        }
                                        if (hasVAlphaFlag && (!hasVColors || vaEmpty || (alphaBlock == -1 && !hasRefractionFlag)))
                                        {
                                            if (checkBox4.Checked)
                                            {
                                                shaderFlags1 -= 8;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags1));
                                                hasVAlphaFlag = false;
                                            }
                                            else
                                            {
                                                outLog.Add("WARNING! USELESS ALPHA FLAG IN TREE: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                            }
                                        }
                                        if ((checkBox6.Checked || checkBox7.Checked) && comboBox1.SelectedIndex != -1)
                                        {
                                            if (((ShadeFlags1)shaderFlags1 & (ShadeFlags1)(1 << comboBox1.SelectedIndex)) != 0)
                                            {
                                                if (checkBox7.Checked)
                                                {
                                                    shaderFlags1 -= 1 << comboBox1.SelectedIndex;
                                                    replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags1));
                                                }
                                            }
                                            else
                                            {
                                                if (checkBox6.Checked)
                                                {
                                                    shaderFlags1 += 1 << comboBox1.SelectedIndex;
                                                    replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags1));
                                                }
                                            }
                                        }
                                        if (checkBox4.Checked && hasEyeEnvMapFlag && bytesFile[realBlockStart] == 16)
                                        {
                                            bytesFile[realBlockStart] = 1;
                                            shaderFlags1 -= 131072;
                                            replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags1));
                                            hasEyeEnvMapFlag = false;
                                            if (!hasEnvMapFlag)
                                            {
                                                shaderFlags1 += 128;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags1));
                                                hasEnvMapFlag = true;
                                            }
                                            int shaderTrimStart = realBlockStart + blocksSizeList[shaderBlock] - 24;
                                            resizeByteArray(shaderTrimStart, 24, shaderBlock);
                                            if (realBlockStart > shaderTrimStart)
                                            {
                                                realBlockStart -= 24;
                                            }
                                            if (jump3 > shaderTrimStart)
                                            {
                                                jump3 -= 24;
                                            }
                                        }
                                        if ((hasEnvMapFlag && bytesFile[realBlockStart] != 1) || (!hasEnvMapFlag && bytesFile[realBlockStart] == 1) || (hasEyeEnvMapFlag && bytesFile[realBlockStart] != 16) || (!hasEyeEnvMapFlag && bytesFile[realBlockStart] == 16))
                                        {
                                            outLog.Add("WARNING! ENVIRONMENT MAP SHADER TYPE AND FLAG CONFLICT: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                        }
                                        if ((skinBlock != -1 && !hasSkinFlag) || (skinBlock == -1 && hasSkinFlag))
                                        {
                                            outLog.Add("WARNING! SKIN IN TREE AND SKIN SHADER FLAG CONFLICT: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                        }
                                        jump3 += 4;
                                        long shaderFlags2 = BitConverter.ToUInt32(bytesFile, jump3);
                                        bool hasAnisotropicLightingFlag = ((ShadeFlags2)shaderFlags2 & ShadeFlags2.SLSF2_Anisotropic_Lighting) != 0;
                                        bool hasGlowFlag = ((ShadeFlags2)shaderFlags2 & ShadeFlags2.SLSF2_Glow_Map) != 0;
                                        bool hasSoftLightingFlag = ((ShadeFlags2)shaderFlags2 & ShadeFlags2.SLSF2_Soft_Lighting) != 0;
                                        bool hasVCFlag = ((ShadeFlags2)shaderFlags2 & ShadeFlags2.SLSF2_Vertex_Colors) != 0;
                                        if (hasVColors && vcRemove && vcEmpty && bytesFile[realBlockStart] != 3 && bytesFile[realBlockStart] >= 0 && bytesFile[realBlockStart] <= 6)
                                        {
                                            if (hasVCFlag)
                                            {
                                                shaderFlags2 -= 32;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags2));
                                                hasVCFlag = false;
                                            }
                                            bytesFile[vcYesNo] = 0;
                                            resizeByteArray(vcStart, numVertices * 16, dataBlock);
                                            hasVColors = false;
                                            if (realBlockStart > vcStart)
                                            {
                                                realBlockStart -= numVertices * 16;
                                            }
                                            if (jump3 > vcStart)
                                            {
                                                jump3 -= numVertices * 16;
                                            }
                                        }
                                        if ((hasVCFlag && !hasVColors) || (!hasVCFlag && hasVColors))
                                        {
                                            outLog.Add("WARNING! VERTEX COLORS FLAGS NOT MATCH: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                        }
                                        if ((bytesFile[realBlockStart] == 2 && !hasGlowFlag) || (bytesFile[realBlockStart] != 2 && hasGlowFlag))
                                        {
                                            outLog.Add("WARNING! GLOW TYPE SHADER NOT MATCH: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                        }
                                        if (!hasSKTexture && hasSoftLightingFlag)
                                        {
                                            if (checkBox4.Checked)
                                            {
                                                shaderFlags2 -= 33554432;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags2));
                                                hasSoftLightingFlag = false;
                                            }
                                            else
                                            {
                                                outLog.Add("WARNING! HAS SOFTLIGHTING FLAG BUT TEXTURE NOT: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                            }
                                        }
                                        if ((checkBox8.Checked || checkBox9.Checked) && comboBox2.SelectedIndex != -1)
                                        {
                                            if (((ShadeFlags2)shaderFlags2 & (ShadeFlags2)(1 << comboBox2.SelectedIndex)) != 0)
                                            {
                                                if (checkBox9.Checked)
                                                {
                                                    shaderFlags2 -= 1 << comboBox2.SelectedIndex;
                                                    replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags2));
                                                }
                                            }
                                            else
                                            {
                                                if (checkBox8.Checked)
                                                {
                                                    shaderFlags2 += 1 << comboBox2.SelectedIndex;
                                                    replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags2));
                                                }
                                            }
                                        }
                                        if (checkBox4.Checked)
                                        {
                                            if (checkHair && hasAnisotropicLightingFlag)
                                            {
                                                shaderFlags2 -= 2097152;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags2));
                                                hasAnisotropicLightingFlag = false;
                                            }
                                            else if (checkMouth && !hasAnisotropicLightingFlag && hasSpecular)
                                            {
                                                shaderFlags2 += 2097152;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(shaderFlags2));
                                                hasAnisotropicLightingFlag = true;
                                            }
                                        }
                                        jump3 += 24;
                                        if ((ecColor || ecMultiple) && (bytesFile[realBlockStart] == ecShaderType || ecShaderType == 21))
                                        {
                                            if (ecColor)
                                            {
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(ecR));
                                                jump3 += 4;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(ecG));
                                                jump3 += 4;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(ecB));
                                                jump3 += 4;
                                            }
                                            else
                                            {
                                                jump3 += 12;
                                            }
                                            if (ecMultiple)
                                            {
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(ecM));
                                            }
                                            jump3 += 4;
                                        }
                                        else if (checkBox4.Checked)
                                        {
                                            float c1 = floatRoundUp(jump3);
                                            jump3 += 4;
                                            float c2 = floatRoundUp(jump3);
                                            jump3 += 4;
                                            float c3 = floatRoundUp(jump3);
                                            jump3 += 4;
                                            float multi = BitConverter.ToSingle(bytesFile, jump3);
                                            if (multi != 0 && c1 == 0 && c2 == 0 && c3 == 0 && bytesFile[realBlockStart] != 2)
                                            {
                                                replaceBytesInFile(jump3, BitConverter.GetBytes(0));
                                            }
                                            jump3 += 4;
                                        }
                                        else
                                        {
                                            jump3 += 16;
                                        }
                                        jump3 += 12;
                                        if ((bytesFile[realBlockStart] == 4 || bytesFile[realBlockStart] == 5) && hasSkinFlag && hasSpaceNormalsFlag)
                                        {
                                            if (hasTangents || hasNormals || hasVColors || hasVCFlag)
                                            {
                                                outLog.Add("WARNING! SKIN INCORRECT PARAMETERS: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                            }
                                            if (checkBox10.Checked)
                                            {
                                                replaceBytesInFile(jump3, BitConverter.GetBytes((float)23));
                                                jump3 += 4;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes((float)0.663));
                                                jump3 += 4;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes((float)0.855));
                                                jump3 += 4;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes((float)1));
                                                jump3 += 4;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes((float)4.5));
                                                jump3 += 4;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes((float)0.4));
                                                jump3 += 4;
                                                replaceBytesInFile(jump3, BitConverter.GetBytes((float)2));
                                            }
                                            else
                                            {
                                                jump3 += 24;
                                            }
                                        }
                                        else if (specIndex >= 0 && specAvailable && (!hasSpecular || overWrite))
                                        {
                                            if (!hasSpecular)
                                            {
                                                shaderFlags1 += 1;
                                                replaceBytesInFile(shader1, BitConverter.GetBytes(shaderFlags1));
                                                hasSpecular = true;
                                            }
                                            replaceBytesInFile(jump3, BitConverter.GetBytes(specGlossiness[specIndex]));
                                            jump3 += 4;
                                            replaceBytesInFile(jump3, BitConverter.GetBytes(specR[specIndex] <= 1 ? specR[specIndex] : (float)1));
                                            jump3 += 4;
                                            replaceBytesInFile(jump3, BitConverter.GetBytes(specG[specIndex] <= 1 ? specG[specIndex] : (float)1));
                                            jump3 += 4;
                                            replaceBytesInFile(jump3, BitConverter.GetBytes(specB[specIndex] <= 1 ? specB[specIndex] : (float)1));
                                            jump3 += 4;
                                            replaceBytesInFile(jump3, BitConverter.GetBytes(specStrength[specIndex]));
                                            jump3 += 8;
                                        }
                                        else if (hasSpecular)
                                        {
                                            jump3 += 4;
                                            float c1 = 0;
                                            float c2 = 0;
                                            float c3 = 0;
                                            if (checkBox4.Checked)
                                            {
                                                c1 = floatRoundUp(jump3);
                                                jump3 += 4;
                                                c2 = floatRoundUp(jump3);
                                                jump3 += 4;
                                                c3 = floatRoundUp(jump3);
                                            }
                                            else
                                            {
                                                c1 = BitConverter.ToSingle(bytesFile, jump3);
                                                jump3 += 4;
                                                c2 = BitConverter.ToSingle(bytesFile, jump3);
                                                jump3 += 4;
                                                c3 = BitConverter.ToSingle(bytesFile, jump3);
                                            }
                                            jump3 += 4;
                                            float stren = BitConverter.ToSingle(bytesFile, jump3);
                                            if ((c1 < 0.1 && c2 < 0.1 && c3 < 0.1) || stren < 0.1)
                                            {
                                                outLog.Add("WARNING! BLACK SPECULAR: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                            }
                                            jump3 += 8;
                                        }
                                        else if (!hasSpecular && checkBox4.Checked)
                                        {
                                            replaceBytesInFile(jump3, BitConverter.GetBytes((float)80));
                                            jump3 += 4;
                                            replaceBytesInFile(jump3, BitConverter.GetBytes((float)0));
                                            jump3 += 4;
                                            replaceBytesInFile(jump3, BitConverter.GetBytes((float)0));
                                            jump3 += 4;
                                            replaceBytesInFile(jump3, BitConverter.GetBytes((float)0));
                                            jump3 += 4;
                                            replaceBytesInFile(jump3, BitConverter.GetBytes((float)1));
                                            jump3 += 8;
                                        }
                                        else
                                        {
                                            jump3 += 24;
                                        }
                                        if (hasEnvMapFlag)
                                        {
                                            jump3 += 4;
                                            if (BitConverter.ToSingle(bytesFile, jump3) < 0.1)
                                            {
                                                outLog.Add("WARNING! ENVMAP SCALE LOW: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                            }
                                        }
                                        if ((hasAnisotropicLightingFlag && !hasSpecular) || (!hasSpecular && hasSpaceNormalsFlag && hasSTexture) || (hasSpecular && hasSpaceNormalsFlag && !hasSTexture))
                                        {
                                            outLog.Add("WARNING! SPECULAR PROBLEM: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                                        }
                                    }
                                    else
                                    {
                                        outLog.Add("WARNING! ERROR PARS: " + blocksNamesList[shaderBlock] + " (" + shaderBlock + ") " + fileName);
                                    }
                                }
                            }
                            else
                            {
                                outLog.Add("WARNING! ERROR PARS: " + blocksNamesList[i] + " (" + i + ") " + fileName);
                            }
                        }
                    }
                    if (numBSX > 1 || numBSFade > 1)
                    {
                        outLog.Add("WARNING! BS BLOCKS MORE THAN ONE: " + fileName);
                    }
                    if (fileChanged)
                    {
                        File.WriteAllBytes(path, bytesFile);
                        outLog.Add("ATTENTION! FILE WAS UPDATE: " + fileName);
                    }
                    successFiles++;
                }
                else
                {
                    outLog.Add("WARNING! HEADER READ ERROR: " + fileName + " SOME REASON: " + headerError);
                }
                blocksTypesList = null;
                blocksNamesList = null;
                stringsList = null;
                groupsList = null;
            }
        }

        void verticesTrim(int jump, double value)
        {
            string premath = value.ToString();
            int index = premath.IndexOf(",");
            double trim = 0;
            if (index > 0)
            {
                if (premath.Length > (index + verTrimNum + 1))
                {
                    premath = premath.Remove(index + verTrimNum + 1);
                }
                Double.TryParse(premath.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out trim);
                replaceBytesInFile(jump, BitConverter.GetBytes((float)trim));
            }
        }

        void extraCheck(int start, int count, int blockIndex, string name)
        {
            for (int i = 0; i < count; i++)
            {
                int extra = (int)BitConverter.ToUInt32(bytesFile, start);
                if (extra < 0 || extra > blocksCount)
                {
                    outLog.Add("WARNING! EXTRA BLOCKS: " + name + " (" + blockIndex + ") " + fileName);
                }
                start += 4;
            }
        }

        void sortBlocks(int blockStart, int jump, int childs, int blockSize, int blockEnd, int blockIndex, string name)
        {
            if (childs > 0)
            {
                List<int> childsList = new List<int>();
                bool dubBlocks = false;
                bool warnBlocks = false;
                jump += 4;
                int startList = jump;
                if (blockSize == blockEnd)
                {
                    for (int i = 0; i < childs; i++)
                    {
                        int child = (int)BitConverter.ToUInt32(bytesFile, jump);
                        if (child == -1)
                        {
                            childsList.Add(-1);
                            outLog.Add("WARNING! NONE CHILDREN BLOCKS: " + name + " (" + blockIndex + ") " + fileName);
                        }
                        else if (childsList.Contains(child))
                        {
                            childsList.Add(-1);
                            outLog.Add("WARNING! DUB CHILDREN BLOCKS: " + name + " (" + blockIndex + ") " + fileName);
                            dubBlocks = true;
                        }
                        else if (child >= 0 && child < blocksCount)
                        {
                            childsList.Add(child);
                        }
                        else
                        {
                            warnBlocks = true;
                            outLog.Add("WARNING! NOT VALID CHILDREN BLOCKS: " + name + " (" + blockIndex + ") " + fileName);
                        }
                        jump += 4;
                    }
                }
                else
                {
                    warnBlocks = true;
                    outLog.Add("WARNING! ERROR PARS: " + name + " (" + blockIndex + "): " + fileName);
                }
                if (checkBox3.Checked)
                {
                    if (!warnBlocks)
                    {
                        if (dubBlocks)
                        {
                            outLog.Add("WARNING! DUB CHILDREN BLOCKS WAS REPLACED TO NONE: " + name + " (" + blockIndex + ") " + fileName);
                        }
                        childsList.Sort();
                        int listCount = 0;
                        for (int i = 0; listCount < childsList.Count; i++)
                        {
                            if (childsList[i] == -1)
                            {
                                childsList.RemoveAt(i);
                                childsList.Add(-1);
                                i--;
                            }
                            listCount++;
                        }
                        for (int j = 0; j < childsList.Count; j++)
                        {
                            replaceBytesInFile(startList, BitConverter.GetBytes(childsList[j]));
                            startList += 4;
                        }
                    }
                    else
                    {
                        outLog.Add("WARNING! SORTING NOT APPLICABLE: " + name + " (" + blockIndex + ") " + fileName);
                    }
                }
            }
            else if (childs < 0)
            {
                outLog.Add("WARNING! ERROR PARS: " + name + " (" + blockIndex + "): " + fileName);
            }
        }

        private float floatRoundUp(int jump)
        {
            float n = BitConverter.ToSingle(bytesFile, jump);
            if (float.IsNaN(n) || n > 0.989)
            {
                replaceBytesInFile(jump, BitConverter.GetBytes((float)1));
                n = (float)1;
            }
            else if (n < 0.011)
            {
                replaceBytesInFile(jump, BitConverter.GetBytes((float)0));
                n = (float)0;
            }
            return n;
        }

        void resizeByteArray(int start, int length, int block)
        {
            List<byte> list = new List<byte>(bytesFile);
            list.RemoveRange(start, length);
            bytesFile = null;
            bytesFile = list.ToArray();
            list = null;
            blocksSizeList[block] = blocksSizeList[block] - length;
            replaceBytesInFile(sizeIndex[block], BitConverter.GetBytes(blocksSizeList[block]));
            for (int i = 0; i < blocksStartList.Count; i++)
            {
                if (blocksStartList[i] > start)
                {
                    blocksStartList[i] = blocksStartList[i] - length;
                }
            }
        }

        void replaceBytesInFile(int start, byte[] array)
        {
            bytesFile[start] = array[0];
            bytesFile[start + 1] = array[1];
            bytesFile[start + 2] = array[2];
            bytesFile[start + 3] = array[3];
            fileChanged = true;
        }

        private byte[] materialBytes(string line)
        {
            byte[] bytesArray = new byte[4];
            if (line == "SKY_HAV_MAT_BROKEN_STONE")
            {
                bytesArray[0] = 71;
                bytesArray[1] = 55;
                bytesArray[2] = 209;
                bytesArray[3] = 7;
            }
            else if (line == "SKY_HAV_MAT_LIGHT_WOOD")
            {
                bytesArray[0] = 227;
                bytesArray[1] = 222;
                bytesArray[2] = 199;
                bytesArray[3] = 21;
            }
            else if (line == "SKY_HAV_MAT_SNOW")
            {
                bytesArray[0] = 175;
                bytesArray[1] = 122;
                bytesArray[2] = 199;
                bytesArray[3] = 23;
            }
            else if (line == "SKY_HAV_MAT_GRAVEL")
            {
                bytesArray[0] = 88;
                bytesArray[1] = 186;
                bytesArray[2] = 139;
                bytesArray[3] = 25;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_CHAIN_METAL")
            {
                bytesArray[0] = 228;
                bytesArray[1] = 68;
                bytesArray[2] = 41;
                bytesArray[3] = 26;
            }
            else if (line == "SKY_HAV_MAT_BOTTLE")
            {
                bytesArray[0] = 246;
                bytesArray[1] = 8;
                bytesArray[2] = 107;
                bytesArray[3] = 29;
            }
            else if (line == "SKY_HAV_MAT_WOOD")
            {
                bytesArray[0] = 17;
                bytesArray[1] = 198;
                bytesArray[2] = 217;
                bytesArray[3] = 29;
            }
            else if (line == "SKY_HAV_MAT_SKIN")
            {
                bytesArray[0] = 2;
                bytesArray[1] = 183;
                bytesArray[2] = 61;
                bytesArray[3] = 35;
            }
            else if (line == "SKY_HAV_MAT_BARREL")
            {
                bytesArray[0] = 20;
                bytesArray[1] = 150;
                bytesArray[2] = 163;
                bytesArray[3] = 43;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_CERAMIC_MEDIUM")
            {
                bytesArray[0] = 91;
                bytesArray[1] = 51;
                bytesArray[2] = 151;
                bytesArray[3] = 46;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_BASKET")
            {
                bytesArray[0] = 110;
                bytesArray[1] = 105;
                bytesArray[2] = 34;
                bytesArray[3] = 47;
            }
            else if (line == "SKY_HAV_MAT_ICE")
            {
                bytesArray[0] = 28;
                bytesArray[1] = 93;
                bytesArray[2] = 14;
                bytesArray[3] = 52;
            }
            else if (line == "SKY_HAV_MAT_STAIRS_STONE")
            {
                bytesArray[0] = 61;
                bytesArray[1] = 115;
                bytesArray[2] = 157;
                bytesArray[3] = 53;
            }
            else if (line == "SKY_HAV_MAT_WATER")
            {
                bytesArray[0] = 199;
                bytesArray[1] = 227;
                bytesArray[2] = 17;
                bytesArray[3] = 61;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_BLADE_1HAND")
            {
                bytesArray[0] = 164;
                bytesArray[1] = 224;
                bytesArray[2] = 48;
                bytesArray[3] = 63;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_BOOK")
            {
                bytesArray[0] = 82;
                bytesArray[1] = 96;
                bytesArray[2] = 97;
                bytesArray[3] = 75;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_CARPET")
            {
                bytesArray[0] = 63;
                bytesArray[1] = 145;
                bytesArray[2] = 177;
                bytesArray[3] = 76;
            }
            else if (line == "SKY_HAV_MAT_SOLID_METAL")
            {
                bytesArray[0] = 59;
                bytesArray[1] = 204;
                bytesArray[2] = 202;
                bytesArray[3] = 76;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_AXE_1HAND")
            {
                bytesArray[0] = 203;
                bytesArray[1] = 2;
                bytesArray[2] = 211;
                bytesArray[3] = 77;
            }
            else if (line == "SKY_HAV_MAT_STAIRS_WOOD")
            {
                bytesArray[0] = 149;
                bytesArray[1] = 245;
                bytesArray[2] = 31;
                bytesArray[3] = 87;
            }
            else if (line == "SKY_HAV_MAT_MUD")
            {
                bytesArray[0] = 129;
                bytesArray[1] = 112;
                bytesArray[2] = 152;
                bytesArray[3] = 88;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_BOULDER_SMALL")
            {
                bytesArray[0] = 214;
                bytesArray[1] = 13;
                bytesArray[2] = 113;
                bytesArray[3] = 92;
            }
            else if (line == "SKY_HAV_MAT_STAIRS_SNOW")
            {
                bytesArray[0] = 43;
                bytesArray[1] = 73;
                bytesArray[2] = 1;
                bytesArray[3] = 93;
            }
            else if (line == "SKY_HAV_MAT_HEAVY_STONE")
            {
                bytesArray[0] = 64;
                bytesArray[1] = 215;
                bytesArray[2] = 160;
                bytesArray[3] = 93;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_BOWS_STAVES")
            {
                bytesArray[0] = 65;
                bytesArray[1] = 214;
                bytesArray[2] = 202;
                bytesArray[3] = 95;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_WOOD_AS_STAIRS")
            {
                bytesArray[0] = 12;
                bytesArray[1] = 80;
                bytesArray[2] = 128;
                bytesArray[3] = 107;
            }
            else if (line == "SKY_HAV_MAT_GRASS")
            {
                bytesArray[0] = 238;
                bytesArray[1] = 104;
                bytesArray[2] = 47;
                bytesArray[3] = 110;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_BOULDER_LARGE")
            {
                bytesArray[0] = 123;
                bytesArray[1] = 206;
                bytesArray[2] = 95;
                bytesArray[3] = 112;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_STONE_AS_STAIRS")
            {
                bytesArray[0] = 127;
                bytesArray[1] = 69;
                bytesArray[2] = 107;
                bytesArray[3] = 112;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_BLADE_2HAND")
            {
                bytesArray[0] = 116;
                bytesArray[1] = 154;
                bytesArray[2] = 144;
                bytesArray[3] = 120;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_BOTTLE_SMALL")
            {
                bytesArray[0] = 88;
                bytesArray[1] = 44;
                bytesArray[2] = 191;
                bytesArray[3] = 120;
            }
            else if (line == "SKY_HAV_MAT_SAND")
            {
                bytesArray[0] = 13;
                bytesArray[1] = 77;
                bytesArray[2] = 62;
                bytesArray[3] = 129;
            }
            else if (line == "SKY_HAV_MAT_HEAVY_METAL")
            {
                bytesArray[0] = 163;
                bytesArray[1] = 38;
                bytesArray[2] = 226;
                bytesArray[3] = 132;
            }
            else if (line == "SKY_HAV_MAT_DRAGON")
            {
                bytesArray[0] = 23;
                bytesArray[1] = 136;
                bytesArray[2] = 26;
                bytesArray[3] = 150;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_BLADE_1HAND_SMALL")
            {
                bytesArray[0] = 204;
                bytesArray[1] = 170;
                bytesArray[2] = 10;
                bytesArray[3] = 156;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_SKIN_SMALL")
            {
                bytesArray[0] = 62;
                bytesArray[1] = 189;
                bytesArray[2] = 230;
                bytesArray[3] = 156;
            }
            else if (line == "SKY_HAV_MAT_STAIRS_BROKEN_STONE")
            {
                bytesArray[0] = 91;
                bytesArray[1] = 105;
                bytesArray[2] = 102;
                bytesArray[3] = 172;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_SKIN_LARGE")
            {
                bytesArray[0] = 147;
                bytesArray[1] = 126;
                bytesArray[2] = 200;
                bytesArray[3] = 176;
            }
            else if (line == "SKY_HAV_MAT_ORGANIC")
            {
                bytesArray[0] = 219;
                bytesArray[1] = 173;
                bytesArray[2] = 81;
                bytesArray[3] = 177;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_BONE")
            {
                bytesArray[0] = 20;
                bytesArray[1] = 124;
                bytesArray[2] = 194;
                bytesArray[3] = 181;
            }
            else if (line == "SKY_HAV_MAT_HEAVY_WOOD")
            {
                bytesArray[0] = 71;
                bytesArray[1] = 112;
                bytesArray[2] = 8;
                bytesArray[3] = 183;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_CHAIN")
            {
                bytesArray[0] = 102;
                bytesArray[1] = 67;
                bytesArray[2] = 59;
                bytesArray[3] = 183;
            }
            else if (line == "SKY_HAV_MAT_DIRT")
            {
                bytesArray[0] = 170;
                bytesArray[1] = 62;
                bytesArray[2] = 35;
                bytesArray[3] = 185;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_ARMOR_LIGHT")
            {
                bytesArray[0] = 157;
                bytesArray[1] = 22;
                bytesArray[2] = 33;
                bytesArray[3] = 204;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_SHIELD_LIGHT")
            {
                bytesArray[0] = 248;
                bytesArray[1] = 211;
                bytesArray[2] = 134;
                bytesArray[3] = 205;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_COIN")
            {
                bytesArray[0] = 62;
                bytesArray[1] = 84;
                bytesArray[2] = 237;
                bytesArray[3] = 213;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_SHIELD_HEAVY")
            {
                bytesArray[0] = 80;
                bytesArray[1] = 251;
                bytesArray[2] = 173;
                bytesArray[3] = 220;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_ARMOR_HEAVY")
            {
                bytesArray[0] = 53;
                bytesArray[1] = 48;
                bytesArray[2] = 10;
                bytesArray[3] = 221;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_ARROW")
            {
                bytesArray[0] = 146;
                bytesArray[1] = 181;
                bytesArray[2] = 14;
                bytesArray[3] = 222;
            }
            else if (line == "SKY_HAV_MAT_GLASS")
            {
                bytesArray[0] = 66;
                bytesArray[1] = 72;
                bytesArray[2] = 233;
                bytesArray[3] = 222;
            }
            else if (line == "SKY_HAV_MAT_STONE")
            {
                bytesArray[0] = 55;
                bytesArray[1] = 242;
                bytesArray[2] = 2;
                bytesArray[3] = 223;
            }
            else if (line == "SKY_HAV_MAT_CLOTH")
            {
                bytesArray[0] = 163;
                bytesArray[1] = 156;
                bytesArray[2] = 211;
                bytesArray[3] = 228;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_BLUNT_2HAND")
            {
                bytesArray[0] = 213;
                bytesArray[1] = 43;
                bytesArray[2] = 155;
                bytesArray[3] = 236;
            }
            else if (line == "SKY_HAV_MAT_MATERIAL_BOULDER_MEDIUM")
            {
                bytesArray[0] = 226;
                bytesArray[1] = 168;
                bytesArray[2] = 86;
                bytesArray[3] = 255;
            }
            else
            {
                outLog.Add("WARNING! WAS REPLACED WRONG MATERIAL COLLISION: " + fileName);
            }
            return bytesArray;
        }

        void checkCollisions(int jump, long coll)
        {
            string mat = null;
            if (coll == 131151687)
            {
                mat = "SKY_HAV_MAT_BROKEN_STONE";
            }
            else if (coll == 365420259)
            {
                mat = "SKY_HAV_MAT_LIGHT_WOOD";
            }
            else if (coll == 398949039)
            {
                mat = "SKY_HAV_MAT_SNOW";
            }
            else if (coll == 428587608)
            {
                mat = "SKY_HAV_MAT_GRAVEL";
            }
            else if (coll == 438912228)
            {
                mat = "SKY_HAV_MAT_MATERIAL_CHAIN_METAL";
            }
            else if (coll == 493553910)
            {
                mat = "SKY_HAV_MAT_BOTTLE";
            }
            else if (coll == 500811281)
            {
                mat = "SKY_HAV_MAT_WOOD";
            }
            else if (coll == 591247106)
            {
                mat = "SKY_HAV_MAT_SKIN";
            }
            else if (coll == 732141076)
            {
                mat = "SKY_HAV_MAT_BARREL";
            }
            else if (coll == 781661019)
            {
                mat = "SKY_HAV_MAT_MATERIAL_CERAMIC_MEDIUM";
            }
            else if (coll == 790784366)
            {
                mat = "SKY_HAV_MAT_MATERIAL_BASKET";
            }
            else if (coll == 873356572)
            {
                mat = "SKY_HAV_MAT_ICE";
            }
            else if (coll == 899511101)
            {
                mat = "SKY_HAV_MAT_STAIRS_STONE";
            }
            else if (coll == 1024582599)
            {
                mat = "SKY_HAV_MAT_WATER";
            }
            else if (coll == 1060167844)
            {
                mat = "SKY_HAV_MAT_MATERIAL_BLADE_1HAND";
            }
            else if (coll == 1264672850)
            {
                mat = "SKY_HAV_MAT_MATERIAL_BOOK";
            }
            else if (coll == 1286705471)
            {
                mat = "SKY_HAV_MAT_MATERIAL_CARPET";
            }
            else if (coll == 1288358971)
            {
                mat = "SKY_HAV_MAT_SOLID_METAL";
            }
            else if (coll == 1305674443)
            {
                mat = "SKY_HAV_MAT_MATERIAL_AXE_1HAND";
            }
            else if (coll == 1461712277)
            {
                mat = "SKY_HAV_MAT_STAIRS_WOOD";
            }
            else if (coll == 1486385281)
            {
                mat = "SKY_HAV_MAT_MUD";
            }
            else if (coll == 1550912982)
            {
                mat = "SKY_HAV_MAT_MATERIAL_BOULDER_SMALL";
            }
            else if (coll == 1560365355)
            {
                mat = "SKY_HAV_MAT_STAIRS_SNOW";
            }
            else if (coll == 1570821952)
            {
                mat = "SKY_HAV_MAT_HEAVY_STONE";
            }
            else if (coll == 1607128641)
            {
                mat = "SKY_HAV_MAT_MATERIAL_BOWS_STAVES";
            }
            else if (coll == 1803571212)
            {
                mat = "SKY_HAV_MAT_MATERIAL_WOOD_AS_STAIRS";
            }
            else if (coll == 1848600814)
            {
                mat = "SKY_HAV_MAT_GRASS";
            }
            else if (coll == 1885326971)
            {
                mat = "SKY_HAV_MAT_MATERIAL_BOULDER_LARGE";
            }
            else if (coll == 1886078335)
            {
                mat = "SKY_HAV_MAT_MATERIAL_STONE_AS_STAIRS";
            }
            else if (coll == 2022742644)
            {
                mat = "SKY_HAV_MAT_MATERIAL_BLADE_2HAND";
            }
            else if (coll == 2025794648)
            {
                mat = "SKY_HAV_MAT_MATERIAL_BOTTLE_SMALL";
            }
            else if (coll == 2168343821)
            {
                mat = "SKY_HAV_MAT_SAND";
            }
            else if (coll == 2229413539)
            {
                mat = "SKY_HAV_MAT_HEAVY_METAL";
            }
            else if (coll == 2518321175)
            {
                mat = "SKY_HAV_MAT_DRAGON";
            }
            else if (coll == 2617944780)
            {
                mat = "SKY_HAV_MAT_MATERIAL_BLADE_1HAND_SMALL";
            }
            else if (coll == 2632367422)
            {
                mat = "SKY_HAV_MAT_MATERIAL_SKIN_SMALL";
            }
            else if (coll == 2892392795)
            {
                mat = "SKY_HAV_MAT_STAIRS_BROKEN_STONE";
            }
            else if (coll == 2965929619)
            {
                mat = "SKY_HAV_MAT_MATERIAL_SKIN_LARGE";
            }
            else if (coll == 2974920155)
            {
                mat = "SKY_HAV_MAT_ORGANIC";
            }
            else if (coll == 3049421844)
            {
                mat = "SKY_HAV_MAT_MATERIAL_BONE";
            }
            else if (coll == 3070783559)
            {
                mat = "SKY_HAV_MAT_HEAVY_WOOD";
            }
            else if (coll == 3074114406)
            {
                mat = "SKY_HAV_MAT_MATERIAL_CHAIN";
            }
            else if (coll == 3106094762)
            {
                mat = "SKY_HAV_MAT_DIRT";
            }
            else if (coll == 3424720541)
            {
                mat = "SKY_HAV_MAT_MATERIAL_ARMOR_LIGHT";
            }
            else if (coll == 3448165368)
            {
                mat = "SKY_HAV_MAT_MATERIAL_SHIELD_LIGHT";
            }
            else if (coll == 3589100606)
            {
                mat = "SKY_HAV_MAT_MATERIAL_COIN";
            }
            else if (coll == 3702389584)
            {
                mat = "SKY_HAV_MAT_MATERIAL_SHIELD_HEAVY";
            }
            else if (coll == 3708432437)
            {
                mat = "SKY_HAV_MAT_MATERIAL_ARMOR_HEAVY";
            }
            else if (coll == 3725505938)
            {
                mat = "SKY_HAV_MAT_MATERIAL_ARROW";
            }
            else if (coll == 3739830338)
            {
                mat = "SKY_HAV_MAT_GLASS";
            }
            else if (coll == 3741512247)
            {
                mat = "SKY_HAV_MAT_STONE";
            }
            else if (coll == 3839073443)
            {
                mat = "SKY_HAV_MAT_CLOTH";
            }
            else if (coll == 3969592277)
            {
                mat = "SKY_HAV_MAT_MATERIAL_BLUNT_2HAND";
            }
            else if (coll == 4283869410)
            {
                mat = "SKY_HAV_MAT_MATERIAL_BOULDER_MEDIUM";
            }
            if (checkBox2.Checked)
            {
                if (mat != null)
                {
                    outLog.Add("HAVE MATERIAL: " + mat);
                }
                else
                {
                    outLog.Add("WARNING! WRONG MATERIAL COLLISION: " + fileName);
                }
            }
            if (checkBox5.Checked && mat == textBox1.Text && textBox2.Text.Length > 12)
            {
                replaceBytesInFile(jump, materialBytes(textBox2.Text));
            }
        }

        void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            debugMode = !debugMode;
        }

        void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox5.Checked;
            textBox2.Enabled = checkBox5.Checked;
        }

        void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                checkBox7.Checked = false;
            }
        }

        void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                checkBox6.Checked = false;
            }
        }

        void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                checkBox9.Checked = false;
            }
        }

        void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked)
            {
                checkBox8.Checked = false;
            }
        }

        void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                fixesOn = true;
                vcColor = false;
                vcAlpha = false;
                buttonColor(5, vcRemove);
            }
            else
            {
                fixesOn = false;
            }
        }

        void button3_Click(object sender, EventArgs e)
        {
            var form = new FormParallax();
            form.ShowDialog();
            form = null;
        }

        void button4_Click(object sender, EventArgs e)
        {
            var form = new FormEC();
            form.ShowDialog();
            form = null;
        }

        void button5_Click(object sender, EventArgs e)
        {
            var form = new FormVC();
            form.ShowDialog();
            form = null;
        }

        void button6_Click(object sender, EventArgs e)
        {
            var form = new FormVer();
            form.ShowDialog();
            form = null;
        }

        void button7_Click(object sender, EventArgs e)
        {
            var form = new FormSpec();
            form.ShowDialog();
            form = null;
        }

        public void buttonColor(int button, bool edit)
        {
            setColor(button == 3 ? button3 : button == 4 ? button4 : button == 5 ? button5 : button == 6 ? button6 : button7, edit);
        }

        void setColor(Button button, bool edit)
        {
            if (edit)
            {
                button.ForeColor = System.Drawing.Color.DarkRed;
            }
            else
            {
                button.ForeColor = System.Drawing.SystemColors.ControlText;
            }
        }
    }
}
