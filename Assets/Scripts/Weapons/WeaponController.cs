using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//weapon class that holds what proj it fires
public class WeaponController : MonoBehaviour
{
    public GameObject owner;
    public SO_WeaponList WeaponList;
    float TimeSinceSwitch = 0f;

    [Header("Projectile")]
    [SerializeField] RangedWeaponData BaseRangedData; //starting weapon type, DO NOT MODIFY BASEDATA.STATS
    public RangedWeaponStats RangedStats; //MODIFY THIS

    [Header("Melee")]
    [SerializeField] MeleeWeaponData BaseMeleeData;
    public MeleeWeaponStats MeleeStats;

    [Header("UI")]
    [SerializeField] TMP_Text ReloadingText;
    [SerializeField] TMP_Text NoAmmoText;
    float ReloadFlashTimer = 0;

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

        ReloadingText.enabled = false;
        NoAmmoText.enabled = false;
    }

    public bool Fire(Vector2 DirToFire) {
        if (RangedStats == null || RangedStats.ProjPrefab == null || RangedStats.TimerForFireRate > 0f)
            return false;

        //check if have ammo. (if no, reload)
        if (RangedStats.AmmoInTheMag <= 0) {
            Reload();
            return false;
        }

        RangedStats.AmmoInTheMag--;
        for (int i = 0; i < RangedStats.ShotsPerFire; i++) {
            //instantiate projectile
            Projectile temp = Instantiate(RangedStats.ProjPrefab).GetComponent<Projectile>();
            //set variables from WeaponController/ Base Data
            temp.caster = owner;
            temp.LinkedWeapon = this;
            LayerMask lm = 1 << owner.gameObject.layer;//get all layers
            lm = ~lm;//reverse, so everything EXCEPT owner
            //remove layers that should not be scanned
            lm -= LayerMask.GetMask("UI", "Ignore Raycast", "TransparentFX", "Water", "Default");
            temp.targetLayer = lm;

            temp.damage = RangedStats.Damage;
            temp.speed = RangedStats.ProjSpeed;
            temp.MaxRange = RangedStats.MaxRange;
            temp.FalloffDist = RangedStats.FallOffDist;

            temp.AOE = RangedStats.AOE;
            temp.AOEDamage = RangedStats.AOEDamage;
            temp.DoesBulletDoDamage = RangedStats.DoesProjDoInitialDamage;
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
        //auto reload after firing, if no more ammo in the mag
        if (RangedStats.AmmoInTheMag <= 0) {
            Reload();
        }
        RangedStats.TimerForFireRate = RangedStats.FireRate;

        return true;
    }

    public void Reload() {
        //no more ammo in inventory, or weapon CanReload is set to false
        if (RangedStats.TotalStoredAmmo <= 0 || !RangedStats.CanReload) { //TEMP: USE GETAMMO FROM INV INSTEAD OF TOTALSTOREAMMO
            NoAmmoText.enabled = true;
            return;
        }
        if (!RangedStats.Reloading) {
            RangedStats.Reloading = true;
            ReloadingText.enabled = true;
            RangedStats.TimerForReload = RangedStats.ReloadTime;
        }
    }

    private void Update() {
        if (NoAmmoText.enabled && RangedStats.TotalStoredAmmo > 0) //TEMP: USE GETAMMO FROM INV INSTEAD OF TOTALSTOREAMMO
            NoAmmoText.enabled = false;
        if (RangedStats.Reloading) {
            ReloadFlashTimer += Time.deltaTime;
            if (ReloadFlashTimer >= 0.2f) {
                ReloadingText.enabled = true;
            }
            if (ReloadFlashTimer > 0.4f) {
                ReloadingText.enabled = false;
                ReloadFlashTimer = 0f;
            }
        }

        //reload individual guns (pauses when not equipped)
        if (RangedStats.Reloading) {
            if (RangedStats.TimerForReload > 0) {
                RangedStats.TimerForReload -= Time.deltaTime;
            }
            else {
                FinishReload();
            }
        }

        //cooldown
        if (RangedStats.TimerForFireRate > 0) {
            RangedStats.TimerForFireRate -= Time.deltaTime;
        }
        if (MeleeStats.TimerForCooldown > 0) {
            MeleeStats.TimerForCooldown -= Time.deltaTime;
        }

        TimeSinceSwitch += Time.deltaTime;
    }

    private void FinishReload() {
        int diff = RangedStats.AmmoPerMag - RangedStats.AmmoInTheMag;
        if (diff > 0) {
            //have enough to reload something
            if (RangedStats.TotalStoredAmmo > 0) {
                //when stored ammo runs out, it just puts in all the bullets it can, then stops
                for (int i = 0; i < diff; i++) {
                    if (RangedStats.TotalStoredAmmo > 0) { //TEMP: USE GETAMMO FROM INV INSTEAD OF TOTALSTOREAMMO
                        RangedStats.TotalStoredAmmo--; //TEMP: USE GETAMMO FROM INV INSTEAD OF TOTALSTOREAMMO
                        RangedStats.AmmoInTheMag++;
                    }
                }
            }
        }
        ReloadingText.enabled = false;
        ReloadFlashTimer = 0f;
        RangedStats.Reloading = false;
    }

    public int Melee(Vector2 origin, Vector2 dir) {
        if (MeleeStats == null || MeleeStats.TimerForCooldown > 0)
            return 0;

        int EnemiesHit = 0;
        MeleeStats.TimerForCooldown = MeleeStats.Cooldown;
        LayerMask lm = 1 << owner.gameObject.layer;//get all layers
        lm = ~lm;//reverse, so everything EXCEPT owner
        //remove layers that should not be scanned
        lm -= LayerMask.GetMask("UI", "Ignore Raycast", "TransparentFX", "Water", "Default");
        if (!MeleeStats.isAOE) {
            Collider2D col = Physics2D.OverlapBox(origin, new Vector2(MeleeStats.RadiusX, MeleeStats.RadiusY), Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, lm);
            if (col == null)
                return 0;

            HealthManager hm = col.gameObject.GetComponent<HealthManager>();
            if (hm != null) {
                hm.TakeDamage(MeleeStats.Damage, owner);
                EnemiesHit++;
            }
        }
        else {
            foreach (Collider2D col in Physics2D.OverlapBoxAll(origin, new Vector2(MeleeStats.RadiusX, MeleeStats.RadiusY), Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, lm)) {
                HealthManager hm = col.gameObject.GetComponent<HealthManager>();
                if (hm != null) {
                    hm.TakeDamage(MeleeStats.Damage, owner);
                    EnemiesHit++;
                }
            }
        }
        
        return EnemiesHit;
    }

    public void ChangeRangedWeapon(RangedWeaponStats newWeaponStat) {
        if (newWeaponStat == null)
            return;

        if (Player_Controller.TempInventory.Contains(newWeaponStat)) {
            //TODO: check if newData exists in the player's inventory (not this temp one)
            //BaseData = newData;
         
            //TODO: use stats of the weapon instance from inventory
            RangedStats = newWeaponStat;
            //decrease fire rate based on time since last weapon switch. weapons "cool down" while inactive
            RangedStats.TimerForFireRate -= TimeSinceSwitch;
            TimeSinceSwitch = 0f;
        }
    }

    public void ChangeMeleeWeapon(MeleeWeaponStats newWeaponStat) {
        if (newWeaponStat == null)
            return;

        if (Player_Controller.TempMeleeInv.Contains(newWeaponStat)) {
            MeleeStats = newWeaponStat;
            //decrease fire rate based on time since last weapon switch. weapons "cool down" while inactive
            MeleeStats.TimerForCooldown -= TimeSinceSwitch;
            TimeSinceSwitch = 0f;
        }
    }

    //TEMP, should be controlled by inventory
    public void AddWeapon(RangedWeaponStats stat) {
        //add copy
        Player_Controller.TempInventory.Add(new RangedWeaponStats(stat));
    }
}
