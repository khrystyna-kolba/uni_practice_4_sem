using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pattern_proxy_np.models.Collection
{
    public class UserValidation
    {
        public static string NameValidation(string name)
        {
            Regex re = new Regex(@"^[A-Z][a-z]+$");
            if (!re.IsMatch(name))
            {
                throw new ArgumentException("name should contain only letters and start with capital one");
            }
            return name;
        }
        public static string PasswordValidation(string password)
        {
            // Regex re = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
            Regex re = new Regex(@"^(?=.*[a-z])(?=.*[A-Z]).{8,}$");
            if (!re.IsMatch(password))
            {
                throw new ArgumentException("password should contain at least one capital letter, one letter and be at least 8 characters long");
            }
            return password;
        }
        public static string IdValidation(string id)
        {
            foreach (var c in id)
            {
                if (!char.IsDigit(c))
                {
                    throw new ArgumentException("id should contain only digits");
                }
            }
            return id;
        }
        public static string EmailValidation(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return email;
            }
            catch (FormatException)
            {
                throw new ArgumentException("email is invalid");
            }
        }
    }
}
