using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class WorldClock : ScriptableObject
{
    [Header("Default World Time Data")]
    [SerializeField]
    private float DefaultDayCounter = 1;
    [SerializeField]
    private float DefaultWorldTime = 800;

    public float WorldTime = 1;
    public float DayCounter = 800;


    //public float CurrentWT
    //{
    //    get { return WorldTime; }
    //    set { WorldTime = value; }
    //}

    //public float CurrentDC
    //{
    //    get { return DayCounter; }
    //    set { DayCounter = value; }
    //}

    public void Reset()
    {
        WorldTime = DefaultWorldTime;
        DayCounter = DefaultDayCounter;
    }
}
