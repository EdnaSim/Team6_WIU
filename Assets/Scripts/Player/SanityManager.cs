using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour
{
    public static SanityManager Instance;

    [Header("Base stats")]
    [SerializeField] float Base_MaxSanity;

    [Tooltip("Amount of sanity drained by certain events")]
    public float DrainAmtOnStarve;
    public float DrainAmtOnHit;
    public float DrainAmtOnPetDeath;

    public bool inDark;

    [Header("Modifiers")]
    [Tooltip("Recommended between -1 and 0. Additive with the PetDrainReduction. Actual Drain = amt * (DarknessDrainIncrease + PetDrainReduction)")]
    [Range(-1, 0)] public float DarknessDrainIncrease;
    [Tooltip("Usually between 0 and 1. 1 being the original amount, and 0 being no drain at all. Additive with DarknessDrainIncrease")]
    [Min(0)] [SerializeField] float PetDrainReduction = 1f;
    [HideInInspector] public bool PetAlive = false;

    [Header("UI")]
    [SerializeField] Slider SanityBar;
    Image BarImage;
    Color originalCol;
    float LowSanityFlashTimer;
    float currVel = 0; //for smooth damp

    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //set stuff for new saves
        if (PlayerData.MaxSanity <= 0)
            PlayerData.MaxSanity = Base_MaxSanity;
        if (PlayerData.CurrSanity <= 0)
            PlayerData.CurrSanity = PlayerData.MaxSanity;

        UpdateSanityBar();
        BarImage = SanityBar.fillRect.GetComponent<Image>();
        originalCol = BarImage.color;
    }

    private void Update() {
        //flashing sanity bar display
        if ((PlayerData.CurrSanity / PlayerData.MaxSanity) * 100 <= 25) {
            LowSanityFlashTimer += Time.deltaTime;
            //flash speed increases the lower the sanity
            if (LowSanityFlashTimer >= (PlayerData.CurrSanity / PlayerData.MaxSanity)) {
                BarImage.color = new Color(0.45f, 0.55f, 0.568f, 1);
            }
            if (LowSanityFlashTimer > (PlayerData.CurrSanity / PlayerData.MaxSanity)*2) {
                BarImage.color = originalCol;
                LowSanityFlashTimer = 0f;
            }
        }
        else {
            BarImage.color = originalCol;
        }

        //make the bar go down smoothly (also updates the bar)
        float smoothValue = Mathf.SmoothDamp(SanityBar.value, PlayerData.CurrSanity, ref currVel, 0.5f);
        SanityBar.value = smoothValue;
    }

    //for both adding and substracting sanity
    public void ChangeSanity(float amt) {
        if (amt < 0)
            PlayerData.CurrSanity += amt * ((PetAlive ? PetDrainReduction : 1) + (inDark ? DarknessDrainIncrease : 0));
        else
            PlayerData.CurrSanity += amt;
        //check if curr sanity exceeding the max
        if (PlayerData.CurrSanity > PlayerData.MaxSanity)
            PlayerData.CurrSanity = PlayerData.MaxSanity; //set back to max
        //check if lost their mind
        else if (PlayerData.CurrSanity <= 0) {
            PlayerData.CurrSanity = 0; //set 0 to not break the sanity bar display
            Player_HealthManager.Instance.OnDeath();
        }
    }

    public void UpdateSanityBar() {
        if (SanityBar != null) {
            SanityBar.maxValue = PlayerData.MaxSanity;
            SanityBar.value = PlayerData.CurrSanity;
        }
    }
}
