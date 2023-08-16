using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//weapon class that holds what proj it fires
public class WeaponController : MonoBehaviour
{
    public GameObject owner;

    [Header("Projectile")]
    [SerializeField] RangedWeaponData BaseRangedData; //starting weapon type, DO NOT MODIFY BASEDATA.STATS
    public RangedWeaponStats RangedStats; //MODIFY THIS

    [Header("Melee")]
    [SerializeField] MeleeWeaponData BaseMeleeData;
    public MeleeWeaponStats MeleeStats;

    private void Awake() {
        if (owner == null) {
            owner = gameObject;
        }
        //TEMP
        Player_Controller.TempInventory = new List<RangedWeaponStats>();
        Player_Controller.TempMeleeInv = new List<MeleeWeaponStats>();
        //get stats from the base weapon type, as a copy.
        if (BaseRangedData != null) {
            RangedStats = new RangedWeaponStats(BaseRangedData.Stats);
            Player_Controller.TempInventory.Add(RangedStats);
        }
        if (BaseMeleeData != null) {
            MeleeStats = new MeleeWeaponStats(BaseMeleeData.Stats);
            Player_Controller.TempMeleeInv.Add(MeleeStats);
        }
    }

    public bool Fire(Vector2 DirToFire) {
        if (RangedStats == null || RangedStats.ProjPrefab == null)
            return false;

        for (int i = 0; i < RangedStats.ShotsPerFire; i++) {
            //instantiate projectile
            Projectile temp = Instantiate(RangedStats.ProjPrefab).GetComponent<Projectile>();
            //set variables from WeaponController/ Base Data
            temp.caster = owner;
            temp.LinkedWeapon = this;
            LayerMask lm = 1 << owner.gameObject.layer;//get all layers
            lm = ~lm;//reverse, so everything EXCEPT owner
            //remove layers that should not be scanned
            lm -= LayerMask.GetMask("UI", "Ignore Raycast", "TransparentFX", "Water");
            temp.targetLayer = lm;

            temp.damage = RangedStats.Damage;
            temp.speed = RangedStats.ProjSpeed;
            temp.MaxRange = RangedStats.MaxRange;
            temp.FalloffDist = RangedStats.FallOffDist;
            //Shotgun spread (if any)
            if (RangedStats.Spread > 0) {
                //spread out the dir of the bullets (they will move out in a cone)
                float x = RangedStats.Spread - ((RangedStats.Spread*2)/RangedStats.ShotsPerFire)*i;
                float y = RangedStats.Spread - ((RangedStats.Spread*2) / RangedStats.ShotsPerFire) * i;
                temp.dir = new Vector2(DirToFire.x + x/100, DirToFire.y + y/100).normalized;
            }
            else {
                temp.dir = DirToFire;
            }
        }

        return true;
    }

    public bool Melee(Vector2 origin, Vector2 dir) {
        if (MeleeStats == null)
            return false;

        LayerMask lm = 1 << owner.gameObject.layer;//get all layers
        lm = ~lm;//reverse, so everything EXCEPT owner
        //remove layers that should not be scanned
        lm -= LayerMask.GetMask("UI", "Ignore Raycast", "TransparentFX", "Water", "Default");
        if (!MeleeStats.isAOE) {
            Collider2D col = Physics2D.OverlapBox(origin, new Vector2(MeleeStats.RadiusX, MeleeStats.RadiusY), Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, lm);
            if (col == null)
                return false;

            HealthManager hm = col.gameObject.GetComponent<HealthManager>();
            if (hm != null) {
                hm.TakeDamage(MeleeStats.Damage, owner);
            }
        }
        else {
            foreach (Collider2D col in Physics2D.OverlapBoxAll(origin, new Vector2(MeleeStats.RadiusX, MeleeStats.RadiusY), Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, lm)) {
                HealthManager hm = col.gameObject.GetComponent<HealthManager>();
                if (hm != null) {
                    hm.TakeDamage(MeleeStats.Damage, owner);
                }
            }
        }

        return true;
    }

    public void ChangeRangedWeapon(RangedWeaponStats newWeaponStat) {
        if (newWeaponStat == null)
            return;

        if (Player_Controller.TempInventory.Contains(newWeaponStat)) {
            //TODO: check if newData exists in the player's inventory (not this temp one)
            //BaseData = newData;

            //TODO: use stats of the weapon instance from inventory
            RangedStats = newWeaponStat;
        }
    }

    public void ChangeMeleeWeapon(MeleeWeaponStats newWeaponStat) {
        if (newWeaponStat == null)
            return;

        if (Player_Controller.TempMeleeInv.Contains(newWeaponStat)) {
            MeleeStats = newWeaponStat;
        }
    }

    //TEMP, should be controlled by inventory
    public void AddWeapon(RangedWeaponStats stat) {
        //add copy
        Player_Controller.TempInventory.Add(new RangedWeaponStats(stat));
    }
}
