using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    public Item firstMaterial;
    public Item secondMaterial;
    public Item finalMaterial;

    public int amtOfFirst;
    public int amtOfSecond;
    public int amtOfFinal;
}
