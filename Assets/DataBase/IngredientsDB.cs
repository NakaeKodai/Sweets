using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IngredientsDB : ScriptableObject
{
    public List<Ingredients> ingredientsList = new List<Ingredients>();
}
