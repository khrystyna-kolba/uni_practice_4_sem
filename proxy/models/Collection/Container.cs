using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json.Serialization;
using pattern_proxy_np.Extension;
using System.Reflection;
using System.Text.Json;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
//using JsonConstructorAttribute = Newtonsoft.Json.JsonConstructorAttribute;
using System.ComponentModel.Design;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Runtime.CompilerServices;
using System.IO;
using System.Xml.Linq;

namespace pattern_proxy_np.models.Collection
{
    public class Container : IBaseClass<Container>
    {
        public Container(Dictionary<string, object> dict)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            foreach (var p in dict)
            {
                try
                {
                    if (GetType().GetProperty(p.Key.ToPascalCase()).PropertyType.IsEnum)
                    {
                        City c = 0;
                        GetType().GetProperty(p.Key.ToPascalCase()).SetValue(this, c.ParseEnum(p.Value.ToString()));
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
                throw new ArgumentException("Container can't be created\n" + er);
            }
        }
        public static Container FromDict(Dictionary<string, object> dict)
        {
            //Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return new Container(dict);
        }
        public override string ToString()
        {
            string data = "Container:\n";
            var options = new JsonSerializerOptions { WriteIndented = true };
            return data + System.Text.Json.JsonSerializer.Serialize(this, options);
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
        static public Container Input()
        {
            var snakeCaseStrategy = new SnakeCaseNamingStrategy();
            var p = typeof(Container).GetProperties();
            Dictionary<string, object> strings = new Dictionary<string, object>();
            for (int i = 0; i < p.Length; i++)
            {
                Console.Write(snakeCaseStrategy.GetPropertyName(p[i].Name, false) + ": ");
                strings[p[i].Name] = Console.ReadLine();
            }
            return new Container(strings);

        }
        private string id;
        private string number;
        private DateTime arrival_date;
        private int amount_of_items;
        [JsonPropertyName("id")]
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = ContainerValidation.IdValidation(value);
            }
        }
        [JsonPropertyName("number")]
        public string Number
        {
            get
            {
                return number;
            }
            set
            {
                number = ContainerValidation.NumberValidation(value);
            }
        }
        [JsonPropertyName("departure_city")]
        public City DepartureCity { get; set; }
        [JsonPropertyName("arrival_city")]
        public City ArrivalCity { get; set; }
        [JsonPropertyName("departure_date")]
        public DateTime DepartureDate { get; set; }
        [JsonPropertyName("arrival_date")]
        public DateTime ArrivalDate
        {
            get
            {
                return arrival_date;
            }
            set
            {
                arrival_date = ContainerValidation.ArrivalDateValidation(DepartureDate, value);
            }
        }
        [JsonPropertyName("amount_of_items")]
        public int AmountOfItems
        {
            get
            {
                return amount_of_items;
            }
            set
            {
                amount_of_items = ContainerValidation.AmountValidation(value);
            }
        }


    }
}
