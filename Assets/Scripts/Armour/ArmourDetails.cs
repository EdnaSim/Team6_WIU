using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourDetails : MonoBehaviour
{
    public static ArmourDetails Instance;

    [Header("Damage Reduction")]
    public float LeatherVestDR;
    public float CivilianVestDR;
    public float PoliceVestDR;
    public float ArmyVestDR;

    [HideInInspector] public Item EquippedArmour;
    private float EquippedDR; //to avoid another switch case in Unequip

    private void Awake() {
        Instance = this;
    }
    public void EquipArmour(Item armour) {
        EquippedArmour = armour;

        switch (armour.itemName) {
            case "Leather Vest":
            Player_HealthManager.Instance.ChangeDamageMultiplier(LeatherVestDR);
            EquippedDR = LeatherVestDR;
            break;

            case "Civilian Vest":
            Player_HealthManager.Instance.ChangeDamageMultiplier(CivilianVestDR);
            EquippedDR = CivilianVestDR;
            break;

            case "Police Vest":
            Player_HealthManager.Instance.ChangeDamageMultiplier(PoliceVestDR);
            EquippedDR = PoliceVestDR;
            break;

            case "Army Vest":
            Player_HealthManager.Instance.ChangeDamageMultiplier(ArmyVestDR);
            EquippedDR = ArmyVestDR;
            break;

            default:
            Debug.LogError("Cant find armour name: \"" + armour.name + "\"");
            break;
        }
    }

    public void UnequipArmour() {
        Player_HealthManager.Instance.ChangeDamageMultiplier(-EquippedDR);
        EquippedArmour = null;
    }
}
