using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//just to record all types of weapons
[CreateAssetMenu]
public class SO_WeaponList : ScriptableObject
{
    public List<RangedWeaponData> RangedWeaponList;
    public List<MeleeWeaponData> MeleeWeaponlist;
}
