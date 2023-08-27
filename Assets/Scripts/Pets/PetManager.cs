using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PetManager : MonoBehaviour
{
    public static PetManager Instance;
    GameObject player;
    public static UnityEvent PetDie;

    [Header("Pet stuff")]
    public SO_PetDetails PetDetails;
    public static Pet Pet;

    [Header("UI")]
    [SerializeField] GameObject PetSelectorContainer;
    [SerializeField] GameObject NameContainer;
    [SerializeField] TMP_Text PetDeathAlert;

    private void Awake() {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //listener to be invoked when pet starves to death
        if (PetDie == null)
            PetDie = new UnityEvent();
        PetDie.AddListener(OnPetDeath);

        if (NameContainer != null)
            NameContainer.SetActive(false);
        if (GetNewGame.isNewGame) {
            MenuManager.Instance.ShowMenu(PetSelectorContainer);
        }
        else {
            //load from json to SO_PetDetails
            LoadPetDetails();
        }

        if (PetDeathAlert != null)
            PetDeathAlert.enabled = false;
    }

    //new save, on select pet
    public void SelectPet(GetPetTypeEnum petTypeEnum) { //using GetPetTypeEnum class cuz Unity doesnt allow enum values into params
        PetDetails.PetType = petTypeEnum.PetType;
        //disable the selection page
        MenuManager.Instance.HideMenu();
        //spawn pet if possible
        if (!SpawnPet()) {
            Debug.LogError("SO_PetDetails PetType is null, or the Prefabs have not been assigned / PetManager.cs (SpawnPet) does not have the pet type included.");
            return;
        }
        if (PetDetails.PetType != SO_PetDetails.PETTYPE.NONE) {
            if (NameContainer != null)
                MenuManager.Instance.ShowMenu(NameContainer);
        }
    }
    
    public bool SpawnPet() {
        if (Pet != null) {
            Debug.LogError("Trying to spawn pet when one is still alive.");
            return false;
        }
        switch (PetDetails.PetType) {
            case SO_PetDetails.PETTYPE.DOG:
            if (PetDetails.DogPrefab == null) return false;
            Pet = Instantiate(PetDetails.DogPrefab, player.transform.position, Quaternion.identity).GetComponent<Pet_Dog>();
            //Pet = go.GetComponent<Pet>();
            break;

            case SO_PetDetails.PETTYPE.CAT:
            if (PetDetails.CatPrefab == null) return false;
            Pet = Instantiate(PetDetails.CatPrefab, player.transform.position, Quaternion.identity).GetComponent<Pet_Cat>();
            break;

            case SO_PetDetails.PETTYPE.BIRD:
            if (PetDetails.BirdPrefab == null) return false;
            Pet = Instantiate(PetDetails.BirdPrefab, player.transform.position, Quaternion.identity).GetComponent<Pet_Bird>();
            break;

            case SO_PetDetails.PETTYPE.HORSE:
            if (PetDetails.HorsePrefab == null) return false;
            Pet = Instantiate(PetDetails.HorsePrefab, player.transform.position, Quaternion.identity).GetComponent<Pet_Horse>();
            break;

            case SO_PetDetails.PETTYPE.NONE:
            //return true to close container
            return true;

            default:
            return false;
        }
        if (PetDetails.PetType != SO_PetDetails.PETTYPE.NONE) {
            PetDetails.MaxHunger = Pet.Base_MaxHunger;
            PetDetails.HungerDrain = Pet.Base_HungerDrain;
            SanityManager.Instance.PetAlive = true;
            EnergyManager.Instance.PetAlive = true;
        }

        return true;
    }

    public void NamePet(TMP_InputField input) {
        PetDetails.Name = input.text;
        Pet.Nametag.text = PetDetails.Name;
        MenuManager.Instance.HideMenu();
    }

    public void OnPetDeath() {
        //show alert
        if (PetDeathAlert != null) {
            PetDeathAlert.enabled = true;
            PetDeathAlert.text = Pet.name + " starved to death!";
            Destroy(PetDeathAlert, 5); //show for x seconds
        }

        ResetAndRemovePet();
        //drain(-) player sanity
        SanityManager.Instance.PetAlive = false;
        SanityManager.Instance.ChangeSanity(-SanityManager.Instance.DrainAmtOnPetDeath);
        EnergyManager.Instance.PetAlive = false;
    }

    public void ResetAndRemovePet() {
        if (Pet == null) return; 

        //reset pet details
        PetDetails.PetType = SO_PetDetails.PETTYPE.NONE;
        PetDetails.MaxHunger = Pet.Base_MaxHunger;
        PetDetails.CurrentHunger = PetDetails.MaxHunger;
        PetDetails.HungerDrain = Pet.Base_HungerDrain;

        //remove pet
        Destroy(Pet.gameObject);
        Pet = null;
    }

    public void LoadPetDetails() {
        //TODO: load details from json to here
        
    }
}
