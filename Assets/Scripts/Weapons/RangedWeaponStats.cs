using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to store weapon and projectile stats
[System.Serializable]
public class RangedWeaponStats
{
    public float Damage;
    public float FireRate; //cooldown
    public float MaxRange;
    public float ProjSpeed;
    public float FallOffDist; //dist to start damage falloff

    [Min(0)]public float Spread; //0 to 360 (deg)
    [Min(1)]public int ShotsPerFire;

    public GameObject ProjPrefab;
    public string WeaponName;

    //constructor
    public RangedWeaponStats() {
        Damage = 10;
        FireRate = 1;
        MaxRange = 10;
        ProjSpeed = 10;
        FallOffDist = 5;
        Spread = 0;
        ShotsPerFire = 1;
        ProjPrefab = null;
        WeaponName = "DefaultName";
    }

    //constructor for a COPY of the weapon, so that modifying the instance wont change the base
    public RangedWeaponStats(RangedWeaponStats copy) {
        Damage = copy.Damage;
        FireRate = copy.FireRate;
        MaxRange = copy.MaxRange;
        ProjSpeed = copy.ProjSpeed;
        FallOffDist = copy.FallOffDist;
        Spread = copy.Spread;
        ShotsPerFire = copy.ShotsPerFire;
        ProjPrefab = copy.ProjPrefab;
        WeaponName = copy.WeaponName;
    }
}
