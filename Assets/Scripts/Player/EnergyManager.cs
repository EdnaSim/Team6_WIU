using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;
    public Slider StaminaBar;

    public float staminaAmt = 100f;

    public float MaxStaminaAmt = 100f;

    public bool CanDrain;

    [HideInInspector] public bool PetAlive = false;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        staminaAmt = MaxStaminaAmt;
        StaminaBar.value = staminaAmt / MaxStaminaAmt;
        CanDrain = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(CanDrain)
        {
            LoseEnergy(1f * Time.deltaTime);
        }

        if(staminaAmt <= 0)
        {
            Player_HealthManager.Player_hm.TakeDamage(1f, gameObject);
        }
    }

    public void LoseEnergy(float energyloss)
    {
        if (PetManager.Instance.PetDetails.PetType == SO_PetDetails.PETTYPE.CAT)
            staminaAmt -= energyloss * (PetAlive ? PetManager.Pet.GetComponent<Pet_Cat>().StaminaDrainReduction : 1);
        else
            staminaAmt -= energyloss;
        StaminaBar.value = staminaAmt / MaxStaminaAmt;
    }

    public void EnergyRecover(float energygain)
    {
        staminaAmt += energygain;
        StaminaBar.value = staminaAmt / MaxStaminaAmt;
    }

    public void AdrenalineShot()
    {
        if(CanDrain)
        {
            Player_HealthManager.Player_hm.ChangeDamageModifier(-0.5f);
            Player_HealthManager.Player_hm.Heal(25f);
            Player_Controller.Instance.MovementSpeed += 25f;
            StartCoroutine(InvulnerableTimer());
            CanDrain = false;
        }
    }

    IEnumerator InvulnerableTimer()
    {
        yield return new WaitForSeconds(5f);
        Player_HealthManager.Player_hm.ChangeDamageModifier(+0.5f);
        Player_Controller.Instance.MovementSpeed = Player_Controller.Instance.Base_MovementSpeed;
        CanDrain = true;
    }
}
