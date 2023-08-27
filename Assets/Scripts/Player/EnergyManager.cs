using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;
    public Slider StaminaBar;

    public float Base_MaxStamina = 100f;

    [Header("Passive drain")]
    [HideInInspector] public bool CanDrain;
    [SerializeField] float PassiveDrainAmt = 1f;

    [Header("Stamina Modifiers")]
    public bool isRunning;
    [Tooltip("Extra passive drain when running. Added to current passive drain.")]
    [SerializeField][Min(0)] float RunningDrainIncrease = 1f;

    [Header("Passive regen")]
    public float RestTimeTillRegenStamina = 3f;
    [SerializeField] float RegenInterval;
    float RegenTimer;
    public bool Regen = false;

    [Header("UI")]
    Image BarImage;
    Color originalCol;
    float LowEnergyFlashTimer;

    [HideInInspector] public bool PetAlive = false;

    private void Awake()
    {
        Instance = this;
        if (PlayerData.MaxStamina <= 0)
            PlayerData.MaxStamina = Base_MaxStamina;
        if (PlayerData.CurrStamina <= 0)
            PlayerData.CurrStamina = PlayerData.MaxStamina;
    }
    // Start is called before the first frame update
    void Start()
    {
        //staminaAmt = MaxStaminaAmt;
        
        UpdateBar();
        CanDrain = true;

        BarImage = StaminaBar.fillRect.GetComponent<Image>();
        originalCol = BarImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        //passive drain (increased when running)
        if(CanDrain && !Regen)
        {
            LoseEnergy((PassiveDrainAmt + (isRunning ? RunningDrainIncrease : 0)) * Time.deltaTime);
        }
        //flashing bar
        if ((PlayerData.CurrStamina / PlayerData.MaxStamina) * 100 <= 25)
        {
            LowEnergyFlashTimer += Time.deltaTime;
            if (LowEnergyFlashTimer >= (PlayerData.CurrStamina / PlayerData.MaxStamina))
            {
                BarImage.color = new Color(0.45f, 0.55f, 0.568f, 1);
            }
            if (LowEnergyFlashTimer > (PlayerData.CurrStamina / PlayerData.MaxStamina) * 2)
            {
                BarImage.color = originalCol;
                LowEnergyFlashTimer = 0f;
            }
        }
        else
        {
            BarImage.color = originalCol;
        }

        //passive regen start
        if (RegenTimer > 0f) RegenTimer -= Time.deltaTime;
        else if (Regen) {
            RegenTimer = RegenInterval;
            EnergyRecover(2);
        }
    }

    public void StartRegen() {
        Regen = true;
        RegenTimer = RegenInterval;
    }

    public void LoseEnergy(float energyloss)
    {
        PlayerData.CurrStamina -= energyloss;
        if (PlayerData.CurrStamina <= 0) {
            //Player_HealthManager.Instance.TakeDamage(0.01f, gameObject);
            PlayerData.CurrStamina = 0;
        }
        UpdateBar();
    }

    public void EnergyRecover(float energygain)
    {
        PlayerData.CurrStamina += energygain;
        if (PlayerData.CurrStamina > PlayerData.MaxStamina) {
            //Player_HealthManager.Instance.TakeDamage(0.01f, gameObject);
            PlayerData.CurrStamina = PlayerData.MaxStamina;
        }
        UpdateBar();
    }

    public void AdrenalineShot()
    {
        if(CanDrain)
        {
            //BarImage.color = new Color(0.45f, 0.55f, 0.568f, 1);
            BarImage.color = new Color(1f, 0.84f, 0f, 1);
            EnergyRecover(75f);
            SanityManager.Instance.ChangeSanity(20);
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
        //adrenaline shot ends
        Player_HealthManager.Instance.ChangeDamageMultiplier(0.5f);
        Player_Controller.Instance.MovementSpeed = Player_Controller.Instance.Base_MovementSpeed;
        BarImage.color = originalCol;
        CanDrain = true;
    }

    public void UpdateBar() {
        StaminaBar.maxValue = PlayerData.MaxStamina;
        StaminaBar.value = PlayerData.CurrStamina;
    }
}
