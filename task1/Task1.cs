using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace np_4sem_proj
{
    internal class Task1
    {
        static private int fake_null = -100;
        static public List<int> EnteringListMenu()
        {
            while (true)
            {
                Console.WriteLine("enter your choice \n 1 - random generating \n 2 - manual input \n  3- (return []) \n");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    int N = InputValidation.InputNonNegativeInt("N - length of list");
                    Tuple<int,int> rng = InputValidation.InputRange();
                    List<int> lis = Generating.RandomGenerating(N, rng.Item1, rng.Item2);
                    return lis;
                }
                else if (choice == "2")
                {
                    int N = InputValidation.InputNonNegativeInt("N - length of list");
                    List<int> lis = InputValidation.InputList(N);
                    return lis;
                }
                else if (choice == "3")
                {
                    return new List<int>();
                }
                else
                {
                    continue;
                }
            }
        }
        static public List<int> Transform(List<int> lis, int k)
        {
            if (lis.Count == 0)
            {
                return lis;
            }
            int amax = lis.Select(x => Math.Abs(x)).Max();
            List<int> negative = new List<int>();
            List<int> positive = new List<int>();
            List<int> zeros = new List<int>();

            foreach(int i in lis)
            {
                if(i < 0)
                {
                    negative.Add(i);
                }
                else if (i > 0)
                {
                    positive.Add(i);    
                }
                else
                {
                    zeros.Add(i);
                }
            }
            int missing_group = fake_null;
            if(negative.Count == 0)
            {
                missing_group = -1;
            }
            else if(positive.Count == 0)
            {
                missing_group = 1;
            }
            else if(zeros.Count == 0)
            {
                missing_group = 0;
            }
            if(missing_group!= fake_null)
            {
                if (k > 0)
                {
                    for(int i=0; i<positive.Count;i++)
                    {
                        if (positive[i] == k)
                        {
                            positive.Insert(i + 1, Generating.GenerateNumInGroup(missing_group, amax));
                        }
                    }
                }
                else if (k < 0)
                {
                    for (int i = 0; i < negative.Count; i++)
                    {
                        if (negative[i] == k)
                        {
                            negative.Insert(i + 1, Generating.GenerateNumInGroup(missing_group, amax));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < zeros.Count; i++)
                    {
                        if (k==0)
                        {
                            zeros.Insert(i + 1, Generating.GenerateNumInGroup(missing_group, amax));
                        }
                    }
                }
            }
            negative.AddRange(positive);
            negative.AddRange(zeros);
            return negative;
        }
        /*Задано масив з N цілих чисел. Сформувати масив таким чином, щоб спочатку були всі від’ємні
 елементи масиву, потім додатні і, після них нульові, зберігши порядок. Якщо якоїсь групи
  чисел не існує, то після кожного числа, що дорівнює K вставити рандомне число х цієї
   групи. Наприклад, -5 0 -4 0 -5 -6 0. K= -5. Немає додатних чисел. -5 7 -4 -5 6 -6 0 0 0.
    Числа 7 і 6 - рандомні, після кожного -5.*/
        static public void Task_1()
        {
            List<int> lis = EnteringListMenu();
            while (true)
            {
                Console.WriteLine(string.Join(", ", lis));
                Console.WriteLine("enter your choice \n 1 - tranform \n 2 - return to creating list \n 3 - exit \n");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    int k = InputValidation.InputInt("k");
                    List<int> result = Transform(lis, k);
                    Console.WriteLine("list: " + string.Join(", ", lis));
                    Console.WriteLine("transformed list: " + string.Join(", ", result));
                }
                else if (choice == "2")
                {
                    lis = EnteringListMenu();
                    continue;
                }
                else if (choice == "3")
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
    }
}
