using System;
using System.Collections.Generic;

namespace RecipeManagement.Core;

/// <summary>
/// Implement this class using the five Part A collections as private fields:
/// Dictionary&lt;int, Recipe&gt;, List&lt;string&gt;, LinkedList&lt;int&gt;,
/// Stack&lt;int&gt; and Queue&lt;string&gt;.
/// </summary>
public sealed class RecipeManager : IRecipeManager
{
    private readonly Dictionary<int, Recipe> _recipes = new Dictionary<int, Recipe>();
    private readonly List<string> _shoppingList = new List<string>();
    private readonly LinkedList<int> _cookingPlan = new LinkedList<int>();
    private readonly Stack<int> _removedRecipes = new Stack<int>();
    private readonly Queue<string> _instructions = new Queue<string>();
    private readonly HashSet<int> _savedRecipes = new HashSet<int>();

    public RecipeManager(IEnumerable<Recipe> recipes)
    {
        if (recipes == null)
        {
            throw new ArgumentNullException(nameof(recipes));
        }

        foreach (var recipe in recipes)
        {
            if (recipe.Id <= 0)
            {
                throw new ArgumentException("Recipe ID must be positive");
            }

            if (string.IsNullOrWhiteSpace(recipe.Title))
            {
                throw new ArgumentException("Title cannot be blank");
            }
            if (_recipes.ContainsKey(recipe.Id))
            {
                throw new ArgumentException("This recipe Id already exist");
            }

            _recipes.Add(recipe.Id, recipe);
        }
    }

    public int RecipeCount => _recipes.Count;
    public int ShoppingItemCount => _shoppingList.Count;
    public int CookingPlanCount => _cookingPlan.Count;
    public int PendingInstructionCount => _instructions.Count;
    public int RemovedRecipeCount => _removedRecipes.Count;

    public bool AddRecipe(Recipe recipe)
    {
        if (recipe == null)
        {
            throw new ArgumentNullException(nameof(recipe));
        }

        if (recipe.Id <= 0)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(recipe.Title))
        {
            return false;
        }

        if (_recipes.ContainsKey(recipe.Id))
        {
            return false;
        }

        _recipes.Add(recipe.Id, recipe);
        return true;
    }

    public Recipe? FindRecipe(int recipeId)
    {
        if (_recipes.TryGetValue(recipeId, out var recipe))
        {
            return recipe;
        }

        return null;
    }

    public bool RemoveRecipe(int recipeId)
    {
        if (!_recipes.ContainsKey(recipeId))
        {
            return false;
        }

        if (_cookingPlan.Contains(recipeId))
        {
            return false;
        }

        _recipes.Remove(recipeId);
        return true;
    }

    public int AddIngredientsToShoppingList(int recipeId)
    {
        if (!_recipes.TryGetValue(recipeId, out var recipe))
        {
            return 0;
        }

        _shoppingList.AddRange(recipe.Ingredients);

        return recipe.Ingredients.Count;
    }

    public IReadOnlyList<string> GetShoppingList()
    {
        return _shoppingList.ToList();
    }

    public void ClearShoppingList()
    {
        _shoppingList.Clear();
    }

    public bool AddRecipeToCookingPlan(int recipeId)
    {
        if (!_recipes.ContainsKey(recipeId))
        {
            return false;
        }

        if (_cookingPlan.Contains(recipeId))
        {
            return false;
        }

        _cookingPlan.AddLast(recipeId);
        return true;
    }

    public bool RemoveRecipeFromCookingPlan(int recipeId)
    {
        if (!_cookingPlan.Contains(recipeId))
        {
            return false;
        }
        else if (_cookingPlan.Contains(recipeId))
        {
            _cookingPlan.Remove(recipeId);
            _removedRecipes.Push(recipeId);
        }

        return true;
    }

    public bool RestoreLastRemovedRecipe()
    {
        if (_removedRecipes.Count == 0)
        {
            return false;
        }

        int recipeId = _removedRecipes.Pop();

        if (!_recipes.ContainsKey(recipeId))
        {
            return false;
        }

        if (_cookingPlan.Contains(recipeId))
        {
            return false;
        }

        _cookingPlan.AddLast(recipeId);

        return true;
    }

    public int? PeekLastRemovedRecipe()
    {
        if (_removedRecipes.Count == 0)
        {
            return null;
        }

        return _removedRecipes.Peek();
    }

    public IReadOnlyList<int> GetCookingPlan()
    {
        return _cookingPlan.ToList();
    }

    public bool StartCooking(int recipeId)
    {
        if (!_recipes.TryGetValue(recipeId, out var recipe))
        {
            return false;
        }

        if (recipe.Instructions.Count == 0)
        {
            return false;
        }

        _instructions.Clear();
        foreach (var instruction in recipe.Instructions)
        {
            _instructions.Enqueue(instruction);
        }

        return true;
    }

    public string? PeekNextInstruction()
    {
        if (_instructions.Count == 0)
        {
            return null;
        }

        return _instructions.Peek();
    }

    public string? CompleteNextInstruction()
    {
         if (_instructions.Count == 0)
        {
            return null;
        }

        return _instructions.Dequeue();
    }

// Part B
    public IReadOnlyList<Recipe> SearchByTitle(string searchText)
    {
        var result = _recipes.Values
            .Where(recipe => recipe.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return result;
    }
    public IReadOnlyList<Recipe> SearchByIngredient(string searchText)
    {
        var result = _recipes.Values
            .Where(recipe => recipe.Ingredients
            .Any(ingredient => ingredient.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        return result;
    }

    public IReadOnlyList<Recipe> GetHighestProteinRecipes(int count)
    {
        var result = _recipes.Values
            .Where(recipe => recipe.Nutrition?.ProteinG != null)
            .OrderByDescending(recipe => recipe.Nutrition?.ProteinG)
            .Take(count)
            .ToList();

        return result;
    }
    public bool AddSavedRecipe(int recipeId)
    {
        if (!_recipes.ContainsKey(recipeId))
        {
            return false;
        }
        
        return _savedRecipes.Add(recipeId);
    }

    public bool RemoveSavedRecipe(int recipeId)
    {
        return _savedRecipes.Remove(recipeId);
    }

    public bool IsRecipeSaved(int recipeId)
    {
        return _savedRecipes.Contains(recipeId);
    }

    public IReadOnlyList<int> GetSavedRecipes()
    {
        return _savedRecipes.ToList();
    }
}
