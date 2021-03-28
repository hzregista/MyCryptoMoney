using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockchainProject
{
    public class Server : WebSocketBehavior
    {
        public WebSocketServer wss = null;
        bool isSyn = false;
        public void start()
        {
            wss = new WebSocketServer($"ws://127.0.0.1:{Program.port}");
            wss.AddWebSocketService<Server>("/Blockchain");
            wss.Start();
            Console.WriteLine($"Server is started here: ws://127.0.0.1:{Program.port}");

        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Hello Server")
            {
                Console.WriteLine(e.Data);
                Send("Hello Client");
            }
            else
            {
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                  if (newChain.IsValid() && newChain.Chain.Count > Program.ourblockchain.Chain.Count)
                  {
                     List<Transaction> newTransaction = new List<Transaction>();
                     newTransaction.AddRange(newChain.PendingTransaction);
                     newTransaction.AddRange(Program.ourblockchain.PendingTransaction);
                     newChain.PendingTransaction = newTransaction;
                     Program.ourblockchain = newChain;
                  }
            }
            if (!isSyn)
            {
                Send(JsonConvert.SerializeObject(Program.ourblockchain));
                isSyn = true;
            }
        }
    }
}
