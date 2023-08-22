using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_PetDetails : ScriptableObject
{
    //prefabs for when need to spawn a new pet
    public GameObject DogPrefab;
    public GameObject CatPrefab;
    public GameObject BirdPrefab;
    public GameObject HorsePrefab;

    [System.Serializable]
    public enum PETTYPE {
        DOG =1,
        CAT =2,
        BIRD =3,
        HORSE = 4,
        NONE = 5
    }
    //pet's details
    public PETTYPE PetType;
    public float MaxHunger;
    public float CurrentHunger;
    public float HungerDrain; //how much to drain the hunger by each time
    public string Name;

    private void Awake() {
        //get saved data
        PetType = PETTYPE.NONE;
        MaxHunger = 100;
        CurrentHunger = MaxHunger;
        HungerDrain = 10;
        Name = "Pet";
    }
}
