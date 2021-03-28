using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;

namespace BlockchainProject
{
   public class Client
   
   {
        public IDictionary<String, WebSocket> wsd = new Dictionary<String,WebSocket>(); 
        
        public void Connect (String url)
        {
            if (!wsd.ContainsKey(url))
            {
                WebSocket wsc = new WebSocket(url);
                wsc.OnMessage += (sender, e) =>
                {
                    if (e.Data == "Hello Client")
                    {
                        Console.WriteLine(e.Data);
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
                };
                wsc.Connect();
                wsc.Send("Hello Server");
                wsc.Send(JsonConvert.SerializeObject(Program.ourblockchain));
                wsd.Add(url, wsc);
            }

        }
        public void send(String url, String data)
        {
            foreach (var item in wsd)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }
        public void broadcast(String data)
        {
            foreach (var item in wsd)
            {
                item.Value.Send(data);
            }
        }
        public IList<String> GetServers()
        {
            IList<String> servers = new List<String>();
            foreach (var item in wsd)
            {
                servers.Add(item.Key);
            }
            return servers;
        }
        public void close()
        {
            foreach (var item in wsd)
            {
                item.Value.Close();
            }
        }
   }


}
