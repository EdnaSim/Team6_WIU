using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    GameObject player;
    //Variables to save
    public static float MaxHp;
    public static float CurrHP;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        MaxHp = 0;
        CurrHP = 0;
    }
}
