using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    //Variables to save
    public static float MaxHp;
    public static float CurrHP;
    //Sanity
    public static float MaxSanity;
    public static float CurrSanity;
    //Energy/Stamina
    public static float MaxStamina;
    public static float CurrStamina;

    private void Awake() {
        MaxHp = 0;
        CurrHP = 0;
        MaxSanity = 0;
        CurrSanity = 0;
        MaxStamina = 0;
        CurrStamina = 0;
    }
}
