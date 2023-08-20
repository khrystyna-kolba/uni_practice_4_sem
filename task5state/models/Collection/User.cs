using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace pattern_proxy_np.models.Collection
{
    public class User : IBaseClass<User>
    {
        private string id;
        private string firstName;
        private string lastName;
        private string email;
        private Role role;
        private string password;
        public string Id { get { return id; } set { id = UserValidation.IdValidation(value); } }
        public string FirstName { get { return firstName; } set { firstName = UserValidation.NameValidation(value); } }
        public string LastName { get { return lastName; } set { lastName = UserValidation.NameValidation(value); } }
        public string Email { get { return email; } set { email = UserValidation.EmailValidation(value); } }
        public Role Role { get { return role; } set { role = value; } }
        public string Password { get { return password; } set 
            {
                //password = UserValidation.PasswordValidation(value);
                //password = Hash.HashString(password);
                password = value;
            } 
        }
        public bool CheckPassword(string password)
        {
            return this.password == Hash.HashString(password);
        }
        public string GetFullName()
        {
            return firstName + " " + lastName;
        }

        public User(Dictionary<string, object> dict)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            foreach (var p in dict)
            {
                try
                {
                    if (GetType().GetProperty(p.Key.ToPascalCase()).PropertyType.IsEnum)
                    {
                        //error
                        string v = p.Value.ToString().TransformEnum();
                        var tp = typeof(User).GetProperty(p.Key.ToPascalCase()).PropertyType;
                        GetType().GetProperty(p.Key.ToPascalCase()).SetValue(this, Convert.ChangeType(Enum.Parse(tp, v, true), tp));
                    }
                    else
                    {
                        GetType().GetProperty(p.Key.ToPascalCase()).SetValue(this, Convert.ChangeType(p.Value.ToString(), GetType().GetProperty(p.Key.ToPascalCase()).PropertyType));
                    }
                }
                catch (TargetInvocationException e)
                {
                    errors.Add(p.Key, e.InnerException.Message);
                }
                catch (Exception e)
                {
                    errors.Add(p.Key, e.Message);
                }
            }
            if (errors.Count > 0)
            {
                string er = "";
                foreach (var d in errors)
                {
                    er += d.Key + ": " + d.Value + "\n";
                }
                throw new ArgumentException("User can't be created\n" + er);
            }
        }
        public static User FromDict(Dictionary<string, object> dict)
        {
            return new User(dict);
        }
        static public User Input()
        {
            var snakeCaseStrategy = new SnakeCaseNamingStrategy();
            var p = typeof(User).GetProperties();
            Dictionary<string, object> strings = new Dictionary<string, object>();
            for (int i = 0; i < p.Length; i++)
            {
                if (snakeCaseStrategy.GetPropertyName(p[i].Name, false) != "role")
                {
                    Console.Write(snakeCaseStrategy.GetPropertyName(p[i].Name, false) + ": ");
                    strings[p[i].Name] = Console.ReadLine();
                }
            }
            return new User(strings);

        }
        public Dictionary<string, object> GetDict()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (var p in GetType().GetProperties())
            {
                dict.Add(p.Name, p.GetValue(this));
            }
            return dict;
        }
    }
}
