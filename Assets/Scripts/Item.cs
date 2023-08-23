using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{

    public enum itemType
    {
        ores,
        resouces,
        food,
        melee,
        ranged,
        equipment
    }
    public string itemName;
    public itemType type;
    public Sprite image;
    [TextArea]
    public string description;
    public int amount;
    public bool stackable = true;

}
