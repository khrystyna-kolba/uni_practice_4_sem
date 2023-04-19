using task2np.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task2np
{
    public class Task2
    {
        public static void Task_2()
        {
            ContainerCollection containers = new ContainerCollection();
            while (true)
            {
                try
                {
                    Console.WriteLine("enter your choice \n 1 - read from file \n 2 - search in the current collection of containers \n" +
                        " 3 - add new Container to current collection \n 4 - print current collection \n 5 - sort " +
                        "collection by property\n 6 - delete Container from collection by ID \n 7 " +
                        "- edit container by ID \n 8 - write collection to json file \n 9 - exit \n");
                    string choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        Console.WriteLine("Enter file name to read from");
                        string file = Console.ReadLine();
                        containers.ReadJsonFile(file);
                    }
                    else if (choice == "2")
                    {
                        //search
                        Console.WriteLine("Enter value to search: ");
                        string s = Console.ReadLine();
                        Console.WriteLine("FOUND: \n" + containers.Search(s));
                    }
                    else if (choice == "3")
                    {
                        Console.WriteLine("enter container data");
                        Container c = Container.Input();
                        containers.Add(c);
                    }
                    else if (choice == "4")
                    {
                        Console.WriteLine(containers);
                    }
                    else if (choice == "5")
                    {
                        //sort
                        Console.WriteLine("enter attribute to sort by");
                        string attr = Console.ReadLine();
                        containers.Sort(attr);
                    }
                    else if (choice == "6")
                    {
                        //delete
                        Console.WriteLine("Enter id to delete");
                        string id = Console.ReadLine();
                        containers.DeleteById(id);
                    }
                    else if (choice == "7")
                    {
                        //edit
                        Console.WriteLine("Enter id to edit");
                        string id = Console.ReadLine();
                        Console.WriteLine("Enter prop to edit");
                        string pr = Console.ReadLine();
                        Console.WriteLine("Enter prop value to edit");
                        string val = Console.ReadLine();
                        containers.EditById(id, pr, val);
                    }
                    else if (choice == "8")
                    {
                        //write to file
                        Console.WriteLine("Enter file name to write to");
                        string file = Console.ReadLine();
                        containers.WriteJsonFile(file);
                    }
                    else if (choice == "9")
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
    }
}
