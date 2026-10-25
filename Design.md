# Design

## 1. Saved recipe collection

In RecipeManager, I chose to use a `HashSet<int>` to store the IDs of user-favorite recipes instead of storing complete Recipe objects.  
The reason for choosing HashSet is as follows:  
- Prevents duplicates: HashSet does not allow duplicate IDs, ensuring that the same recipe cannot be saved multiple times.  
- Adding favorites: `AddSavedRecipe()` first checks whether the recipe exists using `_recipes.ContainsKey()`, then adds it to the collection via HashSet's Add() method.  
- Removing favorites: RemoveSavedRecipe() uses Remove() to delete a specified ID and returns whether the removal was successful.  
- Checking favorites: IsRecipeSaved() uses Contains() to determine if a given recipe ID has already been saved.  
- Performance efficiency: HashSet offers average time complexity of O(1) for insertion, deletion, and lookup operations, making it ideal for this favorite functionality.
  
Additionally, HashSet does not guarantee element ordering, but since the assignment does not require returning IDs in the order they were saved, this characteristic does not affect functionality.

## 2. Integration

In Part B, instead of building a new recipe management system from scratch, I extended the functionality of the original `RecipeManager`. B1 (`SearchByTitle`) and B2 (`SearchByIngredient`) retrieve existing recipes from `_recipes.Values` and search by title and ingredients, respectively. B3 (`GetHighestProteinRecipes`) uses the existing `Nutrition` data in each recipe to filter and rank recipes by protein content. For B4, the saved recipe feature validates each recipe ID against the original dictionary using `_recipes.ContainsKey()` before storing the ID in `_savedRecipes`.

## 3. Basic complexity


| Operation | Structure | Expected complexity | Reason |
| --- | --- | --- | --- |
| Lookup recipe by ID | Dictionary | Average O(1) | Hash-based key lookup. |
| Traverse cooking plan | LinkedList | O(n) | Each planned recipe may need to be visited. |
| Complete next instruction | Queue | O(1) | The item at the front is removed. |
| LINQ title/ingredient search | Recipe collection | O(n) | Each recipe may need to be inspected. |
| Check whether a recipe is saved | `HashSet<int>` | Average O(1) | `_savedRecipes.Contains(recipeId)` uses hash-based lookup to check whether an ID is saved, usually without scanning all saved IDs. |