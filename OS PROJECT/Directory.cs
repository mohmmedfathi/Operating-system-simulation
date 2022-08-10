using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OS_PROJECT_LAST
{
    public class Directory1 : Directory_Entry
    {
        public List<Directory_Entry> DirectoryTable;
        public Directory1 parent;
        public Directory1(string name, byte fileAttribute, int dir_firstCluster, int FileSize, Directory1 pa) : base(name, fileAttribute, dir_firstCluster, FileSize)
        {
            DirectoryTable = new List<Directory_Entry>();
            if (pa != null)
            {
                parent = pa;
            }

        }
        public void Update(Directory_Entry d)
        {
            int index = SearchDirectory(new string(d.Name));
            if (index != -1)
            {
                DirectoryTable.RemoveAt(index);
                DirectoryTable.Insert(index, d);
            }
        }
        public Directory_Entry GetDirectory_Entry()
        {
            Directory_Entry me = new Directory_Entry(new string(this.Name), this.fileAttribute, this.FileFirstCluster, FileSize);
            return me;
        }
        public void WriteDirectory()
        {
            byte[] dirsorfilesBYTES = new byte[DirectoryTable.Count * 32];
            for (int i = 0; i < DirectoryTable.Count; i++)
            {
                byte[] b = Directory_Entry.Getbyte(this.DirectoryTable[i]);
                for (int j = i * 32, k = 0; k < b.Length; k++, j++)
                    dirsorfilesBYTES[j] = b[k];
            }
            List<byte[]> bytesls = FatTable.splitBytes(dirsorfilesBYTES);
            int clusterFATIndex;
            if (this.FileFirstCluster != 0)
            {
                clusterFATIndex = this.FileFirstCluster;
            }
            else
            {
                clusterFATIndex = FatTable.Getavaliableblock();
                this.FileFirstCluster = clusterFATIndex;
            }
            int lastCluster = -1;
            for (int i = 0; i < bytesls.Count; i++)
            {
                if (clusterFATIndex != -1)
                {
                    VirtualDisk.WriteBlock(bytesls[i], clusterFATIndex, 0, bytesls[i].Length);
                    FatTable.setnext(clusterFATIndex, -1);
                    if (lastCluster != -1)
                        FatTable.setnext(lastCluster, clusterFATIndex);
                    lastCluster = clusterFATIndex;
                    clusterFATIndex = FatTable.Getavaliableblock();
                }
            }
            if (this.parent != null)
            {
                this.parent.Update(this.GetDirectory_Entry());
                this.parent.WriteDirectory();
            }
            FatTable.WriteFatTable();
        }
        public int SearchDirectory(string name)
        {
            if (name.Length < 11)
            {
                name += "\0";
                for (int i = name.Length + 1; i < 12; i++)
                    name += " ";
            }
            else
            {
                name = name.Substring(0, 11);
            }
            for (int i = 0; i < DirectoryTable.Count; i++)
            {
                string n = new string(DirectoryTable[i].Name);
                if (n.Equals(name))
                    return i;
            }
            return -1;
        }
        public void ReadDirectory()
        {
            if (this.FileFirstCluster != 0)
            {
                DirectoryTable = new List<Directory_Entry>();
                int cluster = this.FileFirstCluster;
                int next = FatTable.getnext(cluster);
                List<byte> ls = new List<byte>();
                do
                {
                    ls.AddRange(VirtualDisk.ReadBlock(cluster));
                    cluster = next;
                    if (cluster != -1)
                        next = FatTable.getnext(cluster);
                }
                while (next != -1);
                for (int i = 0; i < ls.Count; i++)
                {
                    byte[] b = new byte[32];
                    for (int k = i * 32, m = 0; m < b.Length && k < ls.Count; m++, k++)
                    {
                        b[m] = ls[k];
                    }
                    if (b[0] == 0)
                        break;
                    DirectoryTable.Add(Directory_Entry.Getdirectoryentry(b));
                }
            }
        }
        public void DeleteDirectory()
        {
            if (this.FileFirstCluster != 0)
            {
                int cluster = this.FileFirstCluster;
                int next = FatTable.getnext(cluster);
                do
                {
                    FatTable.setnext(cluster, 0);
                    cluster = next;
                    if (cluster != -1)
                        next = FatTable.getnext(cluster);
                }
                while (cluster != -1);
            }
            if (this.parent != null)
            {
                int index = this.parent.SearchDirectory(new string(this.Name));
                if (index != -1)
                {
                    this.parent.DirectoryTable.RemoveAt(index);
                    this.parent.WriteDirectory();
                }
            }
            if (Program.currentDirectory == this)
            {
                if (this.parent != null)
                {
                    Program.currentDirectory = this.parent;
                    Program.currentPath = Program.currentPath.Substring(0, Program.currentPath.LastIndexOf('\\'));
                    Program.currentDirectory.ReadDirectory();
                }
            }
            FatTable.WriteFatTable();
        }



    }
}
