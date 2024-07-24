using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Projektas9.Models
{
    public class Account
    {
        public string Number { get; set; }
        public decimal Balance { get; set; }

        public List<Transaction> Transactions { get; set; }

        public Account()
        {
            Number = "LT"+DateTime.Now.Ticks.ToString();
            Balance = 0.00M;
            Transactions = new List<Transaction>();
        }

        /// <summary>
        /// Deposits money into account. Amount is firstly rounded to lower number in case user made an input like "52.3666". It multiplies amount by 100, gets the integer value of the resul and divides by 100.
        /// </summary>
        /// <param name="amount"></param>
        public void Deposit(decimal amount)
        {
            amount = Math.Floor(amount * 100M) * 0.01M;
            Transactions.Add(new Transaction(amount, "Deposit", Balance));
            Balance += amount;
        }

        /// <summary>
        /// Withdraws money from account if it has enough money. Amount is firstly rounded to lower number in case user made an input like "52.3666". It multiplies amount by 100, gets the integer value of the resul and divides by 100.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Withdraw(decimal amount)
        {
            amount = Math.Floor(amount * 100M) * 0.01M;
            if (Balance > amount)
            {
                Transactions.Add(new Transaction(amount, "Withdraw", Balance));
                Balance -= amount;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns account number
        /// </summary>
        /// <returns></returns>
        public string GetNumber()
        {
            return Number;
        }

        /// <summary>
        /// Returns account balance
        /// </summary>
        /// <returns></returns>
        public decimal GetBalance()
        {
            return Balance;
        }

        /// <summary>
        /// Compiles all transactions into one string and returns it
        /// </summary>
        /// <returns></returns>
        public string GetHistory()
        {
            if (Transactions.Count > 0)
            {
                string temp = "";

                foreach (Transaction transaction in Transactions)
                    temp += transaction;
                return temp + ToString();
            }
            else return "There are no transactions for this account";
        }

        public override string ToString()
        {
            return $"Account: {Number}      Balance: {Balance} Eur";
        }
    }
}
