using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Time_UIManager : MonoBehaviour
{
    [SerializeField]
    private WorldClock WCData;

    [SerializeField]
    private TMP_Text TimeDisplay;

    // Start is called before the first frame update
    void Start()
    {
        if (WCData.WorldTime < 1000)
        {
            TimeDisplay.text = "0" + WCData.WorldTime;
        }
        else
        {
            TimeDisplay.text = "" + WCData.WorldTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (WCData.WorldTime < 1000)
        {
            TimeDisplay.text = "0" + WCData.WorldTime;
        }
        else
        {
            TimeDisplay.text = "" + WCData.WorldTime;
        }
    }
}
