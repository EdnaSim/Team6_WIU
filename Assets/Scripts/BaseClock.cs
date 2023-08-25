using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseClock : MonoBehaviour
{
    //World Clock Data
    [SerializeField]
    private WorldClock WCData;
    //Sleep UI Canvas
    [SerializeField]
    private GameObject SleepUICanvas;
    //Sleep UI Day Counter Text
    [SerializeField]
    private TMP_Text TM_DayProgress;
    //Sleep UI Next Day Text
    [SerializeField]
    private TMP_Text TM_NextDay;

    private float MinuteToRealTime = 3.0f;
    private float Timer;

    private bool Sleeping;

    // Start is called before the first frame update
    void Start()
    {
        SleepUICanvas.SetActive(false);
        Timer = MinuteToRealTime;
        Sleeping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Sleeping == false)
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                WCData.WorldTime++;
                if (WCData.WorldTime % 100 >= 60)
                {
                    WCData.WorldTime += 40;
                }
                Timer = MinuteToRealTime;
            }
            if (WCData.WorldTime > 2359)
            {
                Sleeping = true;
            }
        }
        if (Sleeping == true)
        {
            SleepUICanvas.SetActive(true);
            TM_DayProgress.text = "You've made it through Day " + WCData.DayCounter + "!";
            if (WCData.DayCounter < 13)
            {
                TM_NextDay.text = "Once more unto the breach...";
            }
            else if (WCData.DayCounter == 13)
            {
                TM_NextDay.text = "One more day...";
            }
            else
            {
                TM_NextDay.text = "...";
            }
        }

    }

    public void InteractWithBed()
    {
        Sleeping = true;
    }

    public void PlayerSleep()
    {
        WCData.WorldTime = 800;
        WCData.DayCounter++;
        Sleeping = false;
    }
}
