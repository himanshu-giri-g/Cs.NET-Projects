using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BudgetingTool
{
    public class Transaction
    {
        public string Description { get; }
        public decimal Amount { get; }
        public DateTime Date { get; }
        public bool IsExpense { get; }
        public string Category { get; }

        public Transaction(string description, decimal amount, bool isExpense, string category)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Amount = amount;
            Date = DateTime.Now;
            IsExpense = isExpense;
            Category = category ?? throw new ArgumentNullException(nameof(category));
        }

        public override string ToString()
        {
            return $"{(IsExpense ? "Expense" : "Income")}: {Description}, Amount: {Amount:C}, Date: {Date.ToShortDateString()}, Category: {Category}";
        }
    }

    public class Budget
    {
        private List<Transaction> transactions;

        public Budget()
        {
            transactions = new List<Transaction>();
        }

        public void AddTransaction(string description, decimal amount, bool isExpense, string category)
        {
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));
            if (string.IsNullOrEmpty(category)) throw new ArgumentNullException(nameof(category));

            transactions.Add(new Transaction(description, amount, isExpense, category));
            Console.WriteLine("Transaction added successfully.");
        }

        public void ViewTransactions()
        {
            if (transactions.Count == 0)
            {
                Console.WriteLine("No transactions available.");
                return;
            }

            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction);
            }
        }

        public void ViewTransactionsByCategory(string category)
        {
            if (string.IsNullOrEmpty(category)) throw new ArgumentNullException(nameof(category));

            var filteredTransactions = transactions.Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            if (filteredTransactions.Count == 0)
            {
                Console.WriteLine($"No transactions found for category: {category}");
            }
            else
            {
                foreach (var transaction in filteredTransactions)
                {
                    Console.WriteLine(transaction);
                }
            }
        }

        public void ViewTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            var filteredTransactions = transactions.Where(t => t.Date >= startDate && t.Date <= endDate).ToList();
            if (filteredTransactions.Count == 0)
            {
                Console.WriteLine("No transactions found for the selected date range.");
            }
            else
            {
                foreach (var transaction in filteredTransactions)
                {
                    Console.WriteLine(transaction);
                }
            }
        }

        public void ViewTransactionsByAmount(decimal minAmount, decimal maxAmount)
        {
            var filteredTransactions = transactions.Where(t => t.Amount >= minAmount && t.Amount <= maxAmount).ToList();
            if (filteredTransactions.Count == 0)
            {
                Console.WriteLine("No transactions found in the specified amount range.");
            }
            else
            {
                foreach (var transaction in filteredTransactions)
                {
                    Console.WriteLine(transaction);
                }
            }
        }

        public decimal CalculateTotalIncome()
        {
            return transactions.Where(t => !t.IsExpense).Sum(t => t.Amount);
        }

        public decimal CalculateTotalExpenses()
        {
            return transactions.Where(t => t.IsExpense).Sum(t => t.Amount);
        }

        public decimal CalculateBalance()
        {
            return CalculateTotalIncome() - CalculateTotalExpenses();
        }

        public void SaveTransactions(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var transaction in transactions)
                {
                    writer.WriteLine($"{transaction.Description}|{transaction.Amount}|{transaction.IsExpense}|{transaction.Category}|{transaction.Date}");
                }
            }
            Console.WriteLine("Transactions saved successfully.");
        }

        public void LoadTransactions(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found.");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 5 &&
                        decimal.TryParse(parts[1], out decimal amount) &&
                        DateTime.TryParse(parts[4], out DateTime date))
                    {
                        bool isExpense = bool.Parse(parts[2]);
                        var description = parts[0];
                        var category = parts[3];

                        transactions.Add(new Transaction(description, amount, isExpense, category));
                    }
                }
            }
            Console.WriteLine("Transactions loaded successfully.");
        }

        public void SearchTransactions(string searchTerm)
        {
            var filteredTransactions = transactions.Where(t => t.Description.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            if (filteredTransactions.Count == 0)
            {
                Console.WriteLine($"No transactions found for search term: {searchTerm}");
            }
            else
            {
                foreach (var transaction in filteredTransactions)
                {
                    Console.WriteLine(transaction);
                }
            }
        }

        public void GenerateReport()
        {
            Console.WriteLine("Budget Summary Report");
            Console.WriteLine($"Total Income: {CalculateTotalIncome():C}");
            Console.WriteLine($"Total Expenses: {CalculateTotalExpenses():C}");
            Console.WriteLine($"Balance: {CalculateBalance():C}");
            Console.WriteLine($"Total Transactions: {transactions.Count}");
            Console.WriteLine("Expenses by Category:");

            var expensesByCategory = transactions.Where(t => t.IsExpense)
                .GroupBy(t => t.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) });

            foreach (var category in expensesByCategory)
            {
                Console.WriteLine($"{category.Category}: {category.Total:C}");
            }
        }

        public void ViewUniqueCategories()
        {
            var uniqueCategories = transactions.Select(t => t.Category).Distinct().ToList();
            if (uniqueCategories.Count == 0)
            {
                Console.WriteLine("No categories found.");
            }
            else
            {
                Console.WriteLine("Unique Categories:");
                foreach (var category in uniqueCategories)
                {
                    Console.WriteLine(category);
                }
            }
        }

        public void DeleteTransaction(string description)
        {
            var transactionToDelete = transactions.FirstOrDefault(t => t.Description.Equals(description, StringComparison.OrdinalIgnoreCase));
            if (transactionToDelete != null)
            {
                transactions.Remove(transactionToDelete);
                Console.WriteLine($"Transaction '{description}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"No transaction found with description: {description}");
            }
        }

        public void EditTransaction(string description, decimal newAmount, bool newIsExpense, string newCategory)
        {
            var transactionToEdit = transactions.FirstOrDefault(t => t.Description.Equals(description, StringComparison.OrdinalIgnoreCase));
            if (transactionToEdit != null)
            {
                transactions.Remove(transactionToEdit);
                transactions.Add(new Transaction(description, newAmount, newIsExpense, newCategory));
                Console.WriteLine($"Transaction '{description}' updated successfully.");
            }
            else
            {
                Console.WriteLine($"No transaction found with description: {description}");
            }
        }

        public void GenerateMonthlyReport(int month, int year)
        {
            var monthlyTransactions = transactions.Where(t => t.Date.Month == month && t.Date.Year == year).ToList();
            if (monthlyTransactions.Count == 0)
            {
                Console.WriteLine("No transactions found for the specified month and year.");
                return;
            }

            decimal totalIncome = monthlyTransactions.Where(t => !t.IsExpense).Sum(t => t.Amount);
            decimal totalExpenses = monthlyTransactions.Where(t => t.IsExpense).Sum(t => t.Amount);
            decimal balance = totalIncome - totalExpenses;

            Console.WriteLine($"Monthly Report for {new DateTime(year, month, 1):MMMM yyyy}");
            Console.WriteLine($"Total Income: {totalIncome:C}");
            Console.WriteLine($"Total Expenses: {totalExpenses:C}");
            Console.WriteLine($"Balance: {balance:C}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Budget budget = new Budget();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nBudgeting Tool");
                Console.WriteLine("1. Add Transaction");
                Console.WriteLine("2. View Transactions");
                Console.WriteLine("3. View Total Income");
                Console.WriteLine("4. View Total Expenses");
                Console.WriteLine("5. View Balance");
                Console.WriteLine("6. View Transactions by Category");
                Console.WriteLine("7. View Transactions by Date Range");
                Console.WriteLine("8. View Transactions by Amount Range");
                Console.WriteLine("9. Save Transactions to File");
                Console.WriteLine("10. Load Transactions from File");
                Console.WriteLine("11. Search Transactions");
                Console.WriteLine("12. Generate Summary Report");
                Console.WriteLine("13. View Unique Categories");
                Console.WriteLine("14. Delete Transaction");
                Console.WriteLine("15. Edit Transaction");
                Console.WriteLine("16. Generate Monthly Report");
                Console.WriteLine("17. Exit");
                Console.Write("Choose an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Enter transaction description: ");
                        string description = Console.ReadLine();
                        Console.Write("Enter amount: ");
                        decimal amount;
                        while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
                        {
                            Console.Write("Please enter a valid amount: ");
                        }
                        Console.Write("Is this an expense? (y/n): ");
                        bool isExpense = Console.ReadLine()?.ToLower() == "y";
                        Console.Write("Enter category: ");
                        string category = Console.ReadLine();
                        budget.AddTransaction(description, amount, isExpense, category);
                        break;

                    case "2":
                        budget.ViewTransactions();
                        break;

                    case "3":
                        Console.WriteLine($"Total Income: {budget.CalculateTotalIncome():C}");
                        break;

                    case "4":
                        Console.WriteLine($"Total Expenses: {budget.CalculateTotalExpenses():C}");
                        break;

                    case "5":
                        Console.WriteLine($"Balance: {budget.CalculateBalance():C}");
                        break;

                    case "6":
                        Console.Write("Enter category to filter: ");
                        string filterCategory = Console.ReadLine();
                        budget.ViewTransactionsByCategory(filterCategory);
                        break;

                    case "7":
                        Console.Write("Enter start date (yyyy-mm-dd): ");
                        DateTime startDate;
                        while (!DateTime.TryParse(Console.ReadLine(), out startDate))
                        {
                            Console.Write("Please enter a valid start date: ");
                        }
                        Console.Write("Enter end date (yyyy-mm-dd): ");
                        DateTime endDate;
                        while (!DateTime.TryParse(Console.ReadLine(), out endDate))
                        {
                            Console.Write("Please enter a valid end date: ");
                        }
                        budget.ViewTransactionsByDateRange(startDate, endDate);
                        break;

                    case "8":
                        Console.Write("Enter minimum amount: ");
                        decimal minAmount;
                        while (!decimal.TryParse(Console.ReadLine(), out minAmount) || minAmount < 0)
                        {
                            Console.Write("Please enter a valid minimum amount: ");
                        }
                        Console.Write("Enter maximum amount: ");
                        decimal maxAmount;
                        while (!decimal.TryParse(Console.ReadLine(), out maxAmount) || maxAmount < minAmount)
                        {
                            Console.Write("Please enter a valid maximum amount: ");
                        }
                        budget.ViewTransactionsByAmount(minAmount, maxAmount);
                        break;

                    case "9":
                        Console.Write("Enter file path to save transactions: ");
                        string savePath = Console.ReadLine();
                        budget.SaveTransactions(savePath);
                        break;

                    case "10":
                        Console.Write("Enter file path to load transactions: ");
                        string loadPath = Console.ReadLine();
                        budget.LoadTransactions(loadPath);
                        break;

                    case "11":
                        Console.Write("Enter search term: ");
                        string searchTerm = Console.ReadLine();
                        budget.SearchTransactions(searchTerm);
                        break;

                    case "12":
                        budget.GenerateReport();
                        break;

                    case "13":
                        budget.ViewUniqueCategories();
                        break;

                    case "14":
                        Console.Write("Enter transaction description to delete: ");
                        string deleteDescription = Console.ReadLine();
                        budget.DeleteTransaction(deleteDescription);
                        break;

                    case "15":
                        Console.Write("Enter transaction description to edit: ");
                        string editDescription = Console.ReadLine();
                        Console.Write("Enter new amount: ");
                        decimal newAmount;
                        while (!decimal.TryParse(Console.ReadLine(), out newAmount) || newAmount <= 0)
                        {
                            Console.Write("Please enter a valid new amount: ");
                        }
                        Console.Write("Is this a new expense? (y/n): ");
                        bool newIsExpense = Console.ReadLine()?.ToLower() == "y";
                        Console.Write("Enter new category: ");
                        string newCategory = Console.ReadLine();
                        budget.EditTransaction(editDescription, newAmount, newIsExpense, newCategory);
                        break;

                    case "16":
                        Console.Write("Enter month (1-12): ");
                        int month;
                        while (!int.TryParse(Console.ReadLine(), out month) || month < 1 || month > 12)
                        {
                            Console.Write("Please enter a valid month (1-12): ");
                        }
                        Console.Write("Enter year: ");
                        int year;
                        while (!int.TryParse(Console.ReadLine(), out year))
                        {
                            Console.Write("Please enter a valid year: ");
                        }
                        budget.GenerateMonthlyReport(month, year);
                        break;

                    case "17":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }
    }
}
