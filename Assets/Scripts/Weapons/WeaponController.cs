using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//weapon class that holds what proj it fires
public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance;
    public GameObject owner;
    public SO_WeaponList WeaponList;
    public SO_SavedInvData InvData;

    [Header("Projectile")]
    public RangedWeaponStats RangedStats;
    public static List<RangedWeaponStats> OwnedRangedList;

    [Header("Melee")]
    public MeleeWeaponStats MeleeStats;
    public static List<MeleeWeaponStats> OwnedMeleeList;

    [Header("UI")]
    [SerializeField] TMP_Text ReloadingText;
    [SerializeField] TMP_Text NoAmmoText;
    float ReloadFlashTimer = 0;

    [Header("Weapon Audio")]
    public AudioClip[] GunSFX;
    private AudioSource audiosrc;

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        if (owner == null) {
            owner = gameObject;
        }
        OwnedRangedList = new List<RangedWeaponStats>();
        OwnedMeleeList = new List<MeleeWeaponStats>();
        LoadLists();

        ReloadingText.enabled = false;
        NoAmmoText.enabled = false;

        for(int i = 0; i < GunSFX.Length; i++)
        {
            GunSFX[i] = GetComponent<AudioClip>();
        }
        audiosrc = GetComponent<AudioSource>();
    }

    private void Update() {
        if (InventoryManager.Instance.getAmmo() > 0 || RangedStats.AmmoInTheMag > 0) 
            NoAmmoText.enabled = false;
        else if (InventoryManager.Instance.getAmmo() <= 0 && RangedStats.AmmoInTheMag <= 0 && RangedStats.ProjPrefab != null)
            NoAmmoText.enabled = true;

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
        else if (ReloadingText.enabled) {
            ReloadingText.enabled = false;
            ReloadFlashTimer = 0f;
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
        for (int i = 0; i < OwnedRangedList.Count; i++) {
            if (OwnedRangedList[i].TimerForFireRate > 0)
                OwnedRangedList[i].TimerForFireRate -= Time.deltaTime;
        }
        for (int i = 0; i < OwnedMeleeList.Count; i++) {
            if (OwnedMeleeList[i].TimerForCooldown > 0)
                OwnedMeleeList[i].TimerForCooldown -= Time.deltaTime;
        }
    }

    public bool CanRanged() {
        if (RangedStats == null 
            || RangedStats.ProjPrefab == null 
            || RangedStats.TimerForFireRate > 0f 
            || RangedStats.Reloading 
            || (RangedStats.AmmoInTheMag == 0 && InventoryManager.Instance.getAmmo() <= 0))
            return false;

        return true;
    }

    public bool Fire(Vector2 DirToFire) {
        if (!CanRanged()) return false;

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
                float x = RangedStats.Spread - ((RangedStats.Spread*2)/RangedStats.ShotsPerFire) * i;
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
        if (InventoryManager.Instance.getAmmo() <= 0 || !RangedStats.CanReload) {
            NoAmmoText.enabled = true;
            return;
        }
        if (!RangedStats.Reloading) {
            if(InventoryManager.Instance.getSelected().name == "Shotgun")
            {
                if (!audiosrc.isPlaying)
                {
                    audiosrc.clip = GunSFX[5];
                    //audiosrc.Play();
                    Debug.Log(audiosrc.clip);
                }
            }
            RangedStats.Reloading = true;
            ReloadingText.enabled = true;
            RangedStats.TimerForReload = RangedStats.ReloadTime;
        }
    }

    private void FinishReload() {
        int diff = RangedStats.AmmoPerMag - RangedStats.AmmoInTheMag;
        if (diff > 0) {
            //when stored ammo runs out, it just puts in all the bullets it can, then stops
            for (int i = 0; i < diff; i++) {
                if (InventoryManager.Instance.getAmmo() > 0) {
                    InventoryManager.Instance.RemoveAmmo(1);
                    RangedStats.AmmoInTheMag++;
                }
                else {
                    break;
                }
            }
        }
        ReloadingText.enabled = false;
        ReloadFlashTimer = 0f;
        RangedStats.Reloading = false;
    }

    public bool CanMelee() {
        if (MeleeStats == null || MeleeStats.TimerForCooldown > 0)
            return false;

        return true;
    }

    public bool Melee(Vector2 origin, Vector2 dir) {
        if (!CanMelee()) return false;

        MeleeStats.TimerForCooldown = MeleeStats.Cooldown;
        LayerMask lm = 1 << owner.gameObject.layer;//get all layers
        lm = ~lm;//reverse, so everything EXCEPT owner
        //remove layers that should not be scanned
        lm -= LayerMask.GetMask("UI", "Ignore Raycast", "TransparentFX", "Water", "Default");
        if (!MeleeStats.isAOE) {
            Collider2D col = Physics2D.OverlapBox(origin, new Vector2(MeleeStats.RadiusX, MeleeStats.RadiusY), Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, lm);
            if (col == null)
                return true;

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

        if (OwnedRangedList.Contains(newWeaponStat)) {
            //TODO: check if newData exists in the player's inventory (not this temp one)
            //BaseData = newData;
         
            //TODO: use stats of the weapon instance from inventory
            RangedStats = newWeaponStat;
        }
    }

    public void ChangeMeleeWeapon(MeleeWeaponStats newWeaponStat) {
        if (newWeaponStat == null)
            return;

        if (OwnedMeleeList.Contains(newWeaponStat)) {
            MeleeStats = newWeaponStat;
        }
    }

    public void AddRangedWeapon(RangedWeaponStats stat) {
        //add copy
        OwnedRangedList.Add(new RangedWeaponStats(stat));
    }
    public void AddMeleeWeapon(MeleeWeaponStats stat) {
        OwnedMeleeList.Add(new MeleeWeaponStats(stat));
    }

    public void SaveLists() {
        InvData.SavedRangedWeapons = OwnedRangedList;
        InvData.SavedMeleeWeapons = OwnedMeleeList;
        InvData.EquippedArmour = ArmourDetails.Instance.EquippedArmour;
    }

    public void LoadLists() {
        OwnedRangedList = InvData.SavedRangedWeapons;
        OwnedMeleeList = InvData.SavedMeleeWeapons;
        ArmourDetails.Instance.EquipArmour(InvData.EquippedArmour);
    }
}
