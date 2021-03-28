using Newtonsoft.Json;
using System;

namespace BlockchainProject
{
    class Program
    {
        public static Blockchain ourblockchain = new Blockchain();
        public static Client cl = new Client();
        public static Server sv = null;
        public static int port = 0;
        public static string name = "Test";
        static void Main(string[] args)
        {
           
            DateTime startPoint = DateTime.Now;
            ourblockchain.InitializeChain();
            if (args.Length>=1)
            {
                port = int.Parse(args[0]);
            }if (args.Length >= 2){
                name = args[1];
            }if (port > 0){
                sv = new Server();
                sv.start();
            }if (name != "Test"){
                Console.WriteLine($"Current User: {name}");
                Console.WriteLine($"1-Connect to Server");
                Console.WriteLine($"2-Add Transaction");
                Console.WriteLine($"3-Show Blockchain");
                Console.WriteLine($"Exit");
                int selection = 0;
                while (selection != 4)
                {
                    switch (selection)
                    {
                        case 1:
                            Console.WriteLine("Server URL: ");
                            string serverURL = Console.ReadLine();
                            cl.Connect($"{serverURL}/Blockchain");
                            break;
                        case 2:
                            Console.WriteLine("Receiver Name: ");
                            String receivername = Console.ReadLine();
                            Console.WriteLine("Amount: ");
                            String amount = Console.ReadLine();
                            ourblockchain.CreateTransaction(new Transaction(name,receivername,int.Parse(amount)));
                            ourblockchain.ProcessPendingTransactions(name);
                            cl.broadcast(JsonConvert.SerializeObject(ourblockchain));
                            break;
                        case 3:
                            Console.WriteLine("Blockchain: ");
                            Console.WriteLine(JsonConvert.SerializeObject(ourblockchain, Formatting.Indented));
                            break;
                    }
                    Console.WriteLine("Please Choose One of Them: ");
                    string action = Console.ReadLine();
                    selection = int.Parse(action);
                }
                cl.close();
            }
            Console.ReadKey();
        }
    }
}
