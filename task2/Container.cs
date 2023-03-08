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
using np_4sem_proj.Extension;
using System.Reflection;
using System.Text.Json;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using JsonConstructorAttribute = Newtonsoft.Json.JsonConstructorAttribute;
using System.ComponentModel.Design;

namespace np_4sem_proj
{
    public class Container
    {
        public Container(string id,string number, City departurecity, City arrivalcity,DateTime departuredate, DateTime arrivaldate, int amountofitems)
        {
            this.Id = id;
            this.Number = number;
            this.ArrivalCity = arrivalcity;
            this.DepartureCity = departurecity;
            this.ArrivalDate = arrivaldate;
            this.DepartureDate =departuredate;
            this.AmountOfItems = amountofitems;
            ValidationContext context = new ValidationContext
            (this, null, null);
            List<ValidationResult> validationResults = new
            List<ValidationResult>();
            bool valid = Validator.TryValidateObject
            (this, context, validationResults, true);
            if (!ContainerValidation.ArrivalDateValidation(departuredate, arrivaldate))
            {
                valid = false;
                validationResults.Add(new ValidationResult("departure date can't be later than arrival date"));
            }
            if (!valid)
            {
                string er = "";
                foreach (ValidationResult validationResult in
               validationResults)
                {
                    er+= validationResult.ErrorMessage + "\n";
                }


                throw new ArgumentException(er);
            }
         
        }
        public Container(params string[] p)
        {
            List<string> props = GetPropsNames();
            Dictionary<string, string> errors = new Dictionary<string, string>();
            for(int i=0; i< props.Count;i++)
            {
                try
                {
                    SetProp(props[i], p[i]);
                }
                catch(ArgumentException e) {
                    errors.Add(props[i], e.Message);
                }
            }
            if(errors.Count > 0 )
            {
                string er = "";
                foreach(var d in errors)
                {
                    er+= d.Key + ": " + d.Value + "\n";
                }
                throw new ArgumentException("Container can't be created\n" + er);
            }
        }
        public string Serialize()
        {
            string s = JsonConvert.SerializeObject(GetDict(), Formatting.Indented);
            return s;
        }
        public static Container Deserialize(string json)
        {
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return new Container(dict["id"], dict["number"], dict["departure_city"], dict["arrival_city"], dict["departure_date"], dict["arrival_date"], dict["amount_of_items"]);
        }
        public override string ToString()
        {
            string data = "Container:\n";
            foreach(var i in this.GetDict())
            {
                data += i.Key + ": " + i.Value + "\n";
            }
            return data;
        }
        public Dictionary<string, string> GetDict()
        {
            if (this == null) return new Dictionary<string, string>();
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                { "id", Id.ToString() },
                { "number", Number.ToString() },
                { "departure_city", DepartureCity.GetName() },
                { "arrival_city", ArrivalCity.GetName() },
                { "departure_date", DepartureDate.ToString("dd.MM.yyyy") },
                { "arrival_date", ArrivalDate.ToString("dd.MM.yyyy") },
                { "amount_of_items", AmountOfItems.ToString() }
            };

            return dict;
        }
        static public List<string> GetPropsNames()
        {
            List<string> res = new List<string>
            {
                "id",
                "number",
                "departure_city",
                "arrival_city",
                "departure_date",
                "arrival_date",
                "amount_of_items"
            };
            return res;
        }
        public void SetProp(string prop, string value)
        {
            if (GetPropsNames().Contains(prop))
            {

                if (prop == "id")
                {
                    SetId(value);
                }
                else if (prop == "number")
                {
                    SetNumber(value);
                }
                else if (prop == "departure_city")
                {
                    SetStringDepartureCity(value);
                }
                else if (prop == "arrival_city")
                {
                    SetStringArrivalCity(value);
                }
                else if (prop == "departure_date")
                {
                    SetStringDepartureDate(value);
                }
                else if (prop == "arrival_date")
                {
                    SetStringArrivalDate(value);
                }
                else
                {
                    SetStringAmountOfItems(value);
                }
                  
            }
            else
            {
                throw new ArgumentException("cant set, this property doesn't exist");
                //Console.WriteLine("cant set, this property does'nt exist");
            }
        }
        static public Container Input()
        {
            PropertyInfo[] props = typeof(Container).GetProperties();
            string[] strings = new string[props.Length];
            for (int i = 0; i < props.Length; i++)
            {
                Console.Write(props[i].Name.ToSnakeCase() + ": ");
                strings[i] = Console.ReadLine();
            }
            return new Container(strings);

        }
        [JsonPropertyName("id")]
        [ValidateId(ErrorMessage = "id should contain only digits")]
        public string Id { get; set; }
        [JsonPropertyName("number")]
        [RegularExpression(@"^[A-Z]{2}-[0-9]{5}$", ErrorMessage = "number invalid representation")]
        public string Number { get; set; }
        [JsonPropertyName("departure_city")]
        public City DepartureCity { get; set; }
        [JsonPropertyName("arrival_city")]
        public City ArrivalCity { get; set; }
        [JsonPropertyName("departure_date")]
        public DateTime DepartureDate { get; set; }
        [JsonPropertyName("arrival_date")]
        public DateTime ArrivalDate { get; set; }
        [JsonPropertyName("amount_of_items")]
        [ValidateMin(0, ErrorMessage = "amount_of_items should be greater than or equal to zero")]
        public int AmountOfItems { get; set; }

        public void SetStringAmountOfItems(string a)
        {
            if (!ContainerValidation.AmountValidation(a.ToString()))
            {
                throw new ArgumentException("amount of items should be non-negative integer");
            }
            AmountOfItems = int.Parse(a.ToString());
        }
        public void SetStringDepartureCity(string city)
        {
            if (!ContainerValidation.CityValidation(city))
            {
                throw new ArgumentException("city is invalid");
            }
            DepartureCity = city.TransformCity().Parse();
    
        }
        public void SetStringArrivalCity(string city)
        {
            if (!ContainerValidation.CityValidation(city))
            {
                throw new ArgumentException("city is invalid");
            }
            ArrivalCity = city.TransformCity().Parse();

        }
        public void SetStringDepartureDate(string d)
        {
            if (!ContainerValidation.DateValidation(d))
            {
                throw new ArgumentException("date is invalid");
              
            }
            DepartureDate = DateTime.Parse(d);
        }
        public void SetStringArrivalDate(string d)
        {
            if (ContainerValidation.DateValidation(d))
            {
                if (!ContainerValidation.ArrivalDateValidation(DepartureDate, DateTime.Parse(d)))
                {
                    throw new ArgumentException("departure date can't be later than arrival date");
                }
                ArrivalDate = DateTime.Parse(d);
                return;
                
            }
            throw new ArgumentException("date is invalid");
        }
        public void SetId(string i)
        {
            if (!ContainerValidation.IdValidation(i))
            {
                throw new ArgumentException("id should contain only digits");
            }
            Id = i;
        }
        public void SetNumber(string n)
        {
            if (!ContainerValidation.NumberValidation(n))
            {
                throw new ArgumentException("number should follow pattern AB-12345");
            }
            Number = n;
        }

    }
}
