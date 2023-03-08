using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace np_4sem_proj
{
    internal class InputValidation

    {
        static public int InputInt(string text) {
            int num;
            Console.WriteLine($"enter integer {text}");
            string i = Console.ReadLine();
            if (int.TryParse(i, out num))
            {
                return num;
            }
            Console.WriteLine("you didn't enter an integer, try again");
            return InputInt(text);
        }
        static public int InputPositiveInt(string text)
        {
            int num;
            Console.WriteLine($"enter positive integer {text}");
            string i = Console.ReadLine();
            if (int.TryParse(i, out num))
            {
                if (num <= 0)
                {
                    Console.WriteLine("you entered number that is less or equal to zero");
                    return InputPositiveInt(text);
                }
                return num;
            }
            Console.WriteLine("you didn't enter an integer, try again");
            return InputPositiveInt(text);
        }
        static public int InputNonNegativeInt(string text)
        {
            int num;
            Console.WriteLine($"enter non negative integer {text}");
            string i = Console.ReadLine();
            if (int.TryParse(i, out num))
            {
                if (num < 0)
                {
                    Console.WriteLine("you entered number that is less than zero");
                    return InputNonNegativeInt(text);
                }
                return num;
            }
            Console.WriteLine("you didn't enter an integer, try again");
            return InputNonNegativeInt(text);
        }
        static public Tuple<int, int> InputRange()
        {
            Console.WriteLine("[a,b]");
            int a = InputInt("a a<=b");
            int b = InputInt("b a<=b");
            if (a<b) {
                return new Tuple<int, int>(a,b);
            }
            return new Tuple<int, int>(b, a);
        }
        static public List<int> InputList(int N)
        {
            List<int> res = new List<int>();
            for(int i=0; i<N;i++)
            {
                res.Add(InputInt($"list[{i}]"));
            }
            return res;
        }

    }
}
