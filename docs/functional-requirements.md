# Functional Requirements

## Product Goal

BadMelon helps users manage a shared food pantry through recipes. Users should be able to choose recipes, understand which ingredients they already have, generate a shopping list for what is missing, and update the pantry as food is purchased or consumed.

## Users

- A household user who plans meals and shops for groceries.
- A family member who shares pantry, recipe, meal plan, and shopping list data with other invited users.
- A user who wants to reduce duplicate purchases and food waste.
- A user who wants a practical pantry system without turning grocery tracking into a chore.

## Must Have

### Authentication

- Users can create an account with email and password.
- Users can sign in with email and password.
- Users can sign up and sign in with Google SSO.
- Users can sign out.
- Users can reset a forgotten password.
- Users can view and update basic profile information.

### Family Sharing

- Users can create a family.
- Users can invite other users to join their family.
- Invited users can accept or decline a family invitation.
- Family members can share pantry items, recipes, meal plans, and shopping lists.
- Family membership controls access to shared data.
- A family has at least one owner or administrator who can manage invitations and membership.

### Recipe Management

- Users can create, view, edit, and delete recipes.
- A recipe includes a name, ingredients, quantities, units, and preparation notes.
- Users can mark recipes as active meal plans or add them to a shopping plan.
- Users can search or filter recipes by name and ingredient.

### Pantry Management

- Users can add, view, edit, and remove pantry items.
- Pantry items include a name, quantity, unit, and optional notes.
- Pantry quantities support loose amounts, such as "some", "half a carton", or "mostly full".
- Pantry quantities optionally support precise amounts, such as subtracting exactly 100 mL from 2 L of milk.
- Users can update the pantry after buying groceries.
- Users can reduce pantry quantities when recipes are cooked.
- Users can see when an ingredient required by a recipe is already available.

### Shopping List

- Users can add meal plans to the shopping list.
- When meal plans are added, the shopping list includes only ingredients that still need to be bought after accounting for available pantry items.
- The shopping list is a user-managed list populated when items are added, not a live view that constantly changes with meal plans or pantry state.
- Users can recalculate or re-add meal plan items when they want to refresh the list.
- Users can manually add, edit, check off, and remove shopping list items.
- Users can add one-off shopping list items that are not related to recipes, such as household goods or personal errands.
- Users can mark shopping list items as purchased and move them into the pantry.
- Users can mark non-pantry items as purchased without adding them to the pantry.

### Ingredient Matching

- The system can match recipe ingredients against pantry items.
- Matching should handle simple name differences where practical, such as pluralization or casing.
- Users can correct or confirm ingredient matches when the system is uncertain.
- Version one uses simple automatic matching plus user-confirmed corrections.

### Basic Data Persistence

- User-created recipes, pantry items, meal plans, family membership, and shopping lists persist between sessions.
- The system protects users from accidental data loss during normal use.

## Should Have

### Meal Planning

- Users can group recipes into a weekly or short-term meal plan.
- Users can add the whole meal plan to the shopping list.
- Users can remove a planned recipe and recalculate missing ingredients.

### Pantry Quality

- Users can add optional expiration dates to pantry items.
- Users can see items that are expired or nearing expiration.
- Users can prioritize recipes that use pantry items before they expire.

### Unit Handling

- The system supports common grocery units such as grams, kilograms, ounces, pounds, milliliters, liters, cups, tablespoons, teaspoons, pieces, cans, packages, and items.
- The system can convert compatible units where conversion is reliable.
- The system avoids unsafe or misleading conversions when ingredient density or package size is unknown.

### Recipe Import

- Users can paste or enter recipe text and convert it into structured ingredients.
- Users can review imported ingredients before saving a recipe.
- URL-based recipe import is not required for the first rewrite.

## Could Have

- Pantry categories such as produce, dairy, frozen, spices, and dry goods.
- Store aisle ordering for shopping lists.
- Favorite recipes.
- Recipe tags such as vegetarian, quick, freezer-friendly, or breakfast.
- Cost estimation for planned groceries.
- Barcode scanning for pantry entry.
- Notifications for low-stock or expiring items.
- URL-based recipe import.

## Initial Screens

- Dashboard showing planned recipes, shopping list status, and pantry highlights.
- Sign up and sign in screens.
- Family invite and membership screens.
- Recipes list.
- Recipe detail and edit form.
- Pantry list.
- Pantry item edit form.
- Shopping list.
- Meal plan view.

## Core Workflows

### Add a Recipe

1. User creates a recipe.
2. User enters ingredients, quantities, and units.
3. User saves the recipe.
4. Recipe is available for planning and shopping.

### Add a Meal Plan to the Shopping List

1. User selects a meal plan.
2. System calculates required ingredients for the planned recipes.
3. System subtracts matching pantry items.
4. System adds missing ingredients to the shopping list.
5. User reviews and edits the shopping list.

### Add a One-Off Shopping Item

1. User opens the shopping list.
2. User adds an item that is not tied to a recipe.
3. User optionally marks whether the item should be added to the pantry after purchase.
4. Item appears alongside meal-plan-derived shopping items.

### Buy Groceries

1. User checks off shopping list items while shopping.
2. User marks checked items as purchased.
3. Purchased quantities are added to the pantry.
4. Shopping list is updated or cleared.

### Cook a Recipe

1. User selects a recipe to cook.
2. System shows pantry items that will be consumed.
3. User confirms the action.
4. Pantry quantities are reduced.

## Open Questions

- What family roles are needed for version one: owner/member only, or owner/admin/member?
- Should recipes belong to a family by default, or can users also keep private recipes?
