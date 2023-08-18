using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SO to create new weapon types
[CreateAssetMenu]
public class MeleeWeaponData : ScriptableObject {
    //hold a reference to the weapon stats
    [SerializeField] MeleeWeaponStats stats;
    public MeleeWeaponStats Stats => stats;
}
