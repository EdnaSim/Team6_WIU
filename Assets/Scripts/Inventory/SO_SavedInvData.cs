using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_SavedInvData : ScriptableObject
{
    public List<SavedInvSlot> SavedInvSlots;
    public List<RangedWeaponStats> SavedRangedWeapons;
    public List<MeleeWeaponStats> SavedMeleeWeapons;
    public Item EquippedArmour;

    public void ClearSavedInvSlots() {
        SavedInvSlots.Clear();
    }
    public void ClearSavedRangedList() {
        SavedRangedWeapons.Clear();
    }
    public void ClearSavedMeleeList() {
        SavedMeleeWeapons.Clear();
    }
}
