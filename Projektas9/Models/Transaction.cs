using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Projektas9.Models
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }

        public Transaction(decimal amount, string type, decimal balance)
        {
            Amount = amount;
            Type = type;
            Balance = balance;
        }

        public override string ToString()
        {
            return $"Old balance: {Balance} Eur     Type: {Type}   Amount: {Amount} Eur\n";
        }
    }
}
