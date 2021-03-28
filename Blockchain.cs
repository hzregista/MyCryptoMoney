using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainProject
{
    public class Blockchain
    {
        public IList<Block> Chain { get; set; }
        public IList<Transaction> PendingTransaction = new List<Transaction>();
        public int diff { get; set; } = 2;
        public int gain { get; set; } = 1;
        public Blockchain()
        {
            InitializeChain();
            AddGenesisBlock();
        }
        public void InitializeChain()
        {
            Chain = new List<Block>();
        }
        public Block CreateGenesisBlock()
        {
            Block block = new Block(DateTime.Now, null, PendingTransaction);
            block.Mining(diff);
            PendingTransaction = new List<Transaction>();
            return block;
        }
        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }
        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }
        public void AddBlock(Block block)
        {
            Block latestblock = GetLatestBlock();
            block.Index = latestblock.Index + 1;
            block.PreviousHash = latestblock.Hash;
            block.Hash = block.CalcHash();
            block.Mining(this.diff);
            Chain.Add(block);
        }
        public void CreateTransaction (Transaction transaction)
        {
            PendingTransaction.Add(transaction);
        }
        public void ProcessPendingTransactions(String miner)
        {
            CreateTransaction(new Transaction(null, miner, gain));
            Block block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransaction);
            AddBlock(block);
            PendingTransaction = new List<Transaction>();

        }
        public bool IsValid()
        {
            for (int i =1; i<Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];
                if (currentBlock.Hash!= currentBlock.CalcHash() || currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }  
            }
            return true;
        }
        public int remainingCoin(String address)
        {
            int total = 0;
            for (int i = 0; i < Chain.Count; i++)
            {
                for (int j = 0; j < Chain[i].Tra.Count; j++)
                {
                    var transaction = Chain[i].Tra[j];
                    if (transaction.Receiver == address)
                    {
                        total += transaction.Amount;
                    }if (transaction.SenderAddress == address){
                        total -= transaction.Amount;
                    }
                }
            }
            return total;
        }

    }
}
