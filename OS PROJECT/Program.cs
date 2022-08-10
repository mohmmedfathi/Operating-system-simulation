using System;
using System.IO;
using System.Text;
namespace OS_PROJECT_LAST
{
    class Program
    {
        public static Directory1 currentDirectory;
        public static string currentPath;
        static void Main(string[] args)
        {
            VirtualDisk.Initalize(@"C:\Users\Dream\Desktop\Projects os\OS1\OS PROJECT LAST\bin\Debug\netcoreapp3.1\container.txt");
            currentPath = new string(currentDirectory.Name) + " ";
            while (true)
            {
                Console.Write(currentPath.Trim());
                string Enter = Console.ReadLine();
                if (!Enter.Contains(" "))
                {
                    if (Enter.ToLower() == "help")
                    {
                        Cmd.Help();
                    }
                    else if (Enter.ToLower() == "quit")
                    {
                        Cmd.Quit();
                    }
                    else if (Enter.ToLower() == "cls")
                    {
                        Cmd.Cls();
                    }
                    else if (Enter.ToLower() == "md")
                    {
                        Console.WriteLine("The syntax of the command is incorrect.");
                    }
                    
                    else if (Enter.ToLower() == "rd")
                    {
                        Console.WriteLine("The syntax of the command is incorrect.");
                    }
                    else if (Enter.ToLower() == "dir")
                    {
                        Cmd.Dir();
                    }//////////////////
                    else if (Enter.ToLower() == "cd")
                    {
                        Cmd.Cd();
                    }
                    else if (Enter.ToLower() == "import")
                    {
                        Console.WriteLine("IMPORT takes Two arguments");
                    }
                    else if (Enter.ToLower() == "type")
                    {
                        Console.WriteLine("TYPE takes one argument");
                    }
                    else if (Enter.ToLower() == "export")
                    {
                        Console.WriteLine("EXPORT takes two arguments");
                    }
                    else if (Enter.ToLower() == "rename")
                    {
                        Console.WriteLine("RENAME takes two arguments");
                    }
                    else if (Enter.ToLower() == "del")
                    {
                        Console.WriteLine("DEL takes one argument");
                    }
                    else if (Enter.ToLower() == "cp")
                    {
                        Console.WriteLine("CP takes two argument");
                    }
                    else
                    {
                        Console.WriteLine("No command with this syntax.");
                        Console.WriteLine("Write (help) to see all commands ");
                    }
                }
                else if (Enter.Contains(" "))
                {
                    string[] EnterSplit = Enter.Split(" ");
                    if (EnterSplit[0].ToLower() == "md")
                    {
                        Cmd.Md(EnterSplit[1]);
                    }
                    else if (EnterSplit[0].ToLower() == "rd")
                    {
                        Cmd.Rd(EnterSplit[1]);
                    }
                    else if (EnterSplit[0].ToLower() == "cd")
                    {
                        Cmd.Cd(EnterSplit[1]);
                    }
                    
                    else if (EnterSplit[0].ToLower() == "dir")
                    {
                        Cmd.Dir();
                    }
                    else if (EnterSplit[0].ToLower() == "quit")
                    {
                        Cmd.Quit();
                    }
                    else if (EnterSplit[0].ToLower() == "import")
                    {
                        Cmd.Import(EnterSplit[1]);
                    }
                    else if (EnterSplit[0].ToLower() == "type")
                    {
                        Cmd.Type(EnterSplit[1]);
                    }
                    
                    else if (EnterSplit[0].ToLower() == "export")
                    {
                        if (EnterSplit.Length >= 2)
                        {
                            Cmd.export(EnterSplit[1], EnterSplit[2]);
                        }
                        else Console.WriteLine("EXPORT takes two arguments");
                    }
                    else if (EnterSplit[0].ToLower() == "rename")
                    {
                        if (EnterSplit.Length > 2)
                        {
                            if (EnterSplit[1] != " " && EnterSplit[2] != " ") { Cmd.rename(EnterSplit[1], EnterSplit[2]); }

                            //else
                            //{
                            //    Console.WriteLine("Enstersecond argument");
                            //    string second = Console.ReadLine();
                            //    Cmd.rename(EnterSplit[1], second);

                            //}
                        }



                        //else Console.WriteLine("Copy takes two arguments");
                        //{
                        //    Console.WriteLine("Enstersecond argument");
                        //    string second = Console.ReadLine();
                        //    Cmd.rename(EnterSplit[1], second);
                        //}
                    }
                    //else if (EnterSplit[0].ToLower() == "cut")
                    //{
                    //    if (EnterSplit.Length > 2)
                    //    {
                    //        if (EnterSplit[1] != " " && EnterSplit[2] != " ") { Cmd.rename(EnterSplit[1], EnterSplit[2]); }

                    //        else
                    //        {
                    //            Console.WriteLine("Enstersecond argument");
                    //            string second = Console.ReadLine();
                    //            //Cmd.cut(EnterSplit[1], second);

                    //        }
                    //    }
                    //    else Console.WriteLine("Copy takes two arguments");
                    //    {
                    //        Console.WriteLine("Enstersecond argument");
                    //        string second = Console.ReadLine();
                    //        Cmd.rename(EnterSplit[1], second);
                    //    }
                    //}
                    else if (EnterSplit[0].ToLower() == "del")
                    {
                        Cmd.del(EnterSplit[1]);
                    }
                    else if (EnterSplit[0].ToLower() == "cp")
                    {
                        if (EnterSplit.Length >= 2)
                        {
                            Cmd.cp(EnterSplit[1], EnterSplit[2]);
                        }
                        else
                            Console.WriteLine("CP takes two argument");
                    }
                    else if (EnterSplit[0].ToLower() == "help")
                    {
                        if (EnterSplit.Length > 2)
                        {
                            Console.WriteLine("Error: " + EnterSplit[0] + " command syntax is \n help \n or \n help [command] \n function:Provides Help information for commands.");
                        }
                        else if (EnterSplit.Length == 2)
                        {
                            Cmd.Help(EnterSplit[1]);

                        }
                        else
                        {
                            Console.WriteLine("Error This command isn't supported.");
                        }

                    }
                }

            }

        }
    }
}