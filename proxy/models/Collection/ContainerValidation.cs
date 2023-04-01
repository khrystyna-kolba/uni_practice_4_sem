using pattern_proxy_np.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace pattern_proxy_np.models.Collection
{
    public class ContainerValidation
    {
        public static DateTime ArrivalDateValidation(DateTime s, DateTime l)
        {
            if (s > l)
            {
                throw new ArgumentException("departure date can't be later than arrival date");
            }
            return l;
        }
        public static string NumberValidation(string num)
        {
            Regex re = new Regex(@"^[A-Z]{2}-[0-9]{5}$");
            if (!re.IsMatch(num))
            {
                throw new ArgumentException("number should follow pattern AB-12345");
            }
            return num;
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
        public static DateTime DateValidation(string dt)
        {
            DateTime res;
            if (DateTime.TryParse(dt, out res))
            {
                return res;
            }
            throw new ArgumentException("date is invalid");
        }

        public static City CityValidation(string city)
        {
            object res;
            try
            {
                City c = new City();
                return c.ParseEnum(city);
            }
            catch
            {
                throw new ArgumentException("city is invalid");
            }
        }
        public static int IntValidation(string amount)
        {
            if (!int.TryParse(amount, out int res))
            {
                throw new ArgumentException("field should be integer");
            }
            return res;
        }
        public static int AmountValidation(int amount)
        {
            if (amount >= 0)
            {
                return amount;
            }
            throw new ArgumentException("field should be non-negative!");
        }
    }
}
