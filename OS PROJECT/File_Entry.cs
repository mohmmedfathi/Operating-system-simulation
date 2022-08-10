using System;
using System.Collections.Generic;
using System.Text;
namespace OS_PROJECT_LAST
{
    public class File_Entry : Directory_Entry
    {
        public string content;
        public Directory1 parent;
        public File_Entry(string name, byte dir_attr, int dir_firstCluster, int dir_filesize, Directory1 pa, string content) : base(name, dir_attr, dir_firstCluster, dir_filesize)
        {
            this.content = content;
            if (pa != null)
                parent = pa;
        }
        public Directory_Entry GetDirectory_Entry()
        {
            Directory_Entry me = new Directory_Entry(new string(this.Name), this.fileAttribute, this.FileFirstCluster, this.FileSize);
            return me;
        }
        public void writeFileContent()
        {
            byte[] contentBYTES = StringToBytes(content);
            List<byte[]> bytesls = FatTable.splitBytes(contentBYTES);
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
        }
        public void ReadFileContent()
        {
            if (this.FileFirstCluster != 0)
            {
                content = string.Empty;
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
                content = BytesToString(ls.ToArray());
            }
        }
        public void DeleteFile()
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
                    FatTable.WriteFatTable();
                }
            }
        }
        public static byte[] StringToBytes(string s)
        {
            byte[] bytes = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                bytes[i] = (byte)s[i];
            }
            return bytes;
        }
        public static string BytesToString(byte[] bytes)
        {
            string s = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                if ((char)bytes[i] != '\0')
                    s += (char)bytes[i];
                else
                    break;
            }
            return s;
        }
    }
}
