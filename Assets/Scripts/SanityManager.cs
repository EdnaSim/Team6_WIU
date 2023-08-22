using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityManager : MonoBehaviour
{
    public static SanityManager Instance;

    [Header("Base stats")]
    [SerializeField] float Base_MaxSanity;

    [Tooltip("Amount of sanity drained by certain events")]
    public float DrainAmtOnStarve;
    public float DrainAmtOnHit;
    public float DrainAmtOnPetDeath;

    [Header("Pets being cute")]
    [Tooltip("Usually between 0 and 1. 1 being the original amount, and 0 being no drain at all.")]
    [Min(0)] [SerializeField] float PetDrainReduction = 0.5f;
    [HideInInspector] public bool PetAlive = false;

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
    }

    //for both adding and substracting sanity
    public void ChangeSanity(float amt) {
        if (amt < 0)
            PlayerData.CurrSanity += amt * PetDrainReduction;
        else
            PlayerData.CurrSanity += amt;
        //check if curr sanity exceeding the max
        if (PlayerData.CurrSanity > PlayerData.MaxSanity)
            PlayerData.CurrSanity = PlayerData.MaxSanity; //set back to max
        //check if lost their mind
        else if (PlayerData.CurrSanity <= 0) {
            PlayerData.CurrSanity = 0; //set 0 to not break the sanity bar display
            //TODO: Game over

        }
    }
}