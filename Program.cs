using System;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Transactions;


class Transaction
{
    public string TransactionId { get; set; }
    public string TransactionType { get; set; }
    public DateTime Date { get; set; }
    public double Amount { get; set; }

    public Transaction(string type, double amount)
    {
        this.TransactionType = type;
        this.Amount = amount;
        this.Date = DateTime.Now;
        this.TransactionId = Guid.NewGuid().ToString();
    }
}


class Account
{ 
    public string AccountNumber { get; set; }
    public string Type { get; set; }
    public double Balance { get; set; }

    public List<Transaction> transactions { get; set; }

    public Account(string type, double balance)
    {
        transactions = new List<Transaction>();
        this.AccountNumber = Guid.NewGuid().ToString();
        this.Type = type;
        transaction("deposit", balance);
    }
    public void transaction(string type, double amount)
    {
        if (type.Equals("deposit", StringComparison.OrdinalIgnoreCase))
        {
            this.Balance += amount;
            this.transactions.Add(new Transaction(type, amount));
            Console.WriteLine("Transaction Successful!!\n");
        }
        else
        {
            if (this.Balance < amount)
            {
                Console.WriteLine("Insufficient Funds!!\n");
            }
            else
            {
                this.Balance -= amount;
                this.transactions.Add(new Transaction(type, amount));
                Console.WriteLine("Transaction Successful!!\n");
            }
        }
    }


    public void statement()
    {
        if(transactions.Count == 0)
            Console.WriteLine("No Transactions Found !!");
        else
        {
            Console.Clear();
            Console.WriteLine("Account Statement:");
            Console.WriteLine("TransactionId\tDate\tType\tAmount");
            foreach(Transaction transaction in transactions)
            {
                Console.WriteLine($"{transaction.TransactionId} {transaction.Date} {transaction.TransactionType} {transaction.Amount}");
            }
        }
    }

    public void getBalance()
    {
        Console.WriteLine($"Your Account Balance is: {Balance}");
    }

    public double calculateInterest()
    {
        double interest = 0, rateOfInterest = 0.015;

        if (Type.Equals("checking")) {
            Console.WriteLine("Account must be Savings account..!!");
            return 0;
        }
        
        foreach( Transaction transaction in transactions)
        {
            double months = ((DateTime.Now - transaction.Date).TotalDays) / 30;
            if (transaction.TransactionType.Equals("deposit"))
            {
                interest += transaction.Amount * months * rateOfInterest;
            }
            else
            {
                interest -= transaction.Amount * months * rateOfInterest;
            }
        }
        return interest;
    }
}

class User
{
    public string Name { get; set; }
    
    public string Password { get; set; }
    
    public List<Account> accounts { get; set; }
    public User(string name, string password)
    {
        accounts = new List<Account>();
        this.Name = name;
        this.Password = password;
    }

    public void createAccount()
    {
        Console.Write("Choose type of Account to Create:\n1.Savings\n2.Checking: ");
        int type = Convert.ToInt32(Console.ReadLine());
        if(type > 2 || type < 1)
        {
            Console.WriteLine("Invalid Option\n");
            return;
        }
        Console.WriteLine("Enter Initial Deposit:\n");
        double amount = Convert.ToDouble(Console.ReadLine());
        if (amount <= 100)
        {
            Console.WriteLine("Invalid Initial Deposit..!!\n Minimum Initial Deposit must be atleast Rs.100\n");
            return;
        }
        string Type = "savings";
        if (type == 2) Type = "checking";

        accounts.Add(new Account(Type, amount));
        Console.WriteLine("Account Created Successfully!\n");
    }


