using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainProject
{
    public class Transaction
    {
        public String SenderAddress { get; set; }
        public String Receiver { get; set; }
        public int Amount { get; set; }

        public Transaction(String senderAddress, String receiver, int amount)

        {
            this.SenderAddress = senderAddress;
            this.Receiver = receiver;
            this.Amount = amount;
        }
    }
}
