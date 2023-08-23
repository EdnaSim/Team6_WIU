using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemStats : ScriptableObject
{
    public Item item;
    public enum statsType
    { 
        health,
        stamina,
        sanity
    }
    public statsType statstype;
    public int statsAmount;
}
