using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektas9.Models
{
    public class Customer
    {
        public string Name { get; set; }
        public List<Account> Accounts { get; set; }

        public Customer(string name)
        {
            Name = name;
            Accounts = new List<Account>();
        }

        /// <summary>
        /// Adds a new account
        /// </summary>
        /// <returns></returns>
        public Account AddAccount()
        {
            Accounts.Add(new Account());
            return Accounts.Last();
        }

        /// <summary>
        /// Sums up balances in all accounts and returns it
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalBalance()
        {
            decimal totalBalance = 0.00M;

            foreach(Account account in Accounts)
            {
                totalBalance += account.GetBalance();
            }

            return totalBalance;
        }

        /// <summary>
        /// Checks if this customer owns an account with given number
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public bool FindAccount(string accountNumber)
        {
            foreach(Account account in Accounts)
            {
                if (account.Number == accountNumber)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Finds the account by account number and deposits money into it
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="amount"></param>
        public void Deposit(string accountNumber, decimal amount)
        {
            foreach (Account account in Accounts)
            {
                if (account.Number == accountNumber)
                    account.Deposit(amount);
            }
        }

        /// <summary>
        /// Finds the account by account number and withdraws money from it
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Withdraw(string accountNumber, decimal amount)
        {
            foreach (Account account in Accounts)
            {
                if (account.Number == accountNumber)
                    return account.Withdraw(amount);
            }
            return true; 
        }

        /// <summary>
        /// Returns balance of account with given account number
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public string GetBalance(string accountNumber)
        {
            foreach (Account account in Accounts)
            {
                if (account.Number == accountNumber)
                    return account + "\n";
            }
            return "Could not get balance of this account\n";
        }

        /// <summary>
        /// Returns all transaction history
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public string GetHistory(string accountNumber)
        {
            foreach (Account account in Accounts)
            {
                if (account.Number == accountNumber)
                    return account.GetHistory() + "\n";
            }
            return "Could not get balance of this account";
        }

        public override string ToString()
        {
            string description = $"Customer: {Name}";            
            
            foreach (Account account in Accounts)
                description += $"\n{account}";
            return description;
        }
    }
}
