using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFitnessTracker
{
    public class User
    {
        public string Username { get; }
        public string Password { get; }
        public int Age { get; }
        public double Weight { get; set; }
        public double Height { get; }
        public List<Workout> Workouts { get; }
        public List<Meal> Meals { get; }
        public List<Goal> Goals { get; }
        public double DailyCaloricGoal { get; set; }
        public double DailyProteinGoal { get; set; }
        public double DailyCarbsGoal { get; set; }
        public double DailyFatsGoal { get; set; }

        public User(string username, string password, int age, double weight, double height)
        {
            Username = username;
            Password = password;
            Age = age;
            Weight = weight;
            Height = height;
            Workouts = new List<Workout>();
            Meals = new List<Meal>();
            Goals = new List<Goal>();
            DailyCaloricGoal = 2000; // Default goal
            DailyProteinGoal = 150;   // Default goal
            DailyCarbsGoal = 250;     // Default goal
            DailyFatsGoal = 70;       // Default goal
        }

        public void LogWorkout(Workout workout)
        {
            Workouts.Add(workout);
            Console.WriteLine($"Workout logged: {workout}");
        }

        public void LogMeal(Meal meal)
        {
            Meals.Add(meal);
            Console.WriteLine($"Meal logged: {meal}");
        }

        public void SetGoal(Goal goal)
        {
            Goals.Add(goal);
            Console.WriteLine($"Goal set: {goal}");
        }

        public void ViewProgress()
        {
            Console.WriteLine($"Progress for {Username}:");
            foreach (var workout in Workouts)
            {
                Console.WriteLine(workout);
            }
            foreach (var meal in Meals)
            {
                Console.WriteLine(meal);
            }
            foreach (var goal in Goals)
            {
                Console.WriteLine(goal);
            }
            ShowNutritionalStatus();
        }

        public void ViewHistory(DateTime startDate, DateTime endDate)
        {
            Console.WriteLine($"Workout History for {Username} from {startDate.ToShortDateString()} to {endDate.ToShortDateString()}:");
            foreach (var workout in Workouts.Where(w => w.Date >= startDate && w.Date <= endDate))
            {
                Console.WriteLine(workout);
            }

            Console.WriteLine($"Meal History for {Username} from {startDate.ToShortDateString()} to {endDate.ToShortDateString()}:");
            foreach (var meal in Meals.Where(m => m.Date >= startDate && m.Date <= endDate))
            {
                Console.WriteLine(meal);
            }
        }

        public double DailyCaloricIntake()
        {
            return Meals.Sum(m => m.Calories);
        }

        public void UpdateProfile(double weight)
        {
            Weight = weight;
            Console.WriteLine($"Profile updated: {Username}, new weight is {Weight} kg.");
        }

        public void ShowNutritionalStatus()
        {
            double totalCalories = DailyCaloricIntake();
            double totalProtein = Meals.Sum(m => m.Protein);
            double totalCarbs = Meals.Sum(m => m.Carbs);
            double totalFats = Meals.Sum(m => m.Fats);

            Console.WriteLine($"Nutritional Status for {Username}:");
            Console.WriteLine($"Calories consumed: {totalCalories}/{DailyCaloricGoal}");
            Console.WriteLine($"Protein consumed: {totalProtein}/{DailyProteinGoal}g");
            Console.WriteLine($"Carbs consumed: {totalCarbs}/{DailyCarbsGoal}g");
            Console.WriteLine($"Fats consumed: {totalFats}/{DailyFatsGoal}g");
        }

        public void ShowGoalSummary()
        {
            int completed = Goals.Count(g => g.IsCompleted);
            int totalGoals = Goals.Count;
            Console.WriteLine($"Goal Summary for {Username}: {completed}/{totalGoals} goals completed.");
        }
    }

    public class Workout
    {
        public string Type { get; }
        public double Duration { get; } // in minutes
        public double CaloriesBurned { get; }
        public string Intensity { get; }
        public DateTime Date { get; }

        public Workout(string type, double duration, double caloriesBurned, string intensity)
        {
            Type = type;
            Duration = duration;
            CaloriesBurned = caloriesBurned;
            Intensity = intensity;
            Date = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Type} for {Duration} minutes, burned {CaloriesBurned} calories (Intensity: {Intensity}) on {Date.ToShortDateString()}.";
        }
    }

    public class Meal
    {
        public string Name { get; }
        public double Calories { get; }
        public double Protein { get; }
        public double Carbs { get; }
        public double Fats { get; }
        public DateTime Date { get; }

        public Meal(string name, double calories, double protein, double carbs, double fats)
        {
            Name = name;
            Calories = calories;
            Protein = protein;
            Carbs = carbs;
            Fats = fats;
            Date = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Name}: {Calories} calories, {Protein}g protein, {Carbs}g carbs, {Fats}g fats on {Date.ToShortDateString()}.";
        }
    }

    public class Goal
    {
        public string Description { get; }
        public DateTime Deadline { get; }
        public bool IsCompleted { get; private set; }

        public Goal(string description, DateTime deadline)
        {
            Description = description;
            Deadline = deadline;
            IsCompleted = false;
        }

        public void Complete()
        {
            IsCompleted = true;
            Console.WriteLine($"Goal completed: {Description}");
        }

        public override string ToString()
        {
            return $"{Description} by {Deadline.ToShortDateString()} - {(IsCompleted ? "Completed" : "Not Completed")}";
        }
    }

    public class FitnessApp
    {
        private List<User> users;

        public FitnessApp()
        {
            users = new List<User>();
        }

        public void RegisterUser(string username, string password, int age, double weight, double height)
        {
            var user = new User(username, password, age, weight, height);
            users.Add(user);
            Console.WriteLine($"User registered: {username}");
        }

        public User Login(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                Console.WriteLine($"Welcome back, {username}!");
                return user;
            }
            else
            {
                Console.WriteLine("Invalid username or password.");
                return null;
            }
        }

        public void DisplayUsers()
        {
            Console.WriteLine("Registered Users:");
            foreach (var user in users)
            {
                Console.WriteLine(user.Username);
            }
        }

        public void ShowWorkoutTypes()
        {
            Console.WriteLine("Available Workout Types:");
            Console.WriteLine("1. Cardio");
            Console.WriteLine("2. Strength Training");
            Console.WriteLine("3. Flexibility");
            Console.WriteLine("4. Balance");
            Console.WriteLine("5. High-Intensity Interval Training (HIIT)");
        }

        public void ShowMealSuggestions()
        {
            Console.WriteLine("Meal Suggestions:");
            Console.WriteLine("1. Grilled Chicken Salad - 350 calories");
            Console.WriteLine("2. Quinoa and Black Beans - 400 calories");
            Console.WriteLine("3. Greek Yogurt with Berries - 200 calories");
            Console.WriteLine("4. Smoothie Bowl - 300 calories");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            FitnessApp fitnessApp = new FitnessApp();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nPersonal Fitness Tracker");
                Console.WriteLine("1. Register User");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Display Registered Users");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Enter username: ");
                        string username = Console.ReadLine();
                        Console.Write("Enter password: ");
                        string password = Console.ReadLine();
                        Console.Write("Enter age: ");
                        int age = int.Parse(Console.ReadLine());
                        Console.Write("Enter weight (kg): ");
                        double weight = double.Parse(Console.ReadLine());
                        Console.Write("Enter height (cm): ");
                        double height = double.Parse(Console.ReadLine());
                        fitnessApp.RegisterUser(username, password, age, weight, height);
                        break;

                    case "2":
                        Console.Write("Enter username: ");
                        string loginUsername = Console.ReadLine();
                        Console.Write("Enter password: ");
                        string loginPassword = Console.ReadLine();
                        User loggedInUser = fitnessApp.Login(loginUsername, loginPassword);
                        if (loggedInUser != null)
                        {
                            bool userSession = true;
                            while (userSession)
                            {
                                Console.WriteLine("\nUser Menu");
                                Console.WriteLine("1. Log Workout");
                                Console.WriteLine("2. Log Meal");
                                Console.WriteLine("3. Set Goal");
                                Console.WriteLine("4. View Progress");
                                Console.WriteLine("5. View History");
                                Console.WriteLine("6. Update Profile");
                                Console.WriteLine("7. Show Goal Summary");
                                Console.WriteLine("8. Show Nutritional Goals");
                                Console.WriteLine("9. Logout");
                                Console.Write("Choose an option: ");

                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        fitnessApp.ShowWorkoutTypes();
                                        Console.Write("Enter workout type: ");
                                        string workoutType = Console.ReadLine();
                                        Console.Write("Enter duration (minutes): ");
                                        double duration = double.Parse(Console.ReadLine());
                                        Console.Write("Enter calories burned: ");
                                        double caloriesBurned = double.Parse(Console.ReadLine());
                                        Console.Write("Enter intensity (Low/Medium/High): ");
                                        string intensity = Console.ReadLine();
                                        loggedInUser.LogWorkout(new Workout(workoutType, duration, caloriesBurned, intensity));
                                        break;

                                    case "2":
                                        fitnessApp.ShowMealSuggestions();
                                        Console.Write("Enter meal name: ");
                                        string mealName = Console.ReadLine();
                                        Console.Write("Enter calories: ");
                                        double mealCalories = double.Parse(Console.ReadLine());
                                        Console.Write("Enter protein (g): ");
                                        double protein = double.Parse(Console.ReadLine());
                                        Console.Write("Enter carbs (g): ");
                                        double carbs = double.Parse(Console.ReadLine());
                                        Console.Write("Enter fats (g): ");
                                        double fats = double.Parse(Console.ReadLine());
                                        loggedInUser.LogMeal(new Meal(mealName, mealCalories, protein, carbs, fats));
                                        break;

                                    case "3":
                                        Console.Write("Enter goal description: ");
                                        string goalDescription = Console.ReadLine();
                                        Console.Write("Enter deadline (yyyy-mm-dd): ");
                                        DateTime deadline = DateTime.Parse(Console.ReadLine());
                                        loggedInUser.SetGoal(new Goal(goalDescription, deadline));
                                        break;

                                    case "4":
                                        loggedInUser.ViewProgress();
                                        break;

                                    case "5":
                                        Console.Write("Enter start date (yyyy-mm-dd): ");
                                        DateTime startDate = DateTime.Parse(Console.ReadLine());
                                        Console.Write("Enter end date (yyyy-mm-dd): ");
                                        DateTime endDate = DateTime.Parse(Console.ReadLine());
                                        loggedInUser.ViewHistory(startDate, endDate);
                                        break;

                                    case "6":
                                        Console.Write("Enter new weight (kg): ");
                                        double newWeight = double.Parse(Console.ReadLine());
                                        loggedInUser.UpdateProfile(newWeight);
                                        break;

                                    case "7":
                                        loggedInUser.ShowGoalSummary();
                                        break;

                                    case "8":
                                        Console.Write("Enter new daily caloric goal: ");
                                        loggedInUser.DailyCaloricGoal = double.Parse(Console.ReadLine());
                                        Console.Write("Enter new daily protein goal: ");
                                        loggedInUser.DailyProteinGoal = double.Parse(Console.ReadLine());
                                        Console.Write("Enter new daily carbs goal: ");
                                        loggedInUser.DailyCarbsGoal = double.Parse(Console.ReadLine());
                                        Console.Write("Enter new daily fats goal: ");
                                        loggedInUser.DailyFatsGoal = double.Parse(Console.ReadLine());
                                        Console.WriteLine("Nutritional goals updated!");
                                        break;

                                    case "9":
                                        userSession = false;
                                        Console.WriteLine("Logged out.");
                                        break;

                                    default:
                                        Console.WriteLine("Invalid option, please try again.");
                                        break;
                                }
                            }
                        }
                        break;

                    case "3":
                        fitnessApp.DisplayUsers();
                        break;

                    case "4":
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
