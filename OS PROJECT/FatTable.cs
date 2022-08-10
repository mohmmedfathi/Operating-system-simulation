using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
namespace OS_PROJECT_LAST
{
    class FatTable
    {
        public static int[] fat_table = new int[1024];
        public static void createFAT()
        {
            for (int i = 0; i < fat_table.Length; i++)
            {
                if (i == 0 || i == 4)
                {
                    fat_table[i] = -1;
                }
                else if (i > 0 && i < 4)
                {
                    fat_table[i] = i + 1;
                }
                else
                {
                    fat_table[i] = 0;
                }
            }
        }
        public static void WriteFatTable()
        {
            byte[] FATBYTES = ToBytes(FatTable.fat_table);
            List<byte[]> ls = splitBytes(FATBYTES);
            for (int i = 0; i < ls.Count; i++)
            {
                VirtualDisk.WriteBlock(ls[i], i + 1, 0, ls[i].Length);
            }
        }
        public static void readFAT()
        {
            List<byte> ls = new List<byte>();
            for (int i = 1; i <= 4; i++)
            {
                ls.AddRange(VirtualDisk.ReadBlock(i));
            }
            fat_table =ToInt(ls.ToArray());
        }
        public static void printFAT()
        {
            Console.WriteLine("Fat table has the following: ");
            for (int i = 0; i < fat_table.Length; i++)
                Console.WriteLine(i +" : " + fat_table[i]);
        }
        public static int Getavaliableblock()
        {
            for (int i = 0; i < fat_table.Length; i++)
            {
                if (fat_table[i] == 0)
                    return i;
            }
            return 0;
        }
        public static byte[] ToBytes(int[] array)
        {
            byte[] bytes = null;
            bytes = new byte[array.Length * 4];
            System.Buffer.BlockCopy(array, 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public static void setnext(int Index, int value)
        {
            fat_table[Index] = value;
        }
        public static List<byte[]> splitBytes(byte[] bytes)
        {
            List<byte[]> ls = new List<byte[]>();
            int number_of_arrays = bytes.Length / 1024;
            int rem = bytes.Length % 1024;
            for (int i = 0; i < number_of_arrays; i++)
            {
                byte[] b = new byte[1024];
                for (int j = i * 1024, k = 0; k < 1024; j++, k++)
                {
                    b[k] = bytes[j];
                }
                ls.Add(b);
            }
            if (rem > 0)
            {
                byte[] b1 = new byte[1024];
                for (int i = number_of_arrays * 1024, k = 0; k < rem; i++, k++)
                {
                    b1[k] = bytes[i];
                }
                ls.Add(b1);
            }
            return ls;
        }
        public static int getnext(int Index)
        {
                return fat_table[Index];
        }
        public static int[] ToInt(byte[] bytes)
        {
            int[] ints = null;
            ints = new int[bytes.Length / 4];


            System.Buffer.BlockCopy(bytes, 0, ints, 0, bytes.Length);
            return ints;
        }
        public static int GetAvilaibleBlocks()
        {
            int count = 0;
            for (int i = 0; i < fat_table.Length; i++)
            {
                if (fat_table[i] == 0)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
