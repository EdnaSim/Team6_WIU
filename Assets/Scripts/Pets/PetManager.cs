using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PetManager : MonoBehaviour
{
    GameObject player;
    public static UnityEvent PetDie;

    [Header("Pet stuff")]
    [SerializeField] SO_PetDetails PetDetails;
    public static Pet Pet;

    [Header("UI")]
    [SerializeField] GameObject PetSelectorContainer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //TODO: load saved data into SO_petDetails

        //listener to be invoked when pet starves to death
        if (PetDie == null)
            PetDie = new UnityEvent();
        PetDie.AddListener(OnPetDeath);
    }

    //new save, select pet
    public void SelectPet(GetPetTypeEnum petTypeEnum) { //using GetPetTypeEnum class cuz Unity doesnt allow enum values into params
        PetDetails.PetType = petTypeEnum.PetType;
        //spawn pet if possible
        if (!SpawnPet()) {
            Debug.LogError("SO_PetDetails PetType is null, or the Prefabs have not been assigned / PetManager.cs (SpawnPet) does not have the pet type included.");
            return;
        }
        //disable the selection page
        PetSelectorContainer.SetActive(false);
    }
    
    public bool SpawnPet() {
        if (Pet != null) {
            Debug.LogError("Trying to spawn pet when one is still alive.");
            return false;
        }
        switch (PetDetails.PetType) {
            case SO_PetDetails.PETTYPE.DOG:
            if (PetDetails.DogPrefab == null) return false;
            Pet = Instantiate(PetDetails.DogPrefab, player.transform.position, Quaternion.identity).GetComponent<Pet>();
            //Pet = go.GetComponent<Pet>();
            break;

            case SO_PetDetails.PETTYPE.CAT:
            if (PetDetails.CatPrefab == null) return false;
            Pet = Instantiate(PetDetails.CatPrefab, player.transform.position, Quaternion.identity).GetComponent<Pet>();
            break;

            case SO_PetDetails.PETTYPE.BIRD:
            if (PetDetails.BirdPrefab == null) return false;
            Pet = Instantiate(PetDetails.BirdPrefab, player.transform.position, Quaternion.identity).GetComponent<Pet>();
            break;

            case SO_PetDetails.PETTYPE.HORSE:
            if (PetDetails.HorsePrefab == null) return false;
            Pet = Instantiate(PetDetails.HorsePrefab, player.transform.position, Quaternion.identity).GetComponent<Pet>();
            break;

            default:
            return false;
        }
        PetDetails.MaxHunger = Pet.Base_MaxHunger;
        PetDetails.HungerDrain = Pet.Base_HungerDrain;

        return true;
    }

    public void OnPetDeath() {
        //reset pet details
        PetDetails.PetType = SO_PetDetails.PETTYPE.NONE;
        PetDetails.MaxHunger = Pet.Base_MaxHunger;
        PetDetails.CurrentHunger = PetDetails.MaxHunger;
        PetDetails.HungerDrain = Pet.Base_HungerDrain;

        //remove pet (maybe: add a corpse permanently in the base)
        Destroy(Pet.gameObject);
        Pet = null;
        
        //drain player sanity

    }
}