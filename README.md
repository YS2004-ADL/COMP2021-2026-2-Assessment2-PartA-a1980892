# Recipe Management System — Student Starter

Starter repository for Parts A and B. Implement `RecipeManager` in Core; the Application menu and JSON loader are supplied.

## What is supplied

- `RecipeManagement.Core/Models/` — recipe model classes
- `RecipeManagement.Core/RecipeLoader.cs` — reads `data/recipes.json`
- `RecipeManagement.Core/IRecipeManager.cs` — public API
- `RecipeManagement.Application/` — console menu (options labelled PartA / PartB)
- `RecipeManagement.Tests/` — example tests
- `data/recipes.json` — recipe dataset

## What you implement

**Part A** — `RecipeManager.cs` using:

- `Dictionary<int, Recipe>`
- `List<string>`
- `LinkedList<int>`
- `Stack<int>`
- `Queue<string>`

**Part B** — LINQ searches, protein report, saved-recipe collection, `Design.md`, and more tests.

## Build and run

Open `RecipeManagement.sln` from the repository root directory.

```bash
dotnet build
dotnet test
dotnet run --project RecipeManagement.Application -- data/recipes.json
```


## AI acknowledgement


AI acknowledgement:I used ChatGPT to help me understand the assignment requirements and review C# programming concepts.

For Part A, I reviewed Dictionary, List, LinkedList, Stack and Queue, their related methods, and how these collections interact within RecipeManager. I also used ChatGPT to help me understand xUnit testing, troubleshoot errors, review debugging approaches, and understand the Git and GitHub workflow.

For Part B, I used ChatGPT to help me understand how to implement case-insensitive title and ingredient searches using LINQ methods such as Where() and Any(). I also reviewed the LINQ methods covered in the course so that I could correctly implement the required functionality. In addition, I reviewed time complexity concepts to support my independent selection of data structures and my answers to the related complexity questions.

I did not copy or adapt AI-generated code or other material into my submission. I developed the submitted solution myself based on my understanding of the course material.