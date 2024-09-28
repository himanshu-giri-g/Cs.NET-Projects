using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MovieRentalSystem
{
    public class Movie
    {
        public int MovieId { get; }
        public string Title { get; }
        public string Genre { get; }
        public bool IsAvailable { get; private set; }

        public Movie(int movieId, string title, string genre)
        {
            MovieId = movieId;
            Title = title;
            Genre = genre;
            IsAvailable = true;
        }

        public void Rent() => IsAvailable = false;
        public void Return() => IsAvailable = true;

        public override string ToString()
        {
            return $"{MovieId}: {Title} ({Genre}) - {(IsAvailable ? "Available" : "Rented")}";
        }
    }

    public class Customer
    {
        public int CustomerId { get; }
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }

        public Customer(int customerId, string name, string phoneNumber)
        {
            CustomerId = customerId;
            Name = name;
            PhoneNumber = phoneNumber;
        }

        public void UpdateInfo(string name, string phoneNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
        }

        public override string ToString()
        {
            return $"{CustomerId}: {Name} - {PhoneNumber}";
        }
    }

    public class Rental
    {
        public Customer Customer { get; }
        public Movie Movie { get; }
        public DateTime RentalDate { get; }
        public DateTime DueDate { get; }
        public bool IsReturned { get; private set; }

        public Rental(Customer customer, Movie movie, DateTime rentalDate, DateTime dueDate)
        {
            Customer = customer;
            Movie = movie;
            RentalDate = rentalDate;
            DueDate = dueDate;
            IsReturned = false;
        }

        public void Return()
        {
            IsReturned = true;
        }

        public double CalculateLateFee()
        {
            if (IsReturned)
                return 0;

            var overdueDays = (DateTime.Now - DueDate).Days;
            return overdueDays > 0 ? overdueDays * 1.5 : 0; // $1.50 per day late
        }

        public override string ToString()
        {
            return $"Rental: {Customer.Name} rented '{Movie.Title}' on {RentalDate.ToShortDateString()} (Due: {DueDate.ToShortDateString()})" +
                   $"{(IsReturned ? " - Returned" : "")}";
        }
    }

    public class MovieRentalStore
    {
        private List<Movie> movies;
        private List<Customer> customers;
        private List<Rental> rentals;

        public MovieRentalStore()
        {
            movies = new List<Movie>();
            customers = new List<Customer>();
            rentals = new List<Rental>();
        }

        public void AddMovie(int movieId, string title, string genre)
        {
            var movie = new Movie(movieId, title, genre);
            movies.Add(movie);
            Console.WriteLine($"Movie added: {movie}");
        }

        public void RegisterCustomer(int customerId, string name, string phoneNumber)
        {
            var customer = new Customer(customerId, name, phoneNumber);
            customers.Add(customer);
            Console.WriteLine($"Customer registered: {customer}");
        }

        public void RentMovie(int customerId, int movieId)
        {
            var customer = customers.FirstOrDefault(c => c.CustomerId == customerId);
            var movie = movies.FirstOrDefault(m => m.MovieId == movieId && m.IsAvailable);
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }
            if (movie == null)
            {
                Console.WriteLine("Movie not available for rent.");
                return;
            }

            movie.Rent();
            var rental = new Rental(customer, movie, DateTime.Now, DateTime.Now.AddDays(7));
            rentals.Add(rental);
            Console.WriteLine($"Movie rented: {rental}");
        }

        public void ReturnMovie(int rentalId)
        {
            var rental = rentals.ElementAtOrDefault(rentalId - 1);
            if (rental == null)
            {
                Console.WriteLine("Rental not found.");
                return;
            }

            rental.Movie.Return();
            rental.Return();
            double lateFee = rental.CalculateLateFee();
            if (lateFee > 0)
            {
                Console.WriteLine($"Movie returned: {rental.Movie.Title}. Late fee: ${lateFee:F2}");
            }
            else
            {
                Console.WriteLine($"Movie returned: {rental.Movie.Title}");
            }
            rentals.Remove(rental);
        }

        public void DisplayMovies()
        {
            Console.WriteLine("Available Movies:");
            foreach (var movie in movies)
            {
                Console.WriteLine(movie);
            }
        }

        public void DisplayCustomers()
        {
            Console.WriteLine("Registered Customers:");
            foreach (var customer in customers)
            {
                Console.WriteLine(customer);
            }
        }

        public void DisplayRentals()
        {
            Console.WriteLine("Current Rentals:");
            foreach (var rental in rentals)
            {
                Console.WriteLine(rental);
            }
        }

        public void GenerateReport()
        {
            Console.WriteLine("Movie Rental Report:");
            Console.WriteLine($"Total Movies: {movies.Count}");
            Console.WriteLine($"Total Customers: {customers.Count}");
            Console.WriteLine($"Total Rentals: {rentals.Count}");
        }

        public void SearchMovies(string title = null, string genre = null)
        {
            var foundMovies = movies.AsEnumerable();
            if (!string.IsNullOrEmpty(title))
            {
                foundMovies = foundMovies.Where(m => m.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(genre))
            {
                foundMovies = foundMovies.Where(m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
            }

            Console.WriteLine("Search Results:");
            foreach (var movie in foundMovies)
            {
                Console.WriteLine(movie);
            }
        }

        public void UpdateCustomerInfo(int customerId, string newName, string newPhoneNumber)
        {
            var customer = customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer != null)
            {
                customer.UpdateInfo(newName, newPhoneNumber);
                Console.WriteLine($"Updated customer info: {customer}");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        public void DisplayRentalHistory(int customerId)
        {
            var customerRentals = rentals.Where(r => r.Customer.CustomerId == customerId).ToList();
            if (!customerRentals.Any())
            {
                Console.WriteLine("No rental history found for this customer.");
                return;
            }

            Console.WriteLine($"Rental History for Customer {customerId}:");
            foreach (var rental in customerRentals)
            {
                Console.WriteLine(rental);
            }
        }

        public void FilterMoviesByGenre(string genre)
        {
            var filteredMovies = movies.Where(m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
            Console.WriteLine($"Movies in Genre '{genre}':");
            foreach (var movie in filteredMovies)
            {
                Console.WriteLine(movie);
            }
        }

        public void CountAvailableMovies()
        {
            int availableCount = movies.Count(m => m.IsAvailable);
            Console.WriteLine($"Total Available Movies: {availableCount}");
        }

        public void ExportRentalReport(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Movie Rental Report:");
                writer.WriteLine($"Total Movies: {movies.Count}");
                writer.WriteLine($"Total Customers: {customers.Count}");
                writer.WriteLine($"Total Rentals: {rentals.Count}");
                writer.WriteLine("Rental Details:");

                foreach (var rental in rentals)
                {
                    writer.WriteLine($"{rental.Customer.Name} rented '{rental.Movie.Title}' on {rental.RentalDate.ToShortDateString()} (Due: {rental.DueDate.ToShortDateString()})");
                }
            }
            Console.WriteLine("Rental report exported successfully.");
        }

        public void FindMovieById(int movieId)
        {
            var movie = movies.FirstOrDefault(m => m.MovieId == movieId);
            if (movie != null)
            {
                Console.WriteLine($"Found Movie: {movie}");
            }
            else
            {
                Console.WriteLine("Movie not found.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MovieRentalStore store = new MovieRentalStore();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nMovie Rental System");
                Console.WriteLine("1. Add Movie");
                Console.WriteLine("2. Register Customer");
                Console.WriteLine("3. Rent Movie");
                Console.WriteLine("4. Return Movie");
                Console.WriteLine("5. Display Movies");
                Console.WriteLine("6. Display Customers");
                Console.WriteLine("7. Display Rentals");
                Console.WriteLine("8. Generate Report");
                Console.WriteLine("9. Search Movies");
                Console.WriteLine("10. Update Customer Info");
                Console.WriteLine("11. Display Rental History");
                Console.WriteLine("12. Filter Movies by Genre");
                Console.WriteLine("13. Count Available Movies");
                Console.WriteLine("14. Export Rental Report");
                Console.WriteLine("15. Find Movie by ID");
                Console.WriteLine("16. Exit");
                Console.Write("Choose an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Enter movie ID: ");
                        int movieId = int.Parse(Console.ReadLine());
                        Console.Write("Enter movie title: ");
                        string title = Console.ReadLine();
                        Console.Write("Enter movie genre: ");
                        string genre = Console.ReadLine();
                        store.AddMovie(movieId, title, genre);
                        break;

                    case "2":
                        Console.Write("Enter customer ID: ");
                        int customerId = int.Parse(Console.ReadLine());
                        Console.Write("Enter customer name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter phone number: ");
                        string phoneNumber = Console.ReadLine();
                        store.RegisterCustomer(customerId, name, phoneNumber);
                        break;

                    case "3":
                        Console.Write("Enter customer ID: ");
                        int rentCustomerId = int.Parse(Console.ReadLine());
                        Console.Write("Enter movie ID: ");
                        int rentMovieId = int.Parse(Console.ReadLine());
                        store.RentMovie(rentCustomerId, rentMovieId);
                        break;

                    case "4":
                        Console.Write("Enter rental ID to return: ");
                        int rentalId = int.Parse(Console.ReadLine());
                        store.ReturnMovie(rentalId);
                        break;

                    case "5":
                        store.DisplayMovies();
                        break;

                    case "6":
                        store.DisplayCustomers();
                        break;

                    case "7":
                        store.DisplayRentals();
                        break;

                    case "8":
                        store.GenerateReport();
                        break;

                    case "9":
                        Console.Write("Enter title or genre to search: ");
                        string searchTerm = Console.ReadLine();
                        store.SearchMovies(searchTerm);
                        break;

                    case "10":
                        Console.Write("Enter customer ID to update: ");
                        int updateCustomerId = int.Parse(Console.ReadLine());
                        Console.Write("Enter new name: ");
                        string newName = Console.ReadLine();
                        Console.Write("Enter new phone number: ");
                        string newPhoneNumber = Console.ReadLine();
                        store.UpdateCustomerInfo(updateCustomerId, newName, newPhoneNumber);
                        break;

                    case "11":
                        Console.Write("Enter customer ID to view rental history: ");
                        int historyCustomerId = int.Parse(Console.ReadLine());
                        store.DisplayRentalHistory(historyCustomerId);
                        break;

                    case "12":
                        Console.Write("Enter genre to filter: ");
                        string filterGenre = Console.ReadLine();
                        store.FilterMoviesByGenre(filterGenre);
                        break;

                    case "13":
                        store.CountAvailableMovies();
                        break;

                    case "14":
                        Console.Write("Enter file path to export report: ");
                        string filePath = Console.ReadLine();
                        store.ExportRentalReport(filePath);
                        break;

                    case "15":
                        Console.Write("Enter movie ID to find: ");
                        int findMovieId = int.Parse(Console.ReadLine());
                        store.FindMovieById(findMovieId);
                        break;

                    case "16":
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
