using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LocationManager : MonoBehaviour
{
    //Player Locations
    public List<PlayerLocation> LocationList;
    //Travel Message UI
    [SerializeField]
    private GameObject TM_UI;
    //Current Location
    [SerializeField]
    private TMP_Text LocationDisplay;
    //Travel Message Location Display
    [SerializeField]
    private TMP_Text TM_LocationDisplay;

    // Start is called before the first frame update
    void Start()
    {
        //LocationList = new List<PlayerLocation>();
        
        foreach(PlayerLocation Loc in LocationList)
        {
            //Debug.Log(Loc.LocationName + Loc.CurrCLCheck);
            if (Loc.CurrCLCheck == true)
            {
                LocationDisplay.text = "" + Loc.LocationName;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TravelMsg()
    {
        foreach (PlayerLocation Loc1 in LocationList)
        {
            if (Loc1.CurrCLCheck == true)
            {
                foreach (PlayerLocation Loc2 in LocationList)
                {
                    if (Loc2.CurrDCheck == true)
                    {
                        TM_UI.SetActive(true);
                        TM_LocationDisplay.text = "" + Loc1.LocationName + " - " + Loc2.LocationName;
                    }
                }
            }
        }
    }
    
    public void CancelTravel()
    {
        foreach (PlayerLocation Loc in LocationList)
        {
            if (Loc.CurrDCheck == true)
            {
                Loc.CurrDCheck = false;
            }
        }
    }

    public void Travelling()
    {
        foreach (PlayerLocation Loc1 in LocationList)
        {
            if (Loc1.CurrCLCheck == true)
            {
                if (Loc1.CurrDCheck == true)
                {
                    Loc1.CurrDCheck = false;

                    switch (Loc1.LocationName)
                    {
                        case "Base":
                            SceneLoader.Instance.LoadScene("Base");
                            break;

                        case "Farm":
                            SceneLoader.Instance.LoadScene("Farm");
                            break;

                        case "Police Station":
                            SceneLoader.Instance.LoadScene("PoliceStation");
                            break;

                        case "Grocery Store":
                            SceneLoader.Instance.LoadScene("GroceryStore");
                            break;

                        case "Hardware Store":
                            SceneLoader.Instance.LoadScene("HardwareStore");
                            break;

                        case "Pharmacy":
                            SceneLoader.Instance.LoadScene("Pharmacy");
                            break;

                        case "High School":
                            SceneLoader.Instance.LoadScene("HighSchool");
                            break;

                        case "Military Checkpoint":
                            SceneLoader.Instance.LoadScene("MilitaryCheckpoint");
                            break;

                        case "Trader":
                            SceneLoader.Instance.LoadScene("Trader");
                            break;

                    }
                }
                else
                {
                    foreach (PlayerLocation Loc2 in LocationList)
                    {
                        if (Loc2.CurrDCheck == true)
                        {
                            Loc1.CurrCLCheck = false;
                            Loc2.CurrDCheck = false;
                            Loc2.CurrCLCheck = true;

                            switch(Loc2.LocationName)
                            {
                                case "Base":
                                    SceneLoader.Instance.LoadScene("Base");
                                    break;

                                case "Farm":
                                    SceneLoader.Instance.LoadScene("Farm");
                                    break;

                                case "Police Station":
                                    SceneLoader.Instance.LoadScene("PoliceStation");
                                    break;

                                case "Grocery Store":
                                    SceneLoader.Instance.LoadScene("GroceryStore");
                                    break;

                                case "Hardware Store":
                                    SceneLoader.Instance.LoadScene("HardwareStore");
                                    break;

                                case "Pharmacy":
                                    SceneLoader.Instance.LoadScene("Pharmacy");
                                    break;

                                case "High School":
                                    SceneLoader.Instance.LoadScene("HighSchool");
                                    break;

                                case "Military Checkpoint":
                                    SceneLoader.Instance.LoadScene("MilitaryCheckpoint");
                                    break;

                                case "Trader":
                                    SceneLoader.Instance.LoadScene("Trader");
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }

    //public void ReturningToCL()
    //{
    //    foreach(PlayerLocation Loc in LocationList)
    //    {
    //        if (Loc.CurrCLCheck == true && Loc.CurrDCheck == true)
    //        {
    //            Loc.CurrDCheck = false;
    //        }
    //    }
    //}
}
