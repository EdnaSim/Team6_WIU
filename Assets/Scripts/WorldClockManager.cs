using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldClockManager : MonoBehaviour
{
    //Player Locations
    public List<PlayerLocation> LocationList;
    [SerializeField]
    private WorldClock WCData;
    [SerializeField]
    private Player_TravelTime OnFoot;
    [SerializeField]
    private Player_TravelTime OnHorse;

    //Travel Message Time Display
    [SerializeField]
    private GameObject TM_TimeDisplay;
    [SerializeField]
    private TMP_Text TM_TimeDisplayMsg;
    //Travel Unavailable
    [SerializeField]
    private GameObject TM_NoTravelDisplay;
    [SerializeField]
    private TMP_Text TM_NoTravelDisplayMsg;
    //Proceed Button
    [SerializeField]
    private GameObject ProceedBtn;

    //Insert Pet Data here
    //Insert Already Explored Bool here

    public bool CanTravel;
    public bool SameLocation;
    public float TravelHour;
    public float TravelMinute;
    public float TravelEndTime;
    private float TravelStartTime;

    // Start is called before the first frame update
    void Start()
    {
        //CanTravel = false;
        TravelStartTime = WCData.WorldTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Display Can Travel UI
        if (CanTravel == true)
        {
            ProceedBtn.SetActive(true);
            TM_NoTravelDisplay.SetActive(false);
            if (SameLocation == true)
            {
                TM_TimeDisplay.SetActive(false);
            }
            else
            {
                TM_TimeDisplay.SetActive(true);
                TM_TimeDisplayMsg.text = "It will take " + TravelHour + " hours and " + TravelMinute + " minutes to reach the destination.";
            }
        }
        else
        {
            ProceedBtn.SetActive(false);
            TM_NoTravelDisplay.SetActive(true);
            if (SameLocation == true)
            {
                TM_TimeDisplay.SetActive(false);
                TM_NoTravelDisplayMsg.text = "You've already explored this location for today.";
            }
            else
            {
                TM_TimeDisplay.SetActive(true);
                TM_TimeDisplayMsg.text = "It will take " + TravelHour + " hours and " + TravelMinute + " minutes to reach the destination.";
                TM_NoTravelDisplayMsg.text = "There isn't enough time left in the day to make the trip.";
            }
        }
    }

    public void CheckTravelTimes()
    {   
        foreach (PlayerLocation Loc1 in LocationList)
        {
            if (Loc1.CurrCLCheck == true)
            {
                if (Loc1.CurrDCheck == true)
                {
                    if (Loc1.LocationName == "Base")
                    {
                        CanTravel = true;
                        SameLocation = true;
                    }
                    else
                    {
                        //if (AlreadyExplored == true)
                        //{
                        //  CanTravel = false;
                        //  SameLocation = true;
                        //}
                        //else
                        //{
                            CanTravel = true;
                            SameLocation = true;
                        //}
                    }
                }
                else
                {
                    SameLocation = false;
                    foreach (PlayerLocation Loc2 in LocationList)
                    {
                        if (Loc2.CurrDCheck == true)
                        {
                            //From Base
                            if (Loc1.LocationName == "Base")
                            {
                                if (Loc2.LocationName == "Farm")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToFarm % 1 * 60;
                                    //TravelHour = OnHorse.BaseToFarm - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToFarm % 1 * 60;
                                    TravelHour = OnFoot.BaseToFarm - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Grocery Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToGroceryStore % 1 * 60;
                                    //TravelHour = OnHorse.BaseToGroceryStore - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToGroceryStore % 1 * 60;
                                    TravelHour = OnFoot.BaseToGroceryStore - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Hardware Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToHardwareStore % 1 * 60;
                                    //TravelHour = OnHorse.BaseToHardwareStore - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToHardwareStore % 1 * 60;
                                    TravelHour = OnFoot.BaseToHardwareStore - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Police Station")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToPoliceStation % 1 * 60;
                                    //TravelHour = OnHorse.BaseToPoliceStation - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToPoliceStation % 1 * 60;
                                    TravelHour = OnFoot.BaseToPoliceStation - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Pharmacy")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToPharmacy % 1 * 60;
                                    //TravelHour = OnHorse.BaseToPharmacy - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToPharmacy % 1 * 60;
                                    TravelHour = OnFoot.BaseToPharmacy - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "High School")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.BaseToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.BaseToHighSchool - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Military Checkpoint")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToMilitaryCheckpoint % 1 * 60;
                                    //TravelHour = OnHorse.BaseToMilitaryCheckpoint - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToMilitaryCheckpoint % 1 * 60;
                                    TravelHour = OnFoot.BaseToMilitaryCheckpoint - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Trader")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToTrader % 1 * 60;
                                    //TravelHour = OnHorse.BaseToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToTrader % 1 * 60;
                                    TravelHour = OnFoot.BaseToTrader - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                            }
                            //From Farm
                            else if (Loc1.LocationName == "Farm")
                            {
                                if (Loc2.LocationName == "Base")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToFarm % 1 * 60;
                                    //TravelHour = OnHorse.BaseToFarm - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToFarm % 1 * 60;
                                    TravelHour = OnFoot.BaseToFarm - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Grocery Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToGroceryStore % 1 * 60;
                                    //TravelHour = OnHorse.FarmToGroceryStore - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToGroceryStore % 1 * 60;
                                    TravelHour = OnFoot.FarmToGroceryStore - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Hardware Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToHardwareStore % 1 * 60;
                                    //TravelHour = OnHorse.FarmToHardwareStore - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToHardwareStore % 1 * 60;
                                    TravelHour = OnFoot.FarmToHardwareStore - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Police Station")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToPoliceStation % 1 * 60;
                                    //TravelHour = OnHorse.FarmToPoliceStation - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToPoliceStation % 1 * 60;
                                    TravelHour = OnFoot.FarmToPoliceStation - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Pharmacy")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToPharmacy % 1 * 60;
                                    //TravelHour = OnHorse.FarmToPharmacy - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToPharmacy % 1 * 60;
                                    TravelHour = OnFoot.FarmToPharmacy - (TravelMinute/60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "High School")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.FarmToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.FarmToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Military Checkpoint")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToMilitaryCheckpoint % 1 * 60;
                                    //TravelHour = OnHorse.FarmToMilitaryCheckpoint - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToMilitaryCheckpoint % 1 * 60;
                                    TravelHour = OnFoot.FarmToMilitaryCheckpoint - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Trader")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToTrader % 1 * 60;
                                    //TravelHour = OnHorse.FarmToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToTrader % 1 * 60;
                                    TravelHour = OnFoot.FarmToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                            }
                            //From Grocery Store
                            else if (Loc1.LocationName == "Grocery Store")
                            {
                                if (Loc2.LocationName == "Base")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToGroceryStore % 1 * 60;
                                    //TravelHour = OnHorse.BaseToGroceryStore - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToGroceryStore % 1 * 60;
                                    TravelHour = OnFoot.BaseToGroceryStore - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Farm")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToGroceryStore % 1 * 60;
                                    //TravelHour = OnHorse.FarmToGroceryStore - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToGroceryStore % 1 * 60;
                                    TravelHour = OnFoot.FarmToGroceryStore - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Hardware Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.GroceryStoreToHardwareStore % 1 * 60;
                                    //TravelHour = OnHorse.GroceryStoreToHardwareStore - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.GroceryStoreToHardwareStore % 1 * 60;
                                    TravelHour = OnFoot.GroceryStoreToHardwareStore - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Police Station")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.GroceryStoreToPoliceStation % 1 * 60;
                                    //TravelHour = OnHorse.GroceryStoreToPoliceStation - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.GroceryStoreToPoliceStation % 1 * 60;
                                    TravelHour = OnFoot.GroceryStoreToPoliceStation - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Pharmacy")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.GroceryStoreToPharmacy % 1 * 60;
                                    //TravelHour = OnHorse.GroceryStoreToPharmacy - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.GroceryStoreToPharmacy % 1 * 60;
                                    TravelHour = OnFoot.GroceryStoreToPharmacy - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "High School")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.GroceryStoreToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.GroceryStoreToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.GroceryStoreToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.GroceryStoreToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Military Checkpoint")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.GroceryStoreToMilitaryCheckpoint % 1 * 60;
                                    //TravelHour = OnHorse.GroceryStoreToMilitaryCheckpoint - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.GroceryStoreToMilitaryCheckpoint % 1 * 60;
                                    TravelHour = OnFoot.GroceryStoreToMilitaryCheckpoint - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Trader")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.GroceryStoreToTrader % 1 * 60;
                                    //TravelHour = OnHorse.GroceryStoreToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.GroceryStoreToTrader % 1 * 60;
                                    TravelHour = OnFoot.GroceryStoreToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                            }
                            //From Hardware Store
                            else if (Loc1.LocationName == "Hardware Store")
                            {
                                if (Loc2.LocationName == "Base")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToHardwareStore % 1 * 60;
                                    //TravelHour = OnHorse.BaseToHardwareStore - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToHardwareStore % 1 * 60;
                                    TravelHour = OnFoot.BaseToHardwareStore - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Farm")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToHardwareStore % 1 * 60;
                                    //TravelHour = OnHorse.FarmToHardwareStore - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToHardwareStore % 1 * 60;
                                    TravelHour = OnFoot.FarmToHardwareStore - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Grocery Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.GroceryStoreToHardwareStore % 1 * 60;
                                    //TravelHour = OnHorse.GroceryStoreToHardwareStore - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.GroceryStoreToHardwareStore % 1 * 60;
                                    TravelHour = OnFoot.GroceryStoreToHardwareStore - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Police Station")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PoliceStationToHardwareStore % 1 * 60;
                                    //TravelHour = OnHorse.PoliceStationToHardwareStore - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PoliceStationToHardwareStore % 1 * 60;
                                    TravelHour = OnFoot.PoliceStationToHardwareStore - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Pharmacy")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.HardwareStoreToPharmacy % 1 * 60;
                                    //TravelHour = OnHorse.HardwareStoreToPharmacy - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.HardwareStoreToPharmacy % 1 * 60;
                                    TravelHour = OnFoot.HardwareStoreToPharmacy - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "High School")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.HardwareStoreToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.HardwareStoreToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.HardwareStoreToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.HardwareStoreToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Military Checkpoint")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.HardwareStoreToMilitaryCheckpoint % 1 * 60;
                                    //TravelHour = OnHorse.HardwareStoreToMilitaryCheckpoint - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.HardwareStoreToMilitaryCheckpoint % 1 * 60;
                                    TravelHour = OnFoot.HardwareStoreToMilitaryCheckpoint - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Trader")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.HardwareStoreToTrader % 1 * 60;
                                    //TravelHour = OnHorse.HardwareStoreToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.HardwareStoreToTrader % 1 * 60;
                                    TravelHour = OnFoot.HardwareStoreToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                            }
                            //From Police Station
                            else if (Loc1.LocationName == "Police Station")
                            {
                                if (Loc2.LocationName == "Base")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToPoliceStation % 1 * 60;
                                    //TravelHour = OnHorse.BaseToPoliceStation - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToPoliceStation % 1 * 60;
                                    TravelHour = OnFoot.BaseToPoliceStation - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Farm")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToPoliceStation % 1 * 60;
                                    //TravelHour = OnHorse.FarmToPoliceStation - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToPoliceStation % 1 * 60;
                                    TravelHour = OnFoot.FarmToPoliceStation - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Grocery Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.GroceryStoreToPoliceStation % 1 * 60;
                                    //TravelHour = OnHorse.GroceryStoreToPoliceStation - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.GroceryStoreToPoliceStation % 1 * 60;
                                    TravelHour = OnFoot.GroceryStoreToPoliceStation - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Hardware Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PoliceStationToHardwareStore % 1 * 60;
                                    //TravelHour = OnHorse.PoliceStationToHardwareStore - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PoliceStationToHardwareStore % 1 * 60;
                                    TravelHour = OnFoot.PoliceStationToHardwareStore - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Pharmacy")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PoliceStationToPharmacy % 1 * 60;
                                    //TravelHour = OnHorse.PoliceStationToPharmacy - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PoliceStationToPharmacy % 1 * 60;
                                    TravelHour = OnFoot.PoliceStationToPharmacy - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "High School")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PoliceStationToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.PoliceStationToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PoliceStationToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.PoliceStationToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Military Checkpoint")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PoliceStationToMilitaryCheckpoint % 1 * 60;
                                    //TravelHour = OnHorse.PoliceStationToMilitaryCheckpoint - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PoliceStationToMilitaryCheckpoint % 1 * 60;
                                    TravelHour = OnFoot.PoliceStationToMilitaryCheckpoint - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Trader")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PoliceStationToTrader % 1 * 60;
                                    //TravelHour = OnHorse.PoliceStationToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PoliceStationToTrader % 1 * 60;
                                    TravelHour = OnFoot.PoliceStationToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                            }
                            //From Pharmacy
                            else if (Loc1.LocationName == "Pharmacy")
                            {
                                if (Loc2.LocationName == "Base")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToPharmacy % 1 * 60;
                                    //TravelHour = OnHorse.BaseToPharmacy - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToPharmacy % 1 * 60;
                                    TravelHour = OnFoot.BaseToPharmacy - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Farm")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToPharmacy % 1 * 60;
                                    //TravelHour = OnHorse.FarmToPharmacy - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToPharmacy % 1 * 60;
                                    TravelHour = OnFoot.FarmToPharmacy - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Grocery Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.GroceryStoreToPharmacy % 1 * 60;
                                    //TravelHour = OnHorse.GroceryStoreToPharmacy - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.GroceryStoreToPharmacy % 1 * 60;
                                    TravelHour = OnFoot.GroceryStoreToPharmacy - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Hardware Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.HardwareStoreToPharmacy % 1 * 60;
                                    //TravelHour = OnHorse.HardwareStoreToPharmacy - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.HardwareStoreToPharmacy % 1 * 60;
                                    TravelHour = OnFoot.HardwareStoreToPharmacy - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Police Station")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PoliceStationToPharmacy % 1 * 60;
                                    //TravelHour = OnHorse.PoliceStationToPharmacy - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PoliceStationToPharmacy % 1 * 60;
                                    TravelHour = OnFoot.PoliceStationToPharmacy - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "High School")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PharmacyToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.PharmacyToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PharmacyToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.PharmacyToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Military Checkpoint")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PharmacyToMilitaryCheckpoint % 1 * 60;
                                    //TravelHour = OnHorse.PharmacyToMilitaryCheckpoint - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PharmacyToMilitaryCheckpoint % 1 * 60;
                                    TravelHour = OnFoot.PharmacyToMilitaryCheckpoint - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Trader")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PharmacyToTrader % 1 * 60;
                                    //TravelHour = OnHorse.PharmacyToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PharmacyToTrader % 1 * 60;
                                    TravelHour = OnFoot.PharmacyToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                            }
                            //From High School
                            else if (Loc1.LocationName == "High School")
                            {
                                if (Loc2.LocationName == "Base")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.BaseToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.BaseToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Farm")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.FarmToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.FarmToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Grocery Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.GroceryStoreToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.GroceryStoreToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.GroceryStoreToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.GroceryStoreToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Hardware Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.HardwareStoreToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.HardwareStoreToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.HardwareStoreToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.HardwareStoreToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Police Station")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PoliceStationToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.PoliceStationToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PoliceStationToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.PoliceStationToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Pharmacy")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PharmacyToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.PharmacyToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PharmacyToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.PharmacyToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Military Checkpoint")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.MilitaryCheckpointToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.MilitaryCheckpointToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.MilitaryCheckpointToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.MilitaryCheckpointToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Trader")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.HighSchoolToTrader % 1 * 60;
                                    //TravelHour = OnHorse.HighSchoolToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.HighSchoolToTrader % 1 * 60;
                                    TravelHour = OnFoot.HighSchoolToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                            }
                            //From Military Checkpoint
                            else if (Loc1.LocationName == "Military Checkpoint")
                            {
                                if (Loc2.LocationName == "Base")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToMilitaryCheckpoint % 1 * 60;
                                    //TravelHour = OnHorse.BaseToMilitaryCheckpoint - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToMilitaryCheckpoint % 1 * 60;
                                    TravelHour = OnFoot.BaseToMilitaryCheckpoint - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Farm")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToMilitaryCheckpoint % 1 * 60;
                                    //TravelHour = OnHorse.FarmToMilitaryCheckpoint - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToMilitaryCheckpoint % 1 * 60;
                                    TravelHour = OnFoot.FarmToMilitaryCheckpoint - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Grocery Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.GroceryStoreToMilitaryCheckpoint % 1 * 60;
                                    //TravelHour = OnHorse.GroceryStoreToMilitaryCheckpoint - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.GroceryStoreToMilitaryCheckpoint % 1 * 60;
                                    TravelHour = OnFoot.GroceryStoreToMilitaryCheckpoint - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Hardware Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.HardwareStoreToMilitaryCheckpoint % 1 * 60;
                                    //TravelHour = OnHorse.HardwareStoreToMilitaryCheckpoint - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.HardwareStoreToMilitaryCheckpoint % 1 * 60;
                                    TravelHour = OnFoot.HardwareStoreToMilitaryCheckpoint - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Police Station")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PoliceStationToMilitaryCheckpoint % 1 * 60;
                                    //TravelHour = OnHorse.PoliceStationToMilitaryCheckpoint - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PoliceStationToMilitaryCheckpoint % 1 * 60;
                                    TravelHour = OnFoot.PoliceStationToMilitaryCheckpoint - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Pharmacy")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PharmacyToMilitaryCheckpoint % 1 * 60;
                                    //TravelHour = OnHorse.PharmacyToMilitaryCheckpoint - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PharmacyToMilitaryCheckpoint % 1 * 60;
                                    TravelHour = OnFoot.PharmacyToMilitaryCheckpoint - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "High School")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.MilitaryCheckpointToHighSchool % 1 * 60;
                                    //TravelHour = OnHorse.MilitaryCheckpointToHighSchool - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.MilitaryCheckpointToHighSchool % 1 * 60;
                                    TravelHour = OnFoot.MilitaryCheckpointToHighSchool - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Trader")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.MilitaryCheckpointToTrader % 1 * 60;
                                    //TravelHour = OnHorse.MilitaryCheckpointToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.MilitaryCheckpointToTrader % 1 * 60;
                                    TravelHour = OnFoot.MilitaryCheckpointToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                            }
                            //From Trader
                            else if (Loc1.LocationName == "Trader")
                            {
                                if (Loc2.LocationName == "Base")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.BaseToTrader % 1 * 60;
                                    //TravelHour = OnHorse.BaseToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.BaseToTrader % 1 * 60;
                                    TravelHour = OnFoot.BaseToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Farm")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.FarmToTrader % 1 * 60;
                                    //TravelHour = OnHorse.FarmToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.FarmToTrader % 1 * 60;
                                    TravelHour = OnFoot.FarmToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Grocery Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.GroceryStoreToTrader % 1 * 60;
                                    //TravelHour = OnHorse.GroceryStoreToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.GroceryStoreToTrader % 1 * 60;
                                    TravelHour = OnFoot.GroceryStoreToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Hardware Store")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.HardwareStoreToTrader % 1 * 60;
                                    //TravelHour = OnHorse.HardwareStoreToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.HardwareStoreToTrader % 1 * 60;
                                    TravelHour = OnFoot.HardwareStoreToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Police Station")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PoliceStationToTrader % 1 * 60;
                                    //TravelHour = OnHorse.PoliceStationToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PoliceStationToTrader % 1 * 60;
                                    TravelHour = OnFoot.PoliceStationToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Pharmacy")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.PharmacyToTrader % 1 * 60;
                                    //TravelHour = OnHorse.PharmacyToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.PharmacyToTrader % 1 * 60;
                                    TravelHour = OnFoot.PharmacyToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "High School")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.HighSchoolToTrader % 1 * 60;
                                    //TravelHour = OnHorse.HighSchoolToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.HighSchoolToTrader % 1 * 60;
                                    TravelHour = OnFoot.HighSchoolToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                                else if (Loc2.LocationName == "Military Checkpoint")
                                {
                                    //if (H == true)
                                    //{
                                    //TravelMinute = OnHorse.MilitaryCheckpointToTrader % 1 * 60;
                                    //TravelHour = OnHorse.MilitaryCheckpointToTrader - (TravelMinute/60);
                                    //TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    //if (TravelEndTime > 2400)
                                    //{
                                    //    CanTravel = false;
                                    //}
                                    //else
                                    //{
                                    //    CanTravel = true;
                                    //}
                                    //}
                                    //else
                                    //{
                                    TravelMinute = OnFoot.MilitaryCheckpointToTrader % 1 * 60;
                                    TravelHour = OnFoot.MilitaryCheckpointToTrader - (TravelMinute / 60);
                                    TravelEndTime = PredictTime(TravelMinute, TravelHour);
                                    if (TravelEndTime > 2400)
                                    {
                                        CanTravel = false;
                                    }
                                    else
                                    {
                                        CanTravel = true;
                                    }
                                    //}
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void MoveTimeForward()
    {
        if (SameLocation == true)
        {
            WCData.WorldTime = TravelStartTime;
        }
        else
        {
            WCData.WorldTime = TravelEndTime;
        }
    }

    private float PredictTime(float Min, float Hour)
    {
        float CurrentTime = WCData.WorldTime;
        float EndTime1 = CurrentTime + (Hour * 100);
        float EndTime2 = EndTime1 + Min;
        if (EndTime2 % 100 >= 60)
        {
            EndTime2 += 40;
        }
        return EndTime2;
    }
}
