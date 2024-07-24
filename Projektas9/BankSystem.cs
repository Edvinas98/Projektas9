using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Projektas9.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Projektas9
{
    internal class BankSystem
    {
        static void Main(string[] args)
        {
            List<Customer> customers = new List<Customer>();
            AddCustomers(ref customers);
            ListCustomers(ref customers, true);
            ListCustomers(ref customers, false);
            ListCommands();
            WaitForCommand(ref customers);
        }

        /// <summary>
        /// Reads customer count and creates a list of customers
        /// </summary>
        /// <param name="customers"></param>
        public static void AddCustomers(ref List<Customer> customers)
        {
            bool bSuccess = false;
            int totalCustomers = 0;
            string name = "";

            while (!bSuccess)
            {
                Console.Write("Enter how many customers you want to add: ");
                if (!int.TryParse(Console.ReadLine(), out totalCustomers) || totalCustomers < 1)
                    PrintWrongInput();
                else
                    bSuccess = true;
            }
            Console.WriteLine();
            for (int i = 0; i < totalCustomers; i++)
            {
                Console.WriteLine($"// Customer Nr. {i + 1} //");
                bSuccess = false;
                while (!bSuccess)
                {
                    Console.Write("Enter customer's name: ");
                    name = Console.ReadLine() ?? string.Empty;
                    if (name != string.Empty)
                    {
                        customers.Add(new Customer(name));
                        bSuccess = true;
                    }
                    else
                        PrintWrongInput();
                }
                AddAccounts(ref customers);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Reads account count for each customer and creates a list of accounts for each customer
        /// </summary>
        /// <param name="customers"></param>
        public static void AddAccounts(ref List<Customer> customers)
        {
            bool bSuccess = false;
            int totalAccounts = 0;
            Account tempAcc;
            decimal tempAmount = 0.00M;

            while (!bSuccess)
            {
                Console.Write("Enter how many accounts do you want to create for this customer: ");
                if (!int.TryParse(Console.ReadLine(), out totalAccounts) || totalAccounts < 1)
                    PrintWrongInput();
                else bSuccess = true;
            }

            for (int j = 0; j < totalAccounts; j++)
            {
                tempAcc = customers.Last().AddAccount();
                Console.Write($"Account ({tempAcc.Number}) for {customers.Last().Name} has been created. Enter a starting money value (Eur) to it: ");
                bSuccess = false;
                while (!bSuccess)
                {
                    if (!decimal.TryParse(Console.ReadLine(), out tempAmount) || tempAmount < 0)
                        PrintWrongInput();
                    else
                        bSuccess = true;
                }
                tempAmount = Math.Floor(tempAmount * 100M) * 0.01M;
                if (tempAmount > 0)
                {
                    tempAcc.Deposit(tempAmount);
                    Console.Write($"{tempAcc.GetBalance()} has been deposited into account {tempAcc.GetNumber()}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prints customers' accounts
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="bOneAccount">t</param>
        public static void ListCustomers(ref List<Customer> customers, bool bOneAccount)
        {
            List<Customer> tempCustomers = new List<Customer>();
            if (bOneAccount)
            {
                foreach (Customer customer in customers)
                {
                    if (customer.Accounts.Count == 1)
                        tempCustomers.Add(customer);
                }

                if (tempCustomers.Count > 0)
                {
                    Console.WriteLine("Customers who have one account:\n");
                    foreach (Customer customer in tempCustomers)
                    {
                        Console.WriteLine(customer);
                    }
                    Console.WriteLine();
                }
                else
                    Console.WriteLine("There are no customers who have one account\n");
            }
            else
            {
                foreach (Customer customer in customers)
                {
                    if (customer.Accounts.Count > 1)
                        tempCustomers.Add(customer);
                }

                if (tempCustomers.Count > 0)
                {
                    Console.WriteLine("Customers who have multiple accounts:\n");
                    foreach (Customer customer in tempCustomers)
                    {
                        Console.WriteLine(customer);
                        Console.WriteLine($"Total balance of all accounts: {customer.GetTotalBalance()}\n");
                    }
                }
                else
                    Console.WriteLine("There are no customers who have multiple accounts\n");
            }
        }

        /// <summary>
        /// Prints a list of commands with information
        /// </summary>
        public static void ListCommands()
        {
            Console.WriteLine("Use these commands to make changes or view customer information:");
            Console.WriteLine("Deposit <account number> <amount>    - deposit funds to specified account");
            Console.WriteLine("Withdraw <account number> <amount>   - withdraw funds from specified account");
            Console.WriteLine("Balance <account number>             - show balance of specified account");
            Console.WriteLine("History <account number>             - show transaction history of specified account");
            Console.WriteLine("Overview                             - list all customers and their accounts");
            Console.WriteLine("Exit                                 - stop the application");
            Console.WriteLine("Help                                 - view the list of commands");
            Console.WriteLine();
        }

        /// <summary>
        /// This function is responsible to keep the program running when it has done all the calculations.
        /// </summary>
        /// <param name="customers"></param>
        public static void WaitForCommand(ref List<Customer> customers)
        {
            string[] theCommand = Array.Empty<string>();

            while (true)
            {
                Console.Write("Enter a command: ");
                string cmd = Console.ReadLine() ?? string.Empty;
                if (cmd != string.Empty)
                {
                    if (GetCommand(cmd, out theCommand))
                    {
                        if (ExecuteCommand(ref customers, theCommand))
                            break;
                    }
                    else PrintWrongInput();
                }
                else
                    PrintWrongInput();
            }
        }

        /// <summary>
        /// First check if the given input is a valid command
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="theCommand"></param>
        /// <returns></returns>
        public static bool GetCommand(string cmd, out string[] theCommand)
        {
            string[] cmdArray = (cmd.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            theCommand = Array.Empty<string>();

            if (cmdArray.Length > 0)
            {
                cmdArray[0] = cmdArray[0].ToLower();
                if (cmdArray.Length == 1 && (cmdArray[0] == "overview" || cmdArray[0] == "exit" || cmdArray[0] == "help"))
                {
                    theCommand = new string[] { cmdArray[0] };
                    return true;
                }
                else if(cmdArray.Length >= 2 && (cmdArray[0] == "deposit" || cmdArray[0] == "withdraw" || cmdArray[0] == "balance" || cmdArray[0] == "history"))
                {
                    if(cmdArray.Length == 2)
                        theCommand = new string[] { cmdArray[0], cmdArray[1] };
                    else
                        theCommand = new string[] { cmdArray[0], cmdArray[1], cmdArray[2] };
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Launches simple commands and give a second check for more complicated commands
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="theCommand"></param>
        /// <returns></returns>
        static bool ExecuteCommand(ref List<Customer> customers, string[] theCommand)
        {
            bool bExit = false;

            if( theCommand.Length == 1 )
            {
                Console.WriteLine();
                switch (theCommand[0])
                {
                    case "overview":
                        ListCustomers(ref customers, true);
                        ListCustomers(ref customers, false);
                        break;

                    case "exit":
                        bExit = true;
                        break;

                    case "help":
                        ListCommands();
                        break;

                    default:
                        PrintWrongInput();
                        break;
                }
            }
            else if(theCommand.Length > 1 && (theCommand[0] == "deposit" || theCommand[0] == "withdraw" || theCommand[0] == "balance" || theCommand[0] == "history"))
                DoAccountCommand(ref customers, theCommand);
            else
                PrintWrongInput();

            return bExit;
        }

        /// <summary>
        /// Prints a message that user made a mistake with his input. A lot of cases can result in misinput, so this message has it's own function
        /// </summary>
        static void PrintWrongInput()
        {
            Console.WriteLine("Wrong input! Try again");
        }

        /// <summary>
        /// Does a last check for more complicated commands and launches them if all seems good
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="theCommand"></param>
        static void DoAccountCommand(ref List<Customer> customers, string[] theCommand)
        {
            decimal amount = 0;

            foreach(Customer customer in customers)
            {
                if (customer.FindAccount(theCommand[1]))
                {
                    Console.WriteLine();
                    switch (theCommand[0])
                    {
                        case "deposit":
                            if (theCommand.Length < 3)
                                PrintWrongInput();
                            else
                            {
                                if (!decimal.TryParse(theCommand[2], out amount) || amount <= 0)
                                    PrintWrongInput();
                                else
                                {
                                    customer.Deposit(theCommand[1], amount);
                                    Console.Write("Deposit was successful\n\n");
                                }
                            }
                            return;

                        case "withdraw":
                            if (theCommand.Length < 3)
                                PrintWrongInput();
                            else
                            {
                                if (!decimal.TryParse(theCommand[2], out amount) || amount <= 0)
                                    PrintWrongInput();
                                else
                                {
                                    if(customer.Withdraw(theCommand[1], amount))
                                        Console.Write("Withdraw was successful\n\n");
                                    else
                                        Console.Write("There is not enough funds in this account\n\n");
                                }
                            }
                            return;

                        case "balance":
                            Console.WriteLine(customer.GetBalance(theCommand[1]));
                            return;

                        case "history":
                            Console.WriteLine(customer.GetHistory(theCommand[1]));
                            return;

                        default:
                            PrintWrongInput();
                            return;
                    }
                }
            }
            PrintWrongInput();
        }
    }
}
