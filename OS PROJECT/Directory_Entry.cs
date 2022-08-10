using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OS_PROJECT_LAST
{
    public class Directory_Entry
    {
        public char[] Name = new char[11];
        public byte fileAttribute;
        public byte[] dir_empty = new byte[12];
        public int FileFirstCluster;
        public int FileSize;
        public Directory_Entry(string name, byte fileAttribute, int FileFirstCluster, int FileSize)
        {
            this.fileAttribute = fileAttribute;
            if (fileAttribute == 0x0)
            {
                AssignFileName(name);
            }
            else if (fileAttribute == 0x10)
            {

                AssignDirectoryName(name.ToCharArray());
            }
            this.FileFirstCluster = FileFirstCluster;
            this.FileSize = FileSize;
        }
        public void AssignFileName(string name)
        {
            if (name.Length <= 11)
            {
                int j = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    j++;
                    this.Name[i] = name[i];
                }
                for (int i = ++j; i < Name.Length; i++)
                {
                    this.Name[i] = ' ';
                }
            }
            else
            {
                int j = 0;
                for (int i = 0; i < 7; i++)
                {
                    j++;
                    this.Name[i] = name[i];
                }
                for (int w = 0; w < 4; w++)
                {
                    if (w == 0)
                    {
                        this.Name[j] = '.';
                        j++;
                    }
                    else if (w == 1)
                    {
                        this.Name[j] = 't';
                        j++;
                    }
                    else if (w == 2)
                    {
                        this.Name[j] = 'x';
                        j++;
                    }
                    else if (w == 3)
                    {
                        this.Name[j] = 't';

                    }
                }
            }
        }
        public void AssignDirectoryName(char[] name)
        {
            if (name.Length <= 11)
            {
                int j = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    j++;
                    this.Name[i] = name[i];
                }
                for (int i = ++j; i < Name.Length; i++)
                {
                    this.Name[i] = ' ';
                }
            }
            else
            {
                int j = 0;
                for (int i = 0; i < 11; i++)
                {
                    j++;
                    this.Name[i] = name[i];
                }
            }
        }

        public static byte[] Getbyte(Directory_Entry d)
        {
            byte[] bytes = new byte[32];
            for (int i = 0; i < d.Name.Length; i++)
            {
                bytes[i] = (byte)d.Name[i];
            }
            bytes[11] = d.fileAttribute;
            int j = 12;
            for (int i = 0; i < d.dir_empty.Length; i++)
            {
                bytes[j] = d.dir_empty[i];
                j++;
            }
            byte[] fc = BitConverter.GetBytes(d.FileFirstCluster);
            for (int i = 0; i < fc.Length; i++)
            {
                bytes[j] = fc[i];
                j++;
            }
            byte[] sz = BitConverter.GetBytes(d.FileSize);
            for (int i = 0; i < sz.Length; i++)
            {
                bytes[j] = sz[i];
                j++;
            }
            return bytes;

        }

        public static Directory_Entry Getdirectoryentry(Byte[] bytes)
        {
            char[] name = new char[11];
            for (int i = 0; i < name.Length; i++)
            {
                name[i] = (char)bytes[i];
            }
            byte attr = bytes[11];
            byte[] empty = new byte[12];
            int j = 12;
            for (int i = 0; i < empty.Length; i++)
            {
                empty[i] = bytes[j];
                j++;
            }
            byte[] fc = new byte[4];
            for (int i = 0; i < fc.Length; i++)
            {
                fc[i] = bytes[j];
                j++;
            }
            int firstcluster = BitConverter.ToInt32(fc, 0);
            byte[] sz = new byte[4];
            for (int i = 0; i < sz.Length; i++)
            {
                sz[i] = bytes[j];
                j++;
            }
            int FileSize = BitConverter.ToInt32(sz, 0);
            Directory_Entry d = new Directory_Entry(new string(name), attr, firstcluster, FileSize);
            d.dir_empty = empty;
            d.FileSize = FileSize;
            return d;

        }

    }
}
