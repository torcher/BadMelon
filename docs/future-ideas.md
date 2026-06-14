# Future Ideas

This document captures product ideas that are intentionally outside the first rewrite. These ideas should not block the design system, API, or frontend work, but they should influence architecture where doing so is cheap and sensible.

## AI-Assisted Ingredient Intelligence

- Suggest ingredient substitutions when a recipe item is missing from the pantry.
- Explain substitution confidence and tradeoffs in plain language.
- Learn from user-confirmed ingredient matches and substitutions.
- Detect when pantry items are similar but not exact, such as "Greek yogurt" and "sour cream".
- Avoid making automatic substitutions without user confirmation.

## What Can I Make?

- Let users search for recipes they can make from the current pantry.
- Show recipes that are fully available.
- Show recipes that are almost available, with a short list of missing items.
- Suggest reasonable substitutions for missing ingredients when available.
- Rank results by pantry fit, expiration urgency, prep time, and family preferences.

## Smarter Meal Planning

- Recommend meal plans that use expiring pantry items.
- Suggest recipes that share ingredients to reduce waste and shopping cost.
- Support family preferences, allergies, dietary goals, and disliked ingredients.

## Shopping Intelligence

- Suggest store aisle grouping.
- Estimate grocery cost.
- Remember frequently purchased one-off items.
- Suggest likely forgotten items based on past shopping behavior.

## Recipe Import And Parsing

- Import recipes from URLs.
- Parse pasted recipe text into structured ingredients and steps.
- Normalize imported ingredients while preserving original wording.

## Nutrition Module

- Estimate nutrition information for recipes and meal plans.
- Show approximate calories, macronutrients, sodium, fiber, sugar, and other common nutrition facts.
- Let users set optional nutrition goals or preferences.
- Highlight recipes that fit selected nutrition preferences.
- Support serving-size adjustments and show how nutrition estimates change.
- Preserve original recipe data while allowing nutrition data to be corrected or overridden.
- Make nutrition estimates clearly approximate unless verified against authoritative data.

## Guardrails

- AI suggestions should be optional and reviewable.
- Food allergy and dietary information should be treated as sensitive.
- The system should avoid presenting substitutions as medically or nutritionally authoritative.
- Nutrition features should avoid medical advice and should not replace guidance from a qualified professional.
- The first implementation should preserve enough structured data to support future intelligence without requiring a data model rewrite.
