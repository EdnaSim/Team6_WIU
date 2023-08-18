using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SO to create new weapon types
[CreateAssetMenu]
public class RangedWeaponData : ScriptableObject {
    //hold a reference to the weapon stats
    [SerializeField] RangedWeaponStats stats;
    public RangedWeaponStats Stats => stats;
}
