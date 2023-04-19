using pattern_proxy_np.models.Proxy;

namespace pattern_proxy_np
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            ProgramInterface P = new ProgramInterface();
            P.RunMenu();
        }
    }
}