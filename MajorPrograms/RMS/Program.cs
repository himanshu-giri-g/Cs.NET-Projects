using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecipeManagement
{
    public class Recipe
    {
        public string Name { get; }
        public List<string> Ingredients { get; }
        public string Instructions { get; }
        public string Category { get; }
        private List<int> ratings; // Change to private
        public string NutritionalInfo { get; }
        public bool IsFavorite { get; set; }

        public Recipe(string name, List<string> ingredients, string instructions, string category, string nutritionalInfo)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Ingredients = ingredients ?? throw new ArgumentNullException(nameof(ingredients));
            Instructions = instructions ?? throw new ArgumentNullException(nameof(instructions));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            ratings = new List<int>();
            NutritionalInfo = nutritionalInfo ?? throw new ArgumentNullException(nameof(nutritionalInfo));
            IsFavorite = false;
        }

        public IReadOnlyList<int> Ratings => ratings.AsReadOnly(); // Expose as read-only
        public double AverageRating => ratings.Count > 0 ? ratings.Average() : 0;

        public void AddRating(int rating)
        {
            if (rating < 1 || rating > 5) throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");
            ratings.Add(rating);
        }

        public override string ToString()
        {
            return $"Recipe: {Name}\nCategory: {Category}\nIngredients: {string.Join(", ", Ingredients)}\n" +
                   $"Instructions: {Instructions}\nNutritional Info: {NutritionalInfo}\n" +
                   $"Average Rating: {AverageRating:F1}\nFavorite: {(IsFavorite ? "Yes" : "No")}";
        }
    }

    public class RecipeManager
    {
        private List<Recipe> recipes;

        public RecipeManager()
        {
            recipes = new List<Recipe>();
        }

        public void AddRecipe(string name, List<string> ingredients, string instructions, string category, string nutritionalInfo)
        {
            if (name == null || ingredients == null || instructions == null || category == null || nutritionalInfo == null)
                throw new ArgumentNullException("All parameters must be non-null.");
            recipes.Add(new Recipe(name, ingredients, instructions, category, nutritionalInfo));
            Console.WriteLine("Recipe added successfully.");
        }

        public void ViewRecipes()
        {
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes available.");
                return;
            }

            foreach (var recipe in recipes)
            {
                Console.WriteLine(recipe);
                Console.WriteLine();
            }
        }

        public void SearchRecipe(string searchTerm)
        {
            if (searchTerm == null) throw new ArgumentNullException(nameof(searchTerm));

            var filteredRecipes = recipes.Where(r => r.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            if (filteredRecipes.Count == 0)
            {
                Console.WriteLine($"No recipes found for search term: {searchTerm}");
            }
            else
            {
                foreach (var recipe in filteredRecipes)
                {
                    Console.WriteLine(recipe);
                    Console.WriteLine();
                }
            }
        }

        public void SearchByIngredient(string ingredient)
        {
            if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));

            var filteredRecipes = recipes.Where(r => r.Ingredients.Any(i => i.IndexOf(ingredient, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
            if (filteredRecipes.Count == 0)
            {
                Console.WriteLine($"No recipes found containing ingredient: {ingredient}");
            }
            else
            {
                foreach (var recipe in filteredRecipes)
                {
                    Console.WriteLine(recipe);
                    Console.WriteLine();
                }
            }
        }

        public void EditRecipe(string name, List<string> newIngredients, string newInstructions, string newCategory, string newNutritionalInfo)
        {
            if (name == null || newIngredients == null || newInstructions == null || newCategory == null || newNutritionalInfo == null)
                throw new ArgumentNullException("All parameters must be non-null.");

            var recipeToEdit = recipes.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (recipeToEdit != null)
            {
                recipes.Remove(recipeToEdit);
                recipes.Add(new Recipe(name, newIngredients, newInstructions, newCategory, newNutritionalInfo));
                Console.WriteLine($"Recipe '{name}' updated successfully.");
            }
            else
            {
                Console.WriteLine($"No recipe found with name: {name}");
            }
        }

        public void DeleteRecipe(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var recipeToDelete = recipes.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (recipeToDelete != null)
            {
                recipes.Remove(recipeToDelete);
                Console.WriteLine($"Recipe '{name}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"No recipe found with name: {name}");
            }
        }

        public void RateRecipe(string name, int rating)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var recipeToRate = recipes.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (recipeToRate != null)
            {
                recipeToRate.AddRating(rating);
                Console.WriteLine($"Recipe '{name}' rated successfully.");
            }
            else
            {
                Console.WriteLine($"No recipe found with name: {name}");
            }
        }

        public void MarkAsFavorite(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var recipeToFavorite = recipes.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (recipeToFavorite != null)
            {
                recipeToFavorite.IsFavorite = true;
                Console.WriteLine($"Recipe '{name}' marked as favorite.");
            }
            else
            {
                Console.WriteLine($"No recipe found with name: {name}");
            }
        }

        public void UnmarkFavorite(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var recipeToUnfavorite = recipes.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (recipeToUnfavorite != null)
            {
                recipeToUnfavorite.IsFavorite = false;
                Console.WriteLine($"Recipe '{name}' unmarked as favorite.");
            }
            else
            {
                Console.WriteLine($"No recipe found with name: {name}");
            }
        }

        public void SortRecipesByName()
        {
            var sortedRecipes = recipes.OrderBy(r => r.Name).ToList();
            Console.WriteLine("Recipes sorted by name:");
            foreach (var recipe in sortedRecipes)
            {
                Console.WriteLine(recipe.Name);
            }
        }

        public void SortRecipesByRating()
        {
            var sortedRecipes = recipes.OrderByDescending(r => r.AverageRating).ToList();
            Console.WriteLine("Recipes sorted by average rating:");
            foreach (var recipe in sortedRecipes)
            {
                Console.WriteLine($"{recipe.Name} - Average Rating: {recipe.AverageRating:F1}");
            }
        }

        public void SortRecipesByCategory()
        {
            var sortedRecipes = recipes.OrderBy(r => r.Category).ToList();
            Console.WriteLine("Recipes sorted by category:");
            foreach (var recipe in sortedRecipes)
            {
                Console.WriteLine(recipe.Name);
            }
        }

        public void PrintRecipe(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var recipeToPrint = recipes.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (recipeToPrint != null)
            {
                Console.WriteLine(recipeToPrint);
            }
            else
            {
                Console.WriteLine($"No recipe found with name: {name}");
            }
        }

        public void SaveRecipes(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var recipe in recipes)
                {
                    writer.WriteLine($"{recipe.Name}|{string.Join(",", recipe.Ingredients)}|{recipe.Instructions}|{recipe.Category}|{recipe.NutritionalInfo}|{string.Join(",", recipe.Ratings)}|{(recipe.IsFavorite ? "1" : "0")}");
                }
            }
            Console.WriteLine("Recipes saved successfully.");
        }

        public void LoadRecipes(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
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
                    if (parts.Length == 7)
                    {
                        string name = parts[0];
                        var ingredients = parts[1].Split(',').ToList();
                        string instructions = parts[2];
                        string category = parts[3];
                        string nutritionalInfo = parts[4];
                        var ratings = parts[5].Split(',').Select(int.Parse).ToList();
                        bool isFavorite = parts[6] == "1";

                        var recipe = new Recipe(name, ingredients, instructions, category, nutritionalInfo);
                        foreach (var rating in ratings)
                        {
                            recipe.AddRating(rating);
                        }
                        recipe.IsFavorite = isFavorite;
                        recipes.Add(recipe);
                    }
                }
            }
            Console.WriteLine("Recipes loaded successfully.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            RecipeManager recipeManager = new RecipeManager();
            bool running = true;

            while (running)
            {
                Console.WriteLine("1. Add Recipe");
                Console.WriteLine("2. View Recipes");
                Console.WriteLine("3. Search Recipe");
                Console.WriteLine("4. Edit Recipe");
                Console.WriteLine("5. Delete Recipe");
                Console.WriteLine("6. Rate Recipe");
                Console.WriteLine("7. Mark Recipe as Favorite");
                Console.WriteLine("8. Unmark Recipe as Favorite");
                Console.WriteLine("9. Search by Ingredient");
                Console.WriteLine("10. Sort Recipes by Name");
                Console.WriteLine("11. Sort Recipes by Rating");
                Console.WriteLine("12. Sort Recipes by Category");
                Console.WriteLine("13. Print Recipe");
                Console.WriteLine("14. Save Recipes to File");
                Console.WriteLine("15. Load Recipes from File");
                Console.WriteLine("16. Exit");
                Console.Write("Choose an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Enter recipe name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter ingredients (comma separated): ");
                        List<string> ingredients = Console.ReadLine()?.Split(',').Select(i => i.Trim()).ToList() ?? new List<string>();
                        Console.Write("Enter instructions: ");
                        string instructions = Console.ReadLine();
                        Console.Write("Enter category: ");
                        string category = Console.ReadLine();
                        Console.Write("Enter nutritional information: ");
                        string nutritionalInfo = Console.ReadLine();
                        recipeManager.AddRecipe(name, ingredients, instructions, category, nutritionalInfo);
                        break;

                    case "2":
                        recipeManager.ViewRecipes();
                        break;

                    case "3":
                        Console.Write("Enter search term: ");
                        string searchTerm = Console.ReadLine();
                        recipeManager.SearchRecipe(searchTerm);
                        break;

                    case "4":
                        Console.Write("Enter recipe name to edit: ");
                        string editName = Console.ReadLine();
                        Console.Write("Enter new ingredients (comma separated): ");
                        List<string> newIngredients = Console.ReadLine()?.Split(',').Select(i => i.Trim()).ToList() ?? new List<string>();
                        Console.Write("Enter new instructions: ");
                        string newInstructions = Console.ReadLine();
                        Console.Write("Enter new category: ");
                        string newCategory = Console.ReadLine();
                        Console.Write("Enter new nutritional information: ");
                        string newNutritionalInfo = Console.ReadLine();
                        recipeManager.EditRecipe(editName, newIngredients, newInstructions, newCategory, newNutritionalInfo);
                        break;

                    case "5":
                        Console.Write("Enter recipe name to delete: ");
                        string deleteName = Console.ReadLine();
                        recipeManager.DeleteRecipe(deleteName);
                        break;

                    case "6":
                        Console.Write("Enter recipe name to rate: ");
                        string rateName = Console.ReadLine();
                        Console.Write("Enter rating (1-5): ");
                        if (int.TryParse(Console.ReadLine(), out int rating))
                        {
                            recipeManager.RateRecipe(rateName, rating);
                        }
                        else
                        {
                            Console.WriteLine("Invalid rating.");
                        }
                        break;

                    case "7":
                        Console.Write("Enter recipe name to mark as favorite: ");
                        string favoriteName = Console.ReadLine();
                        recipeManager.MarkAsFavorite(favoriteName);
                        break;

                    case "8":
                        Console.Write("Enter recipe name to unmark as favorite: ");
                        string unfavoriteName = Console.ReadLine();
                        recipeManager.UnmarkFavorite(unfavoriteName);
                        break;

                    case "9":
                        Console.Write("Enter ingredient to search: ");
                        string ingredient = Console.ReadLine();
                        recipeManager.SearchByIngredient(ingredient);
                        break;

                    case "10":
                        recipeManager.SortRecipesByName();
                        break;

                    case "11":
                        recipeManager.SortRecipesByRating();
                        break;

                    case "12":
                        recipeManager.SortRecipesByCategory();
                        break;

                    case "13":
                        Console.Write("Enter recipe name to print: ");
                        string printName = Console.ReadLine();
                        recipeManager.PrintRecipe(printName);
                        break;

                    case "14":
                        Console.Write("Enter file path to save recipes: ");
                        string savePath = Console.ReadLine();
                        recipeManager.SaveRecipes(savePath);
                        break;

                    case "15":
                        Console.Write("Enter file path to load recipes: ");
                        string loadPath = Console.ReadLine();
                        recipeManager.LoadRecipes(loadPath);
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
