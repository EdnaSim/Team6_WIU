using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Day_UIManager : MonoBehaviour
{
    [SerializeField]
    private WorldClock WCData;

    [SerializeField]
    private TMP_Text DayCount;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        DayCount.text = "" + WCData.DayCounter;
    }

    // Update is called once per frame
    void Update()
    {
        DayCount.text = "" + WCData.DayCounter;
    }
}
