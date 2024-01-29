using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Crafting/Recipe")]
public class Recipe : ScriptableObject
{
    public List<Ingredient> recipeIngredients;
    public Ingredient resultIngredient;
}