    public void performAccountOperations()
    {
        Console.Clear();
        if (accounts.Count == 0)
        {
            Console.WriteLine("No accounts found!!");
            return;
        }

        Console.WriteLine("Choose an Account:");
        Console.WriteLine("  Account No.\tType\tBalance");
        for(int i = 0; i < accounts.Count; i++)
        {
            Console.WriteLine($"{i + 1}:{accounts[i].AccountNumber}\t{accounts[i].Type}\t{accounts[i].Balance}");
        }
        Console.WriteLine($"{accounts.Count + 1}: Exit");
        int idx = Convert.ToInt32(Console.ReadLine());
        if(idx <= 0 || idx > accounts.Count + 1)
        {
            Console.WriteLine("Invalid Option\n");
            return;
        }
        if(idx == accounts.Count + 1)
        {
            Console.WriteLine("Exiting.. Press Enter to Continue");
            return;
        }
        idx--;
        
        Console.Clear();
        while (true)
        {

            Console.Write("Choose type of operation:\n1.Deposit\n2.WithDrawl\n3.Account Statement\n4.Check Balance\n5.Calculate Interest\n6.Exit: ");
            int option = Convert.ToInt32(Console.ReadLine());
            if (option <= 0 || option > 6)
            {
                Console.WriteLine("Invalid Option\n");
                Console.ReadKey();
                Console.Clear();
                continue;
            }
            if (option == 6) break;
            if (option <= 2)
            {
                Console.Write("Enter Amount:");
                double amount = Convert.ToDouble(Console.ReadLine());
                string Type = "deposit";
                if (option == 2) Type = "withdrawl";
                accounts[idx].transaction(Type, amount);
            }
            else if(option == 3)
            {
                accounts[idx].statement();
            }
            else if(option == 4) 
            {
                accounts[idx].getBalance();
            }
            else
            {
                double interest = accounts[idx].calculateInterest();
                Console.WriteLine($"Interest calculated for the selected account: {Math.Round(interest, 3)}");
            }
            Console.WriteLine("Press Enter to Continue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}

class Program
{   
    static List<User> users = new List<User>();
    static void performOperations(User user)
    {
        while (true)
        {
            Console.WriteLine($"\nWelcome {user.Name}\n");
            Console.WriteLine("Choose an Option:\n1.Create an Account\n2.Choose Existing Account\n3.Logout");
            int option = Convert.ToInt32(Console.ReadLine());
            switch(option)
            {
                case 1: user.createAccount(); break;
                case 2: user.performAccountOperations(); break;
            }
            if(option == 3)
            {
                break;
            }
            if(option > 3 || option <= 0)
            {
                Console.WriteLine("Enter Valid option\n");
            }
            Console.WriteLine("Press Enter to Continue");
            Console.ReadKey();
            Console.Clear();
        }
    }

    static void login()
    {
        Console.WriteLine("\nPlease enter UserName and Password:");
        Console.Write("UserName: ");
        string name = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();
        User user = users.Find(i => i.Name == name && i.Password == password);
        if (user == null)
        {
            Console.WriteLine("\nInvalid Login Credentials!!\n");
        }
        else
        {
            Console.Clear();
            performOperations(user);
        }
        Console.WriteLine("Press Enter to Continue");
        Console.ReadKey();
        Console.Clear();
    }

    static void signup() {
        Console.WriteLine("\nWelcome to Bomb Squad Banking..\n");
        Console.WriteLine("Please enter UserName and Password to create a User:\n");

     
        Console.Write("UserName: ");
        string name = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();

        if (name.Length == 0 || password.Length == 0)
        {
            Console.WriteLine("\nUserName and Password cannot be empty\n");
        }
        else
        {
            User user = users.Find(i => i.Name == name);
            if (user == null)
            {
                users.Add(new User(name, password));
                Console.WriteLine("\nSignUp Successful..!!\n");
            }
            else
            {
                Console.WriteLine("\nUser Already Exists..!!\n");
            }
        }
        Console.WriteLine("Press Enter to Continue");
        Console.ReadKey();
        Console.Clear();
    }


    static void Main()
    {

        while (true)
        {
            Console.WriteLine("Welcome to Bomb Squad Banking..\n");
            Console.WriteLine("1.LogIn to your Account\n2.Create a New Account\n3.Exit ");
            int option = Convert.ToInt32(Console.ReadLine());
            switch (option)
            {
                case 1: login(); break;
                case 2: signup(); break;
            }
            if (option == 3) break;
            else if(option >= 3 || option <= 0)
            {
                Console.WriteLine("Enter Valid option..!!");
            }
        }
    }
}