using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Pokemon_Shuffle_Unpacker.Properties;

using Ionic.Zip;

namespace Pokemon_Shuffle_Unpacker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = this.RTB_Output.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.RTB_Output.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);
            this.RTB_Output.DragDrop += new DragEventHandler(Form1_DragDrop);
            FileNames = GetDictionary(Resources.File_Names);
            ArchiveNames = GetDictionary(Resources.Archive_Names);
            MessageRegions = GetDictionary(Resources.Message_Regions);
            Rename_Files = CHK_UseKnownNames.Checked;
            Rename_Folders = CHK_RenameArchiveFolders.Checked;
            Delete_Archives = CHK_DeleteArchives.Checked;
        }

        private Dictionary<uint, string> FileNames;
        private Dictionary<uint, string> ArchiveNames;
        private Dictionary<uint, string> MessageRegions;

        private string Selected_Path;
        private bool Rename_Files;
        private bool Rename_Folders;
        private bool Delete_Archives;

        private volatile int threads = 0;

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (threads > 0)
            {
                MessageBox.Show("Please wait until all operations are finished.");
                return;
            }
            new Thread(() =>
            {
                threads++;
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                    Open(file);
                threads--;
            }).Start();
        }

        private void B_Open_Click(object sender, EventArgs e)
        {
            B_Go.Enabled = false;
            CommonDialog dialog;
            if (Control.ModifierKeys == Keys.Alt)
                dialog = new FolderBrowserDialog();
            else
                dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog is OpenFileDialog)
                    TB_FilePath.Text = (dialog as OpenFileDialog).FileName;
                else if (dialog is FolderBrowserDialog)
                    TB_FilePath.Text = (dialog as FolderBrowserDialog).SelectedPath;
                else
                    TB_FilePath.Text = string.Empty;
                B_Go.Enabled = true;
            }
        }

        private void B_Go_Click(object sender, EventArgs e)
        {
            if (threads > 0)
            {
                MessageBox.Show("Please wait until all operations are finished.");
                return;
            }
            new Thread(() =>
            {
                threads++;
                Open(Selected_Path);
                threads--;
            }).Start();
        }

        private void Open(string path)
        {
            if (Directory.Exists(path))
                OpenDir(new DirectoryInfo(path));
            else if (File.Exists(path))
                if (Path.GetFileName(path).Length == 8 && IsHex(Path.GetFileName(path)))
                    ExtractFile(new FileInfo(path));
        }

        private void OpenDir(DirectoryInfo di)
        {
            foreach (FileInfo fi in di.GetFiles())
            {
                Open(fi.FullName);
            }
            foreach (DirectoryInfo d in di.GetDirectories())
                OpenDir(d);
        }

        private void ExtractFile(FileInfo fi)
        {
            uint key = uint.Parse(fi.Name, System.Globalization.NumberStyles.AllowHexSpecifier);
            ShuffleARC Archive = new ShuffleARC(fi.FullName);
            if (Archive.IsValid())
            {
                string OutputDirectory = Path.GetDirectoryName(fi.FullName) + Path.DirectorySeparatorChar;
                if (Rename_Folders)
                {
                    if (ArchiveNames.ContainsKey(key))
                        OutputDirectory += ArchiveNames[key] + string.Format(" ({0})", fi.Name) + Path.DirectorySeparatorChar;
                    else
                    {
                        OutputDirectory += fi.Name + "_" + Path.DirectorySeparatorChar;
                        AddLine(RTB_Output, string.Format("Unable to find a name for {0}. Please contact SciresM.", fi.Name));
                    }
                }
                else
                    OutputDirectory += fi.Name + "_" + Path.DirectorySeparatorChar;
                if (Directory.Exists(OutputDirectory))
                    Directory.Delete(OutputDirectory, true);
                Directory.CreateDirectory(OutputDirectory);
                AddText(RTB_Output, string.Format("Extracting {0} ({1} files)...", fi.Name, Archive.GetFileCount()));
                string diglen = new string(Enumerable.Repeat('0', (int)(Math.Log10(Archive.GetFileCount()) + 1)).ToArray()); // Number of Digits for No-Rename Files.
                using (var fs = File.OpenRead(fi.FullName))
                {
                    for (int i = 0; i < Archive.GetFileCount(); i++)
                    {
                        byte[] ZipBuffer = new byte[Archive.GetFiles()[i].Length];
                        fs.Seek(Archive.GetFiles()[i].Offset, SeekOrigin.Begin);
                        fs.Read(ZipBuffer, 0, ZipBuffer.Length);
                        string ZipName = OutputDirectory + i.ToString(diglen) + ".zip";
                        File.WriteAllBytes(ZipName, ZipBuffer);
                        bool Unknowns = false;
                        using (ZipFile Zip = new ZipFile(ZipName))
                        {
                            if (Zip.ToList().Count == 1)
                            {
                                string FileName = Zip[0].FileName;
                                if (isEmptyString(FileName))
                                {
                                    if (Rename_Files)
                                    {
                                        if (FileNames.ContainsKey(Archive.GetFiles()[i].NameHash))
                                        {
                                            Zip[0].FileName = FileNames[Archive.GetFiles()[i].NameHash];
                                            if (MessageRegions.ContainsKey(key))
                                                Zip[0].FileName = string.Format(Zip[0].FileName, MessageRegions[key]);
                                        }
                                        else
                                        {
                                            AddLine(RTB_Output, string.Empty);
                                            AddLine(RTB_Output, string.Format("Found unknown file at index {0} in {1}. Please contact SciresM.", i.ToString(diglen), fi.Name));
                                            if (!Unknowns)
                                                AddLine(RTB_Output, string.Format("Zip {0} will not be deleted for debugging purposes.", i.ToString(diglen)));
                                            Unknowns = true;
                                            AddText(RTB_Output, "...");
                                        }
                                    }
                                    else
                                        Zip[0].FileName = i.ToString(diglen);
                                }
                                Zip.Save();
                                Zip.ExtractAll(OutputDirectory);
                            }
                        }
                        if (!Unknowns)
                            File.Delete(ZipName);
                    }
                }
                AddLine(RTB_Output, "Complete!");
                if (Delete_Archives)
                {
                    AddLine(RTB_Output, "Deleting " + fi.Name);
                    File.Delete(fi.FullName);
                }
            }
            else
                AddLine(RTB_Output, string.Format("Did not extract invalid archive '{0}'.", fi.Name));

        }

        private void AddText(RichTextBox RTB, string msg)
        {
            if (RTB.InvokeRequired)
                RTB.Invoke(new Action(() => RTB.AppendText(msg)));
            else
                RTB.AppendText(msg);
        }

        private void AddLine(RichTextBox RTB, string line)
        {
            if (RTB.InvokeRequired)
                RTB.Invoke(new Action(() => RTB.AppendText(line + Environment.NewLine)));
            else
                RTB.AppendText(line + Environment.NewLine);
        }

        public static bool IsHex(string str)
        {
            foreach (char c in str.ToCharArray())
                if (!((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
                    return false;
            return true;
        }

        public static bool isEmptyString(string s)
        {
            foreach (char c in s.ToCharArray())
                if (c != (char)0)
                    return false;
            return true;
        }

        private void TB_FilePath_TextChanged(object sender, EventArgs e)
        {
            Selected_Path = TB_FilePath.Text;
        }

        private void CHK_RenameArchiveFolders_CheckedChanged(object sender, EventArgs e)
        {
            Rename_Folders = CHK_RenameArchiveFolders.Checked;
        }

        private void CHK_UseKnownNames_CheckedChanged(object sender, EventArgs e)
        {
            Rename_Files = CHK_UseKnownNames.Checked;
        }

        private void CHK_DeleteArchives_CheckedChanged(object sender, EventArgs e)
        {
            Delete_Archives = CHK_DeleteArchives.Checked;
        }

        private Dictionary<uint, string> GetDictionary(string resourcefile)
        {
            const char tab = '	';
            Dictionary<uint, string> Dict = new Dictionary<uint, string>();
            foreach (string line in resourcefile.Split(Environment.NewLine.ToCharArray()))
            {
                if (line.Contains(tab))
                {
                    string[] vals = line.Split(tab);
                    Dict.Add(uint.Parse(vals[0], System.Globalization.NumberStyles.AllowHexSpecifier), vals[1]);
                }
            }
            return Dict;
        }
    }

    public class ShuffleARC
    {
        private uint Magic; //0xB
        private uint Unknown;
        private uint Unknown2;
        private uint FileCount;
        private uint Padding;
        private List<ShuffleFile> Files;

        private string FileName;
        private string FilePath;
        private string Extension;
        private bool IsExtdata;
        private bool Valid;

        public ShuffleARC(string path)
        {
            if (!File.Exists(path))
            {
                this.Valid = false;
                return;
            }
            this.FileName = Path.GetFileNameWithoutExtension(path);
            if (FileName.Length != 8 || !Form1.IsHex(FileName))
            {
                this.Valid = false;
                return;
            }
            this.FilePath = Path.GetDirectoryName(path);
            this.Extension = Path.GetExtension(path);
            using (BinaryReader br = new BinaryReader(File.OpenRead(path)))
            {
                if (br.ReadUInt32() != 0xB)
                {
                    br.BaseStream.Seek(0x100, SeekOrigin.Begin); // Extdata archives are RSA-signed...and thus data starts after a 0x100 signature.
                    if (br.ReadUInt32() != 0xB)
                    {
                        this.Valid = false;
                        return;
                    }
                    else
                        this.IsExtdata = true;
                }
                this.Magic = br.ReadUInt32();
                if (Magic != uint.Parse(FileName, System.Globalization.NumberStyles.AllowHexSpecifier))
                {
                    this.Valid = false;
                    return;
                }
                this.Valid = true; // Error Checking done.
                this.Unknown = br.ReadUInt32();
                this.Unknown2 = br.ReadUInt32();
                this.FileCount = br.ReadUInt32();
                this.Padding = br.ReadUInt32();
                this.Files = new List<ShuffleFile>();
                for (int i = 0; i < this.FileCount; i++)
                {
                    uint nh = br.ReadUInt32();
                    br.ReadUInt32();
                    uint len = br.ReadUInt32();
                    uint ofs = br.ReadUInt32();
                    if (this.IsExtdata)
                        ofs += 0x100;
                    this.Files.Add(new ShuffleFile() { NameHash = nh, Length = len, Offset = ofs });
                    br.BaseStream.Seek(0x10, SeekOrigin.Current);
                }
            }
        }

        public bool IsValid()
        {
            return this.Valid;
        }

        public uint GetFileCount()
        {
            return this.FileCount;
        }

        public List<ShuffleFile> GetFiles()
        {
            return this.Files;
        }
    }

    public struct ShuffleFile
    {
        public uint Offset;
        public uint Length;
        public uint NameHash;
    }
}
