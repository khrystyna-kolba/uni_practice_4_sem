using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using pattern_proxy_np.models.Collection;

namespace pattern_proxy_np.models.Authentification
{
    public class Auth
    {
        private User? current_user;
        private static readonly string usersFile = "users.json";
        private Collection<User>? users;
        public User CurrentUser { get => current_user; }
        public Auth()
        {
            users = new Collection<User>();
            users.ReadJsonFile(usersFile);
        }
        public void Update()
        {
            users.WriteJsonFile(usersFile);
        }
        public void Login(string email, string password)
        {
            foreach (User user in users)
            {
                if (user.Email == email && user.CheckPassword(password))
                {
                    current_user = user;
                    return;
                }
            }
            throw new InvalidCredentialException("invalid credentials");
        }
        public void Logout()
        {
            current_user = null;
            Console.WriteLine("you have logged out");
        }
        public void Register()
        {
            User new_user = User.Input();
            new_user.Password = UserValidation.PasswordValidation(new_user.Password);
            new_user.Password = Hash.HashString(new_user.Password);
            foreach (User us in users)
            {
                if(us.Email == new_user.Email)
                {
                    throw new ArgumentException("user with that email is already registered");
                }
            }
            new_user.Role = Role.customer;
            users.Add(new_user);
            Update();
        }
     
    }
}
