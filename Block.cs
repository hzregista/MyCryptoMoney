using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainProject
{
   public class Block
    {
        public int Index { get; set; }
        public int Nonce { get; set; } = 0;
        public DateTime Stamp { get; set; }
        public String PreviousHash { get; set; }
        public String Hash { get; set; }

        public IList<Transaction> Tra { get; set; } 

        public Block (DateTime stamp, string previousHash, IList<Transaction> tra)
        {
            Index = 0;
            Stamp = stamp;
            PreviousHash = previousHash;
            Tra = tra;
        }
        public string CalcHash()
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inbytes = Encoding.ASCII.GetBytes($"{Stamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Tra)}-{Nonce}");
            byte[] outbytes = sha256.ComputeHash(inbytes);
            return Convert.ToBase64String(outbytes);
        }
        public void Mining( int diff)
        {
            var leadingZeros = new string('0',diff);
            while (this.Hash == null || this.Hash.Substring(0,diff)!=leadingZeros)
            {
                this.Nonce++;
                this.Hash = this.CalcHash();
            }
        }
    }
}
