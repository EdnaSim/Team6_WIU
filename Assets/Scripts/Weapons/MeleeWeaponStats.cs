using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeleeWeaponStats {
    public float Damage;
    public float Cooldown;
    public float RadiusX; //ps: due to how the melee box is rotated, X looks like Y and vice versa
    public float RadiusY;
    public float StaminaCost;
    public bool isAOE;

    [HideInInspector] public float TimerForCooldown;

    public string WeaponName;

    public MeleeWeaponStats() {
        Damage = 10;
        Cooldown = 1;
        RadiusX = 1;
        RadiusY = 1;
        StaminaCost = 5;
        WeaponName = "DefaultName";
        isAOE = false;

        //fixed value
        TimerForCooldown = 0f;
    }

    public MeleeWeaponStats(MeleeWeaponStats copy) {
        Damage = copy.Damage;
        Cooldown = copy.Cooldown;
        RadiusX = copy.RadiusX;
        RadiusY = copy.RadiusY;
        StaminaCost = copy.StaminaCost;
        WeaponName = copy.WeaponName;
        isAOE = copy.isAOE;
    }
}
