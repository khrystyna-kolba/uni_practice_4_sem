
namespace task1
{
    /*8. Перестановкою P [1..n] розміру n називається набір чисел від 1 до n,
 розташованих в певному порядку. При цьому в ньому мають бути присутні рівно
  один раз кожне з цих чисел. Прикладом перестановок є 1, 3, 4, 5, 2 (для n = 5)
   і 3, 2, 1(для n = 3), а, наприклад, 1, 2, 3, 4, 5, 1 перестановкою не є, так
   як число 1 зустрічається два рази. Число i називається нерухомою точкою для
    перестановки P, якщо P[i] = i.Наприклад, в перестановці 1, 3, 4, 2, 5
    рівно дві нерухомих точки: 1 і 5, а перестановка 4, 3, 2, 1 не має
    нерухомих точок.
Дано два числа: n та k. Знайдіть кількість перестановок
 розміру n з рівно k нерухомими точками.
Вхідні дані
Ввести з клавіатури два цілих числа n (1 ≤ n ≤ 9) і k(0 ≤ k ≤ n).
Вихідні дані
Вивести на екран відповідь на задачу.*/
    internal class Program
    {
        static public bool CheckInput(string n,string k)
        {
            if (Checks.CheckIfInt(n) && Checks.CheckIfInt(k))
            {
                int nn = int.Parse(n);
                int kk = int.Parse(k);
                if (Checks.CheckInRange(nn, 1, 9))
                {
                    if(Checks.CheckInRange(kk, 0, nn))
                    {
                        return true;
                    }
                    Console.WriteLine("You should enter k in range [0, n]");
                    return false;
                }
                Console.WriteLine("You should enter n in range [1,9]");
                return false;
            }
            else
            {
                Console.WriteLine("You should enter two integers");
                return false;
            }
        }
        private static long GetFactorial(int number)
        {
            if (number == 0)
            {
                return 1;
            }
            return number * GetFactorial(number - 1);
        }
        static public long MatemPermutes(int n, int k)
        {
            long c = GetFactorial(n) / (GetFactorial(k) * GetFactorial(n - k));
            long subfactorial = 0;
            for (int i = 0; i < n-k+1; i++)
            {
                int p = (int)Math.Pow(-1.0, i*1.0);
                subfactorial = subfactorial + (int)(p * GetFactorial(n - k) / GetFactorial(i));
   
            }
            return (int)(c * subfactorial);
        }


        static void Main(string[] args)
        {
            string stop = "false";
            while (true)
            {
                Console.WriteLine("Hello, World!, enter exit to exit\nanything else to continue\n");
                stop = Console.ReadLine();
                if (stop == "exit")
                {
                    break;
                }
                //input
                Console.WriteLine("input integer n (1 <= n <= 9)");
                string n = Console.ReadLine();
                Console.WriteLine("input integer k (0 <= k <= n)");
                string k = Console.ReadLine();

                if (CheckInput(n, k))
                {
                    int nn = int.Parse(n);
                    int kk = int.Parse(k);
                    Console.WriteLine("Result: "+ MatemPermutes(nn, kk) + "\n\n\n");
                }
                else
                {
                    Console.WriteLine("Something Bad Hapenned!!!");
                }
            }
        }
    }
}


