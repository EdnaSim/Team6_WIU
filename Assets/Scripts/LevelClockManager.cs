using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClockManager : MonoBehaviour
{
    [SerializeField]
    private WorldClock WCData;

    [SerializeField]
    private GameObject TM_WarningDisplay;

    private float MinuteToRealTime = 3.0f;
    private float Timer;

    // Start is called before the first frame update
    void Start()
    {
        Timer = MinuteToRealTime;
        TM_WarningDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
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

        if (WCData.WorldTime >= 2200)
        {
            TM_WarningDisplay.SetActive(true);
        }
        else if (WCData.WorldTime > 2359)
        {
            //Insert your code for that passing out animation here.
            SceneLoader.Instance.LoadScene("Base");
        }
    }
}
