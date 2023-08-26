using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;
    public Slider StaminaBar;

    [Header("Base stats")]
    [SerializeField] float Base_MaxEnergy;

    public float staminaAmt = 100f;

    public float MaxStaminaAmt = 100f;

    public bool CanDrain;

    [Header("UI")]
    [SerializeField] Slider EnergyBar;
    Image BarImage;
    Color originalCol;
    float LowEnergyFlashTimer;
    float currVel = 0; //for smooth damp

    [HideInInspector] public bool PetAlive = false;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //staminaAmt = MaxStaminaAmt;
        //StaminaBar.value = staminaAmt / MaxStaminaAmt;
        //set stuff for new saves
        if (PlayerData.MaxEnergy <= 0)
            PlayerData.MaxEnergy = Base_MaxEnergy;
        if (PlayerData.CurrEnergy <= 0)
            PlayerData.CurrEnergy = PlayerData.MaxEnergy;
        CanDrain = true;

        BarImage = EnergyBar.fillRect.GetComponent<Image>();
        originalCol = BarImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(CanDrain)
        {
            LoseEnergy(1f * Time.deltaTime);
        }

        if(PlayerData.CurrEnergy <= 0)
        {
            Player_HealthManager.Instance.TakeDamage(0.01f, gameObject);
            PlayerData.CurrEnergy = 0;
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            AdrenalineShot();
        }

        if ((PlayerData.CurrEnergy / PlayerData.MaxEnergy) * 100 <= 25)
        {
            LowEnergyFlashTimer += Time.deltaTime;
            if (LowEnergyFlashTimer >= (PlayerData.CurrEnergy / PlayerData.MaxEnergy))
            {
                BarImage.color = new Color(0.45f, 0.55f, 0.568f, 1);
            }
            if (LowEnergyFlashTimer > (PlayerData.CurrEnergy / PlayerData.MaxEnergy) * 2)
            {
                BarImage.color = originalCol;
                LowEnergyFlashTimer = 0f;
            }
        }
        else
        {
            BarImage.color = originalCol;
        }

        //make the bar go down smoothly
        float smoothValue = Mathf.SmoothDamp(EnergyBar.value, PlayerData.CurrSanity, ref currVel, 0.5f);
        EnergyBar.value = smoothValue;
    }

    public void LoseEnergy(float energyloss)
    {
        //staminaAmt -= energyloss;
        //StaminaBar.value = staminaAmt / MaxStaminaAmt;
        PlayerData.CurrEnergy -= energyloss;
        StaminaBar.value = PlayerData.CurrEnergy / PlayerData.MaxEnergy;
    }

    public void EnergyRecover(float energygain)
    {
        //staminaAmt += energygain;
        //StaminaBar.value = staminaAmt / MaxStaminaAmt;
        PlayerData.CurrEnergy += energygain;
        StaminaBar.value = PlayerData.CurrEnergy / PlayerData.MaxEnergy;
    }

    public void AdrenalineShot()
    {
        if(CanDrain)
        {
            //BarImage.color = new Color(0.45f, 0.55f, 0.568f, 1);
            BarImage.color = new Color(1f, 0.84f, 0f, 1);
            EnergyRecover(75f);
            PlayerData.CurrSanity += 20f;
            Player_HealthManager.Instance.ChangeDamageMultiplier(-0.5f);
            Player_HealthManager.Instance.Heal(25f);
            Player_Controller.Instance.MovementSpeed += 25f;
            StartCoroutine(InvulnerableTimer());
            CanDrain = false;
        }
    }

    IEnumerator InvulnerableTimer()
    {
        yield return new WaitForSeconds(5f);
        Player_HealthManager.Instance.ChangeDamageMultiplier(+0.5f);
        Player_Controller.Instance.MovementSpeed = Player_Controller.Instance.Base_MovementSpeed;
        BarImage.color = originalCol;
        CanDrain = true;
    }
}
