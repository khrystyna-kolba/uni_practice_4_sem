using pattern_proxy_np.models.Authentification;
using pattern_proxy_np.models.Collection;
using pattern_proxy_np.models.State;
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
        private void Search(IState? filter = null)
        {
            //search
            Console.WriteLine("Enter value to search: ");
            Console.WriteLine(proxy.Search(Console.ReadLine(), filter));
        }
        private void Create()
        {
            //add new
            Console.WriteLine("enter container data");
            proxy.Create();
        }
        private void ViewList(IState? filter = null)
        {
            //print
            Console.WriteLine(proxy.ViewList(filter));
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
        private void Sort(IState? filter = null)
        {
            //sort
            Console.WriteLine("enter attribute to sort by");
            Console.WriteLine(proxy.Sort(Console.ReadLine(), filter));
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

        private void ViewById()
        {
            Console.WriteLine("Enter id to view");
            string id = Console.ReadLine();
            Console.WriteLine(proxy.ViewById(id));
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
        private void Publish()
        {
            Console.WriteLine("Enter id: ");
            string id = Console.ReadLine();
            proxy.Publish(id);
        }
        public void RunMenu()
        {
            while (true)
            {
                if (auth.CurrentUser is null)
                {
                    try
                    {
                        Console.WriteLine("enter your choice \n 1 - login \n 2 - register \n 3 - exit \n");
                        string choice1 = Console.ReadLine();
                        if (choice1 == "1")
                        {
                            Login();

                        }
                        else if (choice1 == "2")
                        {
                            Register();
                        }
                        else if (choice1 == "3")
                        {
                            break;
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
                else if (auth.CurrentUser.Role == Role.customer)
                {
                    try
                    {
                        Console.WriteLine("enter your choice \n 1 - ViewList \n 2 - ViewByID \n 3 - Search \n " +
                            "4 - Sort \n 5 - logout \n 6 - exit \n");
                        string choice2 = Console.ReadLine();
                        if (choice2 == "1")
                        {
                            ViewList(new Published());
                        }
                        else if (choice2 == "2")
                        {
                            ViewById();
                        }
                        else if (choice2 == "3")
                        {
                            Search(new Published());
                        }
                        else if (choice2 == "4")
                        {
                            Sort(new Published());
                        }
                        else if (choice2 == "5")
                        {
                            Logout();
                        }
                        else if (choice2 == "6")
                        {
                            break;
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
                else if (auth.CurrentUser.Role == Role.admin)
                {
                    try
                    {
                        Console.WriteLine("enter your choice \n 1 - Published \n 2 - Drafts \n 3 - Moderation \n 4 - Create Container \n 5 - ReadFile \n 6 - WriteToFile \n 7 - logout \n 8 - exit \n");
                        string choice2 = Console.ReadLine();
                        if (choice2 == "1")
                        {
                            while (true)
                            {
                                Console.WriteLine("YOU ARE ON PUBLISHED PAGE, PRESS R TO RETURN TO MAIN MENU \n");
                                try
                                {
                                    Console.WriteLine("enter your choice \n 1 - ViewPublished \n 2 - ViewById \n 3 - EditById \n" +
                                        " 4 - Search \n 5 - Sort \n 6 - DeleteById \n");

                                    string choice3 = Console.ReadLine();
                                    if (choice3 == "1")
                                    {
                                        ViewList(new Published());
                                    }
                                    else if (choice3 == "2")
                                    {
                                        ViewById();
                                    }
                                    else if (choice3 == "3")
                                    {
                                        Edit();
                                    }
                                    else if (choice3 == "4")
                                    {
                                        Search(new Published());
                                    }
                                    else if (choice3 == "5")
                                    {
                                        Sort(new Published());
                                    }
                                    else if (choice3 == "6")
                                    {
                                        Delete();
                                    }
                                    else if (choice3 == "R")
                                    {
                                        break;
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
                        else if (choice2 == "2")
                        {
                            while (true)
                            {
                                Console.WriteLine("YOU ARE ON DRAFTS PAGE, PRESS R TO RETURN TO MAIN MENU \n");
                                try
                                {
                                    Console.WriteLine("enter your choice \n 1 - ViewDrafts \n 2 - ViewById \n 3 - EditById \n" +
                                        " 4 - Search \n 5 - Sort \n 6 - Publish by Id \n 7 - DeleteById");

                                    string choice3 = Console.ReadLine();
                                    if (choice3 == "1")
                                    {
                                        ViewList(new Draft());
                                    }
                                    else if (choice3 == "2")
                                    {
                                        ViewById();
                                    }
                                    else if (choice3 == "3")
                                    {
                                        Edit();
                                    }
                                    else if (choice3 == "4")
                                    {
                                        Search(new Draft());
                                    }
                                    else if (choice3 == "5")
                                    {
                                        Sort(new Draft());
                                    }
                                    else if (choice3 == "6")
                                    {
                                        Publish();
                                    }
                                    else if (choice3 == "7")
                                    {
                                        Delete();
                                    }
                                    else if (choice3 == "R")
                                    {
                                        break;
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
                        else if (choice2 == "3")
                        {
                            while (true)
                            {
                                Console.WriteLine("YOU ARE ON MODERATION PAGE, PRESS R TO RETURN TO MAIN MENU \n");
                                try
                                {
                                    Console.WriteLine("enter your choice \n 1 - View Containers that are in moderation \n 2 - ViewById \n 3 - EditById \n" +
                                        " 4 - Search \n 5 - Sort \n 6 - PublishById \n 7 - DeleteById \n");

                                    string choice3 = Console.ReadLine();
                                    if (choice3 == "1")
                                    {
                                        ViewList(new Moderation());
                                    }
                                    else if (choice3 == "2")
                                    {
                                        ViewById();
                                    }
                                    else if (choice3 == "3")
                                    {
                                        Edit();
                                    }
                                    else if (choice3 == "4")
                                    {
                                        Search(new Moderation());
                                    }
                                    else if (choice3 == "5")
                                    {
                                        Sort(new Moderation());
                                    }
                                    else if (choice3 == "6")
                                    {
                                        Publish();
                                    }
                                    else if (choice3 == "7")
                                    {
                                        Delete();
                                    }
                                    else if (choice3 == "R")
                                    {
                                        break;
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
                        else if (choice2 == "4")
                        {
                            Create();
                        }
                        else if (choice2 == "5")
                        {
                            ReadFile();
                        }
                        else if (choice2 == "6")
                        {
                            WriteFile();
                        }
                        else if (choice2 == "7")
                        {
                            Logout();
                        }
                        else if (choice2 == "8")
                        {
                            break;
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
                else if (auth.CurrentUser.Role == Role.manager)
                {
                    try
                    {
                        Console.WriteLine("enter your choice \n 1 - Published \n 2 - Drafts \n 3 - Moderation \n 4 - Create Container \n 5 - logout \n 6 - exit \n");
                        string choice2 = Console.ReadLine();
                        if (choice2 == "1")
                        {
                            while (true)
                            {
                                Console.WriteLine("YOU ARE ON PUBLISHED PAGE, PRESS R TO RETURN TO MAIN MENU \n");
                                try
                                {
                                    Console.WriteLine("enter your choice \n 1 - ViewPublished \n 2 - ViewById \n 3 - EditById \n" +
                                        " 4 - Search \n 5 - Sort \n");
   
                                    string choice3 = Console.ReadLine();
                                    if (choice3 == "1")
                                    {
                                        ViewList(new Published());
                                    }
                                    else if (choice3 == "2")
                                    {
                                        ViewById();
                                    }
                                    else if (choice3 == "3")
                                    {
                                        Edit();
                                    }
                                    else if (choice3 == "4")
                                    {
                                        Search(new Published());
                                    }
                                    else if (choice3 == "5")
                                    {
                                        Sort(new Published());
                                    }
                                    else if (choice3 == "R")
                                    {
                                        break;
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
                        else if (choice2 == "2")
                        {
                            while (true)
                            {
                                Console.WriteLine("YOU ARE ON DRAFTS PAGE, PRESS R TO RETURN TO MAIN MENU \n");
                                try
                                {
                                    Console.WriteLine("enter your choice \n 1 - ViewDrafts \n 2 - ViewById \n 3 - EditById \n" +
                                        " 4 - Search \n 5 - Sort \n 6 - Moderation Request by Id \n");

                                    string choice3 = Console.ReadLine();
                                    if (choice3 == "1")
                                    {
                                        ViewList(new Draft());
                                    }
                                    else if (choice3 == "2")
                                    {
                                        ViewById();
                                    }
                                    else if (choice3 == "3")
                                    {
                                        Edit();
                                    }
                                    else if (choice3 == "4")
                                    {
                                        Search(new Draft());
                                    }
                                    else if (choice3 == "5")
                                    {
                                        Sort(new Draft());
                                    }
                                    else if (choice3 == "6")
                                    {
                                        Publish();
                                    }
                                    else if (choice3 == "R")
                                    {
                                        break;
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
                        else if (choice2 == "3")
                        {
                            while (true)
                            {
                                Console.WriteLine("YOU ARE ON MODERATION PAGE, PRESS R TO RETURN TO MAIN MENU \n");
                                try
                                {
                                    Console.WriteLine("enter your choice \n 1 - View Containers that are in moderation \n 2 - ViewById \n 3 - EditById \n" +
                                        " 4 - Search \n 5 - Sort \n");

                                    string choice3 = Console.ReadLine();
                                    if (choice3 == "1")
                                    {
                                        ViewList(new Moderation());
                                    }
                                    else if (choice3 == "2")
                                    {
                                        ViewById();
                                    }
                                    else if (choice3 == "3")
                                    {
                                        Edit();
                                    }
                                    else if (choice3 == "4")
                                    {
                                        Search(new Moderation());
                                    }
                                    else if (choice3 == "5")
                                    {
                                        Sort(new Moderation());
                                    }
                                    else if (choice3 == "R")
                                    {
                                        break;
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
                        else if (choice2 == "4")
                        {
                            Create();
                        }
                        else if (choice2 == "5")
                        {
                            Logout();
                        }
                        else if (choice2 == "6")
                        {
                            break;
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
}
