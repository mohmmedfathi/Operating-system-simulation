using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
namespace OS_PROJECT_LAST
{
    class VirtualDisk
    {
        public static FileStream Vir_disk;
        public static void Initalize(string path)
        {
            
                if (!File.Exists(path))
                {
                 Vir_disk = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    byte[] b = new byte[1024];
                    for (int i = 0; i < b.Length; i++)
                        b[i] = 0;
                    WriteBlock(b, 0);
                    FatTable.createFAT();
                    Directory1 root = new Directory1("R:", 0x10, 5, 0, null);
                    root.WriteDirectory();
                    FatTable.setnext(5, -1);
                    Program.currentDirectory = root;
                    FatTable.WriteFatTable();
                }
                else
                {
                    Vir_disk = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    FatTable.readFAT();
                    Directory1 root = new Directory1("R:", 0x10, 5, 0, null);
                    root.ReadDirectory();
                    Program.currentDirectory = root;
                }
            
        }
        public static void WriteBlock(byte[] cluster, int clusterIndex, int offset = 0, int count = 1024)
        {
            Vir_disk.Seek(clusterIndex * 1024, SeekOrigin.Begin);
            Vir_disk.Write(cluster, offset, count);
            Vir_disk.Flush();
        }
        public static byte[] ReadBlock(int clusterIndex)
        {
            Vir_disk.Seek(clusterIndex * 1024, SeekOrigin.Begin);
            byte[] bytes = new byte[1024];
            Vir_disk.Read(bytes, 0, 1024);
            return bytes;
        }
    }
}