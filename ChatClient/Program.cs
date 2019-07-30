using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----- Chat Program -----");
            ChatClient client = new ChatClient();
            client.RunNode();
        }
    }
}
