using System.Collections.Generic;
using RecipeManagement.Core;

namespace RecipeManagement.Tests;

/// <summary>
/// Example tests from the assignment specification. Add your own tests as you work.
/// </summary>
public sealed class RecipeManagerTests
{
    [Fact]
    public void Constructor_BuildsRecipeDictionary()
    {
        var manager = CreateManager();
        Assert.Equal(2, manager.RecipeCount);
        Assert.Equal("Recipe A", manager.FindRecipe(10)?.Title);
    }

    [Fact]
    public void InstructionsAreCompletedInFileOrder()
    {
        var manager = CreateManager();
        Assert.True(manager.StartCooking(10));
        Assert.Equal("First step", manager.PeekNextInstruction());
        Assert.Equal("First step", manager.CompleteNextInstruction());
        Assert.Equal("Second step", manager.PeekNextInstruction());
    }

    [Fact]
    public void RemovedRecipesAreRestoredLastInFirstOut()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);
        manager.RemoveRecipeFromCookingPlan(10);
        manager.RemoveRecipeFromCookingPlan(20);
        Assert.Equal(20, manager.PeekLastRemovedRecipe());
        Assert.True(manager.RestoreLastRemovedRecipe());
        Assert.Equal(new[] { 20 }, manager.GetCookingPlan());
    }

    private static RecipeManager CreateManager()
    {
        return new RecipeManager(new[]
        {
            new Recipe
            {
                Id = 10,
                Title = "Recipe A",
                Ingredients = new() { "1 apple" },
                Instructions = new() { "First step", "Second step" }
            },
            new Recipe
            {
                Id = 20,
                Title = "Recipe B"
            }
        });
    }

    [Fact]
    public void AddRecipe_ReturnsFalseForDuplicateId()
    {
        var manager = CreateManager();
        var duplicateRecipe = new Recipe
        {
            Id = 10,
            Title = "Duplicate Recipe"
        };
        bool result = manager.AddRecipe(duplicateRecipe);

        Assert.False(result);
        Assert.Equal(2, manager.RecipeCount);
    }

    [Fact]
    public void FindRecipe_ReturnsNullForMissingId()
    {
        var manager = CreateManager();
        var result = manager.FindRecipe(999);

        Assert.Null(result);
    }

    [Fact]
    public void PeekLastRemovedRecipe_ReturnsNullWhenStackIsEmpty()
    {
        var manager = CreateManager();
        var result = manager.PeekLastRemovedRecipe();

        Assert.Null(result);
    }

    [Fact]
    public void PeekNextInstruction_ReturnsNullWhenQueueIsEmpty()
    {
        var manager = CreateManager();
        var result = manager.PeekNextInstruction();

        Assert.Null(result);
    }

    [Fact]
    public void AddRecipeToCookingPlan_ReturnsFalseForDuplicateRecipe()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        var result = manager.AddRecipeToCookingPlan(10);

        Assert.False(result);

        Assert.Equal(1, manager.CookingPlanCount);
    }

    [Fact]
    public void AddIngredientsToShoppingList_AddsRecipeIngredients()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(10);

        Assert.Equal(1, manager.ShoppingItemCount);
    }

    [Fact]
    public void GetShoppingList_ReturnsRecipeIngredients()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(10);
        var shoppingList = manager.GetShoppingList();

        Assert.Equal("1 apple", shoppingList[0]);
    }

    [Fact]
    public void ClearShoppingList_RemovesAllItems()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(10);
        manager.ClearShoppingList();

        Assert.Equal(0, manager.ShoppingItemCount);
    }

    [Fact]
    public void CompleteNextInstruction_ReturnsNullWhenQueueIsEmpty()
    {
        var manager = CreateManager();
        var result = manager.CompleteNextInstruction();

        Assert.Null(result);
    }

    [Fact]
    public void GetCookingPlan_ReturnsRecipesInAddedOrder()
    {
        var manager = CreateManager();

        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);

        var cookingPlan = manager.GetCookingPlan();

        Assert.Equal(10, cookingPlan[0]);
        Assert.Equal(20, cookingPlan[1]);
    }

    [Fact]
    public void RemoveRecipe_RemovesExistingRecipe()
    {
        var manager = CreateManager();

        manager.RemoveRecipe(10);

        Assert.Equal(1, manager.RecipeCount);

        var result = manager.FindRecipe(10);

        Assert.Null(result);
    }

    [Fact]
    public void AddRecipe_AddsNewRecipe()
    {
        var manager = CreateManager();
        var newRecipe = new Recipe { Id = 30, Title = "Recipe C" };

        manager.AddRecipe(newRecipe);

        Assert.Equal(3, manager.RecipeCount);
    }

    [Fact]
    public void AddRecipe_CanFindAddedRecipe()
    {
        var manager = CreateManager();
        var newRecipe = new Recipe
        {
            Id = 30,
            Title = "Recipe C"
        };

        manager.AddRecipe(newRecipe);
        var result = manager.FindRecipe(30);

        Assert.Equal("Recipe C", result?.Title);
    }

    [Fact]
    public void AddIngredientsToShoppingList_ReturnsZeroForMissingRecipe()
    {
        var manager = CreateManager();
        var result = manager.AddIngredientsToShoppingList(999);

        Assert.Equal(0, result);
        Assert.Equal(0, manager.ShoppingItemCount);
    }

    [Fact]
    public void RemoveRecipeFromCookingPlan_ReturnsFalseWhenRecipeNotInPlan()
    {
        var manager = CreateManager();
        var result = manager.RemoveRecipeFromCookingPlan(10);

        Assert.False(result);
        Assert.Equal(0, manager.CookingPlanCount);
    }

    [Fact]
    public void RestoreLastRemovedRecipe_ReturnsFalseWhenStackIsEmpty()
    {
        var manager = CreateManager();
        var result = manager.RestoreLastRemovedRecipe();

        Assert.False(result);
    }

    [Fact]
    public void StartCooking_ReturnsFalseForMissingRecipe()
    {
        var manager = CreateManager();
        var result = manager.StartCooking(999);

        Assert.False(result);
        Assert.Equal(0, manager.PendingInstructionCount);
    }

    [Fact]
    public void StartCooking_LoadsAllInstructionsIntoQueue()
    {
        var manager = CreateManager();
        var result = manager.StartCooking(10);

        Assert.Equal(2, manager.PendingInstructionCount);
    }

    
}
