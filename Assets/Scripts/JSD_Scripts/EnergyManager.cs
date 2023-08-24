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

    [Header("UI")]
    [SerializeField] Slider EnergyBar;
    Image BarImage;
    Color originalCol;
    float LowEnergyFlashTimer;

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

        if(staminaAmt <= 0)
        {
            Player_HealthManager.Player_hm.TakeDamage(0.01f, gameObject);
            staminaAmt = 0;
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            AdrenalineShot();
        }

        if ((staminaAmt / MaxStaminaAmt) * 100 <= 25)
        {
            LowEnergyFlashTimer += Time.deltaTime;
            if (LowEnergyFlashTimer >= (staminaAmt / MaxStaminaAmt))
            {
                BarImage.color = new Color(0.45f, 0.55f, 0.568f, 1);
            }
            if (LowEnergyFlashTimer > (staminaAmt / MaxStaminaAmt) * 2)
            {
                BarImage.color = originalCol;
                LowEnergyFlashTimer = 0f;
            }
        }
        else
        {
            BarImage.color = originalCol;
        }
    }

    public void LoseEnergy(float energyloss)
    {
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
            //BarImage.color = new Color(0.45f, 0.55f, 0.568f, 1);
            BarImage.color = new Color(1f, 0.84f, 0f, 1);
            EnergyRecover(75f);
            PlayerData.CurrSanity += 20f;
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
        BarImage.color = originalCol;
        CanDrain = true;
    }
}
