using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]

public class Player_TravelTime : ScriptableObject
{
    [Header("Default Travel Time Data")]
    //Starting Point Base
    [SerializeField]
    public float BaseToFarm = 0;
    [SerializeField]
    public float BaseToGroceryStore = 0;
    [SerializeField]
    public float BaseToPoliceStation = 0;
    [SerializeField]
    public float BaseToHardwareStore = 0;
    [SerializeField]
    public float BaseToPharmacy = 0;
    [SerializeField]
    public float BaseToMilitaryCheckpoint = 0;
    [SerializeField]
    public float BaseToHighSchool = 0;
    [SerializeField]
    public float BaseToTrader = 0;

    //Starting Point Farm
    [SerializeField]
    public float FarmToGroceryStore = 0;
    [SerializeField]
    public float FarmToPoliceStation = 0;
    [SerializeField]
    public float FarmToHardwareStore = 0;
    [SerializeField]
    public float FarmToPharmacy = 0;
    [SerializeField]
    public float FarmToMilitaryCheckpoint = 0;
    [SerializeField]
    public float FarmToHighSchool = 0;
    [SerializeField]
    public float FarmToTrader = 0;

    //Starting Point Grocery Store
    [SerializeField]
    public float GroceryStoreToPoliceStation = 0;
    [SerializeField]
    public float GroceryStoreToHardwareStore = 0;
    [SerializeField]
    public float GroceryStoreToPharmacy = 0;
    [SerializeField]
    public float GroceryStoreToMilitaryCheckpoint = 0;
    [SerializeField]
    public float GroceryStoreToHighSchool = 0;
    [SerializeField]
    public float GroceryStoreToTrader = 0;

    //Starting Point Police Station
    [SerializeField]
    public float PoliceStationToHardwareStore = 0;
    [SerializeField]
    public float PoliceStationToPharmacy = 0;
    [SerializeField]
    public float PoliceStationToMilitaryCheckpoint = 0;
    [SerializeField]
    public float PoliceStationToHighSchool = 0;
    [SerializeField]
    public float PoliceStationToTrader = 0;

    //Starting Point Hardware Store
    [SerializeField]
    public float HardwareStoreToPharmacy = 0;
    [SerializeField]
    public float HardwareStoreToMilitaryCheckpoint = 0;
    [SerializeField]
    public float HardwareStoreToHighSchool = 0;
    [SerializeField]
    public float HardwareStoreToTrader = 0;

    //Starting Point Pharmacy
    [SerializeField]
    public float PharmacyToMilitaryCheckpoint = 0;
    [SerializeField]
    public float PharmacyToHighSchool = 0;
    [SerializeField]
    public float PharmacyToTrader = 0;

    //Starting Point Military Checkpoint
    [SerializeField]
    public float MilitaryCheckpointToHighSchool = 0;
    [SerializeField]
    public float MilitaryCheckpointToTrader = 0;

    //Starting Point High School
    [SerializeField]
    public float HighSchoolToTrader = 0;

    //Starting Point Trader

    private void OnEnable()
    {
    }
}
