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
using ContainersApiTask.Models;
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
using ContainersApiTask.Models.State;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Azure;

namespace ContainersApiTask.Models.Containers
{
    [DataContract]
    public class Container : IBaseClass<Container>
    {
        private IState _state = new Draft();
        public IState GetState() { return _state; }
        public void SetState(IState state)
        {
            _state = state;
        }
        public void Publish(string role)
        {
            _state.Publish(this, role);
        }
        public void Unpublish()
        {
            _state.Unpublish(this);
        }
        public Container(StateEnum state, string id, string number, City departureCity, City arrivalCity, DateTime departureDate, DateTime arrivalDate, int amountOfItems)
            : this(MakeDict(state, id, number, departureCity, arrivalCity, departureDate, arrivalDate, amountOfItems))
        { 
        }
        static public Dictionary<string, object> MakeDict(StateEnum state, string id, string number, City departureCity, City arrivalCity, DateTime departureDate, DateTime arrivalDate, int amountOfItems)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>()
            {{"state", state },
                {"id", id},
            { "number", number},
                {"departure_city" , departureCity },
                {"arrival_city", arrivalCity },
                {"departure_date", departureDate},
                {"arrival_date",  arrivalDate },
                {"amount_of_items", amountOfItems}
            };
            return dict;
        }
        public Container(Dictionary<string, object> dict)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            foreach (var p in dict)
            {
                try
                {
                    if (GetType().GetProperty(p.Key.ToPascalCase()).PropertyType.IsEnum)
                    {
                        string v = p.Value.ToString().TransformEnum();
                        var tp = typeof(Container).GetProperty(p.Key.ToPascalCase()).PropertyType;
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
                throw new ArgumentException("Container can't be created\n" + er);
            }
        }
        public bool Search(string search)
        {
            foreach (var p in typeof(Container).GetProperties())
            {
                if (p.GetValue(this).ToString().ToLower().Contains(search.ToLower()))
                {
                    return true;
                }
            }

            return false;
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
        private City departure_city;
        private City arrival_city;
        private DateTime departure_date;
        private DateTime arrival_date;
        private int amount_of_items;
        [JsonPropertyName("state")]
        [DataMember(Order = 0)]
        public StateEnum State { 
            get
            {
                if (_state is Draft) 
                {
                    return StateEnum.DRAFT;
                }
                else if (_state is Moderation)
                {
                    return StateEnum.MODERATION;
                }
                else if (_state is Published)
                {
                    return StateEnum.PUBLISHED;
                }
                return 0;
            }
            set
            {
                if (value == StateEnum.DRAFT)
                {
                    _state = new Draft();
                }
                else if (value == StateEnum.MODERATION)
                {
                    _state = new Moderation();
                }
                else if (value == StateEnum.PUBLISHED)
                {
                    _state = new Published();
                }
            }
        }
        [JsonPropertyName("id")]
        [DataMember(Order = 1)]
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                //id = ContainerValidation.IdValidation(value);
                //SetState(new Draft());
            }
        }
        [JsonPropertyName("number")]
        [DataMember(Order = 2)]
        public string Number
        {
            get
            {
                return number;
            }
            set
            {
                number = ContainerValidation.NumberValidation(value);
                //SetState(new Draft());
            }
        }
        [JsonPropertyName("departure_city")]
        [DataMember(Order = 3)]
        public City DepartureCity
        {
            get
            {
                return departure_city;
            }
            set
            {
                departure_city = value;
                //SetState(new Draft());
            }
        }
        [JsonPropertyName("arrival_city")]
        [DataMember(Order = 4)]
        public City ArrivalCity
        {
            get
            {
                return arrival_city;
            }
            set
            {
                arrival_city = value;
               // SetState(new Draft());
            }
        }
        [JsonPropertyName("departure_date")]
        [DataMember(Order = 5)]
        public DateTime DepartureDate
        {
            get
            {
                return departure_date;
            }
            set
            {
                departure_date = value;
               // SetState(new Draft());
            }
        }
        [JsonPropertyName("arrival_date")]
        [DataMember(Order = 6)]
        public DateTime ArrivalDate
        {
            get
            {
                return arrival_date;
            }
            set
            {
                arrival_date = ContainerValidation.ArrivalDateValidation(DepartureDate, value);
                //SetState(new Draft());
            }
        }
        [JsonPropertyName("amount_of_items")]
        [DataMember(Order = 7)]
        public int AmountOfItems
        {
            get
            {
                return amount_of_items;
            }
            set
            {
                amount_of_items = ContainerValidation.AmountValidation(value);
                //SetState(new Draft());
            }
        }


    }
}
