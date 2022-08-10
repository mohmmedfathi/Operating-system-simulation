using System;
using System.IO;
namespace OS_PROJECT_LAST
{
    class Cmd
    {
        public static void Cls()
        {
            Console.Clear();
        }
        public static void Help()
        {
            Console.WriteLine("cd       - Change the current default directory to .");
            Console.WriteLine("cls      - Clear the screen.");
            Console.WriteLine("dir      - List the contents of directory .");
            Console.WriteLine("quit     - Quit the shell.");
            Console.WriteLine("copy     - Copy files from and to your virual disk");
            Console.WriteLine("del      - Delete files.");
            Console.WriteLine("help     - Provides information for commands.");
            Console.WriteLine("md       - Creates a directory.");
            Console.WriteLine("rd       - Removes a directory.");
            Console.WriteLine("rename   - Renames a file.");
            Console.WriteLine("type     - Displays the contents of a text file.");
            Console.WriteLine("import   – import text file(s) from your computer");
            Console.WriteLine("export   – export text file(s) to your computer");
        }
        public static void Help(string EnterSplit)
        {
            if (EnterSplit.ToLower() == "cd")
            {
                Console.WriteLine("Change the current default directory to the directory given in the argument.");
                Console.WriteLine("If the argument is not present, report the current directory.");
            }
            else if (EnterSplit.ToLower() == "help")
            {

                Console.WriteLine("Provides information for commands.");
            }
            else if (EnterSplit.ToLower() == "cls")
            {

                Console.WriteLine("Clear the screen.");
            }
            else if (EnterSplit.ToLower() == "dir")
            {

                Console.WriteLine("List the contents of directories or Files given in the argument.");
            }
            else if (EnterSplit.ToLower() == "quit")
            {

                Console.WriteLine("Quit the shell.");
            }
            else if (EnterSplit.ToLower() == "cp")
            {

                Console.WriteLine("Copies one or more files to another location.");
            }
            else if (EnterSplit.ToLower() == "del")
            {

                Console.WriteLine("Delete  files.");
            }
            else if (EnterSplit.ToLower() == "md")
            {
                Console.WriteLine("Creates a directory.");
            }
            else if (EnterSplit.ToLower() == "rd")
            {
                Console.WriteLine("Removes a directory.");
            }
            else if (EnterSplit.ToLower() == "rename")
            {
                Console.WriteLine("Renames a file.");
            }
            else if (EnterSplit.ToLower() == "type")
            {

                Console.WriteLine("Displays the contents of a text file.");
            }
            else if (EnterSplit.ToLower() == "import")
            {
                Console.WriteLine("– import text file(s) from your computer ");
            }
            else if (EnterSplit.ToLower() == "export")
            {
                Console.WriteLine("– export text file(s) to your computer ");
            }
            else
            {
                Console.WriteLine("Error This command isn't supported.");
                Console.WriteLine("Write (help) to see all commands ");
            }

        }
        public static void Quit()
        {
                FatTable.WriteFatTable();
                VirtualDisk.Vir_disk.Close();
                Environment.Exit(0);
        }
        public static void Md(string Name)
        {
            int index = Program.currentDirectory.SearchDirectory(Name);
            if (index == -1)
            {
                Directory_Entry newdirectory = new Directory_Entry(Name, 0x10, 0, 0);
                Directory1 name = new Directory1(Name, 0x10, 0, 0, Program.currentDirectory);
                Program.currentDirectory.DirectoryTable.Add(newdirectory);
                Program.currentDirectory.WriteDirectory();
                if (Program.currentDirectory.parent != null)
                {
                    Program.currentDirectory.parent.Update(Program.currentDirectory.parent);
                    Program.currentDirectory.parent.WriteDirectory();
                }
            }
            else
            {
                if (Program.currentDirectory.DirectoryTable[index].fileAttribute == 0x10)
                {
                    Console.WriteLine( Name + " already exists.");
                }
                else
                {
                    Console.WriteLine( Name + " already exists.");
                }
            }


        }
        public static void Rd(string Name)
        {
            int index = Program.currentDirectory.SearchDirectory(Name);
            if (index != -1)
            {
                if (Program.currentDirectory.DirectoryTable[index].fileAttribute == 0x10)
                {
                    int firstCluster = Program.currentDirectory.DirectoryTable[index].FileFirstCluster;
                    int fileSize = Program.currentDirectory.DirectoryTable[index].FileSize;
                    Directory1 d1 = new Directory1(Name, 0x10, firstCluster, fileSize, Program.currentDirectory);
                    d1.DeleteDirectory();
                    Program.currentDirectory.WriteDirectory();
                    
                }
                else 
                { 
                    Console.WriteLine(" using (del) for delete Files");
                }
            }
            else
            {
                Console.WriteLine("This Dir isn't exist.");
            }
        }
        public static void Import(string Path)
        {
            string Name = "";
            if (File.Exists(Path))
            {
                char[] separators = new char[1];
                separators[0] = '\\';
                string[] nam = Path.Split(separators);
                Name = nam[nam.Length - 1];

                string Content = "";
                int index = Program.currentDirectory.SearchDirectory(Name);
                if (index == -1)
                {
                    Content += File.ReadAllText(Path);
                    int size = Content.Length;
                    int firstCluster = 0;
                    if (size > 0)
                    {
                        firstCluster = FatTable.Getavaliableblock();
                    }

                    File_Entry file = new File_Entry(Name, 0x0, firstCluster, size, Program.currentDirectory, Content);
                    file.writeFileContent();
                    Directory_Entry f = new Directory_Entry(Name, 0x0, firstCluster, size);
                    Program.currentDirectory.DirectoryTable.Add(f);

                    Program.currentDirectory.WriteDirectory();

                    if (Program.currentDirectory.parent != null)
                    {
                        Program.currentDirectory.parent.Update(Program.currentDirectory.parent);
                        Program.currentDirectory.parent.WriteDirectory();
                    }


                }
                else
                {
                    if (Program.currentDirectory.DirectoryTable[index].fileAttribute == 0x10)
                    {
                        Console.WriteLine(Name + "  exists as a Directory  ");
                    }
                }
            }
            else if (!File.Exists(Path))
            {
                Console.WriteLine("Path isn't correct");
            }
        }        
        public static void Dir()
        {
            string name = " ";
            int numOfFiles = 0, numOfOFolders = 0, sizeOfFiles = 0;
            Console.WriteLine("\nDirectory of : " + Program.currentPath.Trim()+"\n");

            for (int i = 0; i < Program.currentDirectory.DirectoryTable.Count; i++)
            {
                if (Program.currentDirectory.DirectoryTable[i].fileAttribute == 0x10)
                {
                    numOfOFolders += 1;
                    for (int j = 0; j < Program.currentDirectory.DirectoryTable[i].Name.Length; j++)
                    {
                        name += Program.currentDirectory.DirectoryTable[i].Name[j];
                    }
                    Console.WriteLine("<DIR>\t  " + name);
                    name = " ";
                }
                else
                {
                    numOfFiles += 1;
                    sizeOfFiles += Program.currentDirectory.DirectoryTable[i].FileSize;
                    for (int j = 0; j < Program.currentDirectory.DirectoryTable[i].Name.Length; j++)
                    {
                        name += Program.currentDirectory.DirectoryTable[i].Name[j];
                    }
                    Console.WriteLine("       " + Program.currentDirectory.DirectoryTable[i].FileSize + " " + name);
                    name = " ";
                }
            }
            int FreeSpace = FatTable.Getavaliableblock() * 1024;
            Console.Write("\t    " + numOfFiles + " File(s)\t" +   + sizeOfFiles +"bytes"+ "\n\t    " + numOfOFolders + " Dir(s)"  + "\t" + FreeSpace + "bytes\n");
        }
        public static int export(string source, string dest)
        {
            int name_start = source.LastIndexOf(".");
            string filename = source.Substring(name_start + 1);
            if (filename == "txt")
            {
                int index = Program.currentDirectory.SearchDirectory(source);
                if (index != -1)
                {
                    if (Program.currentDirectory.DirectoryTable[index].fileAttribute == 0x0)
                    {
                        if (Directory.Exists(dest))
                        {
                                int cluster = Program.currentDirectory.DirectoryTable[index].FileFirstCluster;
                                int size = Program.currentDirectory.DirectoryTable[index].FileSize;
                                string content = null;
                                File_Entry file = new File_Entry(source, 0x0, cluster, size, Program.currentDirectory, content);
                                file.ReadFileContent();
                                StreamWriter st = new StreamWriter(dest + "\\" + source);
                                st.Write(file.content);
                                st.Flush();
                                st.Close();
                                return 1;
                        }
                        else
                        {
                            Console.WriteLine("The system can't find this Path");
                            return 0;
                        }
                    }
                    else
                    {
                        Console.WriteLine(source + "  This is Directory");
                        return 0;
                    }
                }
                else
                {
                    Console.WriteLine("This  \"" + source + "\" is not exist");
                    return 0;
                }
            }
            else
            {
                Console.WriteLine("try again with file name");
                return 0;
            }

        }
        public static void rename(string oldName, string newName)
        {
            int oldIndex = Program.currentDirectory.SearchDirectory(oldName);
            if (oldIndex != -1)
            {
                int newIndex = Program.currentDirectory.SearchDirectory(new string(newName));

                if (newIndex == -1)
                {
                    Directory_Entry d1 = new Directory_Entry(newName, Program.currentDirectory.DirectoryTable[oldIndex].fileAttribute, Program.currentDirectory.DirectoryTable[oldIndex].FileFirstCluster, Program.currentDirectory.DirectoryTable[oldIndex].FileSize);
                    Program.currentDirectory.DirectoryTable.RemoveAt(oldIndex);
                    Program.currentDirectory.DirectoryTable.Insert(oldIndex, d1);
                    Program.currentDirectory.WriteDirectory();
                }
                else
                {
                    Console.WriteLine("This Name(" + newName + ") is already exist");
                }
            }
            else
            {
                Console.WriteLine(oldName + " isn't exist");
            }
        }
        public static void del(string fileName)
        {
            int index = Program.currentDirectory.SearchDirectory(fileName);
            if (index != -1)
            {
                if (Program.currentDirectory.DirectoryTable[index].fileAttribute == 0x0)
                {
                    int cluster = Program.currentDirectory.DirectoryTable[index].FileFirstCluster;
                    int size = Program.currentDirectory.DirectoryTable[index].FileSize;
                    File_Entry f = new File_Entry(fileName, 0x0, cluster, size, Program.currentDirectory, null);
                    Program.currentDirectory.DirectoryTable.RemoveAt(index);
                    Program.currentDirectory.WriteDirectory();
                }
                else
                {
                    Console.WriteLine("use (rd) command fot deleting Directory");
                }

            }
            else
            {
                Console.WriteLine("  no file with this name");
            }
        }
        public static void Type(string Name)
        {
            int index = Program.currentDirectory.SearchDirectory(Name);
            if (index != -1)
            {
                if (Program.currentDirectory.DirectoryTable[index].fileAttribute == 0x0)
                {
                    int FirstCluster = Program.currentDirectory.DirectoryTable[index].FileFirstCluster;
                    int FileSize = Program.currentDirectory.DirectoryTable[index].FileSize;
                    string Content = string.Empty;
                    File_Entry file = new File_Entry(Name, 0x0, FirstCluster, FileSize, Program.currentDirectory, Content);
                    file.ReadFileContent();
                    Console.WriteLine(file.content);
                }
                else
                {
                    Console.WriteLine("Write file name  ");
                }
            }
            else
            {
                Console.WriteLine("No file such name");
            }
        }
        public static void Cd()
        {
            Console.WriteLine("Current Path : " + Program.currentPath);

        }
        public static void cp(string Path, string dest)
        {
            string s = Program.currentPath;
            Directory1 d2 = Program.currentDirectory;
            export(Path, "F:");
            Cd(dest);
            string n = "F:\\" + Path;

            Import(n);
            Program.currentDirectory = d2;
            Program.currentPath = s;
        }
        public static void Cd(string Name)
        {
            char[] separators = new char[2];
            separators[0] = '\\';

            string[] nam = Name.Split(separators);
            int index = Program.currentDirectory.SearchDirectory(Name);
            if (nam.Length > 1)
            {
                string s = " ";
                string y = " ";
                if (Program.currentDirectory.parent == null)
                {
                    for (int q = 0; q < nam.Length - 1;)
                    {

                        s = new string(Program.currentDirectory.Name).Trim();
                        y = new string(nam[q]) + "\0";

                        if (y == s)
                        {
                            int inde = Program.currentDirectory.SearchDirectory(nam[q + 1]);
                            if (inde != -1)
                            {


                                if (Program.currentDirectory.DirectoryTable[inde].fileAttribute == 0x10)
                                {
                                    int firstCluster = Program.currentDirectory.DirectoryTable[inde].FileFirstCluster;
                                    int fileSize = Program.currentDirectory.DirectoryTable[inde].FileSize;
                                    Directory1 d1 = new Directory1(nam[q + 1], 0x10, firstCluster, fileSize, Program.currentDirectory);
                                    Program.currentPath = Program.currentPath.Trim();
                                    Program.currentPath += "\\" + new string(d1.Name).Trim();
                                    Program.currentDirectory = d1;
                                    Program.currentDirectory.ReadDirectory();
                                }
                                else
                                {
                                    Console.WriteLine(nam[q + 1] + "is not a Directory");

                                    break;
                                }

                            }
                            else
                            {

                                Console.WriteLine("Path not correct");
                                break;
                            }

                        }
                        q++;
                    }

                }
                else
                {
                    while (true)
                    {
                        if (Program.currentDirectory.parent == null)
                        {
                            break;
                        }
                        Program.currentDirectory = Program.currentDirectory.parent;
                    }
                    Program.currentPath = new string(Program.currentDirectory.Name) + " ";
                    Program.currentDirectory.ReadDirectory();
                    Cd(Name);
                }

                Program.currentPath += " ";
            }
            else if (nam.Length == 1)
            {
                if (Name == "..")
                {
                    if (Program.currentDirectory.parent != null)
                    {
                        int goodPrint = Program.currentPath.LastIndexOf("\\");
                        Program.currentPath = Program.currentPath.Substring(0, goodPrint);
                        Program.currentPath = Program.currentPath + " ";
                        Program.currentDirectory = Program.currentDirectory.parent;
                        Program.currentDirectory.ReadDirectory();
                    }
                    else
                        Console.WriteLine("This is The root Directory " + Program.currentPath);
                }
                if (Name == "~")
                {
                    if (Program.currentDirectory.parent != null)
                    {
                        bool a = true;
                        while (a)
                        {
                            if (Program.currentDirectory.parent == null)
                            {
                                a = false;
                                break;
                            }
                            Program.currentDirectory = Program.currentDirectory.parent;
                        }
                        Program.currentPath = new string(Program.currentDirectory.Name) + " ";
                        Program.currentDirectory.ReadDirectory();
                    }
                    else
                        Console.WriteLine("This is The root Directory " + Program.currentPath);
                }
                else if (index != -1)
                {
                    if (Program.currentDirectory.DirectoryTable[index].fileAttribute == 0x10)
                    {
                        int firstCluster = Program.currentDirectory.DirectoryTable[index].FileFirstCluster;
                        int fileSize = Program.currentDirectory.DirectoryTable[index].FileSize;

                        Directory1 d1 = new Directory1(Name, 0x10, firstCluster, fileSize, Program.currentDirectory);
                        Program.currentPath = Program.currentPath.Trim();
                        Program.currentPath += "\\" + new string(d1.Name).Trim();
                        Program.currentDirectory = d1;
                        Program.currentDirectory.ReadDirectory();
                        Program.currentPath = Program.currentPath + " ";
                    }
                    else { Console.WriteLine("Enter Directory"); }
                }
                //else
                //{
                //    Console.WriteLine("The system cannot find the path specified.");
                //}
            }
        }
    }
}
