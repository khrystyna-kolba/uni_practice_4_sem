using pattern_proxy_np.models.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pattern_proxy_np.models.Proxy
{
    public class ProgramInterface
    {
        Auth auth;
        RealSubject subject;
        PermissionProxy permission_proxy;
        LoggerProxy proxy;
        public ProgramInterface() 
        {
            auth = new Auth();
            subject = new RealSubject();
            permission_proxy = new PermissionProxy(subject, auth);
            proxy = new LoggerProxy(permission_proxy);
        }
        private void ReadFile()
        {
            Console.WriteLine("Enter file name to read from");
            string file = Console.ReadLine();
            proxy.ReadFromJsonFile(file);
        }
        private void Search()
        {
            //search
            Console.WriteLine("Enter value to search: ");
            Console.WriteLine(proxy.Search(Console.ReadLine()));
        }
        private void Create()
        {
            //add new
            Console.WriteLine("enter container data");
            proxy.Create();
        }
        private void ViewList()
        {
            //print
            Console.WriteLine(proxy.ViewList());
        }
        private void Delete()
        {
            //delete
            Console.WriteLine("Enter id to delete");
            proxy.Delete(Console.ReadLine());
        }
        private void Edit()
        {
            //edit
            Console.WriteLine("Enter id to edit");
            string id = Console.ReadLine();
            Console.WriteLine("Enter prop to edit");
            string pr = Console.ReadLine();
            Console.WriteLine("Enter prop value to edit");
            string val = Console.ReadLine();
            proxy.Edit(id, pr, val);
        }
        private void Sort()
        {
            //sort
            Console.WriteLine("enter attribute to sort by");
            Console.WriteLine(proxy.Sort(Console.ReadLine()));
        }
        private void WriteFile()
        {
            //write to file
            Console.WriteLine("Enter file name to write to");
            string file = Console.ReadLine();
            proxy.WriteToJsonFile(file);
        }
        private void Login()
        {
            Console.WriteLine("Enter your email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();

            auth.Login(email, password);
        }
        private void Logout()
        {
            auth.Logout();
        }
        private void Register()
        {
            auth.Register();
            auth.Logout();
        }
        public void RunMenu()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("enter your choice \n 1 - read from file \n 2 - search in the current collection of containers \n" +
                        " 3 - create new Container and add to current collection \n 4 - print current collection \n 5 - sort " +
                        "collection by property\n 6 - delete Container from collection by ID \n 7 " +
                        "- edit container by ID \n 8 - write collection to json file \n" +
                        " 9 - login \n 10 - logout \n 11 - register \n 12 - exit \n");
                    string choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        ReadFile();

                    }
                    else if (choice == "2")
                    {
                        Search();
                    }
                    else if (choice == "3")
                    {
                        Create();
                    }
                    else if (choice == "4")
                    {
                        ViewList();
                    }
                    else if (choice == "5")
                    {
                        Sort();
                    }
                    else if (choice == "6")
                    {
                        Delete();
                    }
                    else if (choice == "7")
                    {
                        Edit();
                    }
                    else if (choice == "8")
                    {
                        WriteFile();
                    }
                    else if (choice == "12")
                    {
                        break;
                    }
                    else if (choice == "9")
                    {
                        Login();
                    }
                    else if (choice == "10")
                    {
                        Logout();
                    }
                    else if (choice == "11")
                    {
                        Register();
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
    }
}
