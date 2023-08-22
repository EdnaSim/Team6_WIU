using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to store weapon and projectile stats
[System.Serializable]
public class RangedWeaponStats
{
    [Header("Standard")]
    public float Damage;
    public float FireRate; //cooldown
    public float StaminaCost;
    public float MaxRange;
    public float ProjSpeed;
    [Tooltip("Distance to start damage fall off")]
    public float FallOffDist; //dist to start damage falloff

    [Header("Ammo")]
    [Tooltip("Current ammo in the inventory")]
    public int TotalStoredAmmo;
    [Tooltip("Total ammo of this type that can be carried in inventory")]
    public int MaxAmmo;
    [Tooltip("Ammo currently in the magazine. Once this number reaches 0, need to reload. Not counted as in the inventory")]
    public int AmmoInTheMag;
    [Tooltip("How many bullets that can be fired before needing to reload")]
    public int AmmoPerMag;
    public float ReloadTime;

    [Header("Shotgun")]
    [Tooltip("In degrees. How fanned out all the bullets will be")]
    [Min(0)]public float Spread; //0 to 360 (deg)
    [Min(1)]public int ShotsPerFire;

    [Header("Explode on-hit")]
    public float AOE;
    [Tooltip("If False, the bullet does not deal damage, only the AOE blast will. If True, both the AOE and the bullet's collision does damage. Ignored if AOE <= 0")]
    public bool DoesProjDoInitialDamage;
    [Tooltip("Damage of the AOE blast. If DoesProjDoInitialDamage is false, the projectile will only do AOE damage")]
    public float AOEDamage;

    [Header("Misc")]
    public GameObject ProjPrefab;
    public string WeaponName;

    //constructor
    public RangedWeaponStats() {
        Damage = 10;
        FireRate = 1;
        StaminaCost = 5;
        MaxRange = 10;
        ProjSpeed = 10;
        FallOffDist = 5;

        TotalStoredAmmo = 0;
        MaxAmmo = 100;
        AmmoPerMag = 10;
        AmmoInTheMag = 0;
        ReloadTime = 1.5f;

        Spread = 0;
        ShotsPerFire = 1;

        AOE = 0;
        DoesProjDoInitialDamage = true;
        AOEDamage = 0;

        ProjPrefab = null;
        WeaponName = "DefaultName";
    }

    //constructor for a COPY of the weapon, so that modifying the instance wont change the base
    public RangedWeaponStats(RangedWeaponStats copy) {
        Damage = copy.Damage;
        FireRate = copy.FireRate;
        StaminaCost = copy.StaminaCost;
        MaxRange = copy.MaxRange;
        ProjSpeed = copy.ProjSpeed;
        FallOffDist = copy.FallOffDist;

        TotalStoredAmmo = copy.TotalStoredAmmo;
        MaxAmmo = copy.MaxAmmo;
        AmmoPerMag = copy.AmmoPerMag;
        AmmoInTheMag = copy.AmmoInTheMag;
        ReloadTime = copy.ReloadTime;

        Spread = copy.Spread;
        ShotsPerFire = copy.ShotsPerFire;

        AOE = copy.AOE;
        DoesProjDoInitialDamage = copy.DoesProjDoInitialDamage;
        AOEDamage = copy.AOEDamage;

        ProjPrefab = copy.ProjPrefab;
        WeaponName = copy.WeaponName;
    }
}