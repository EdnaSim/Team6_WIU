using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class PlayerLocation : ScriptableObject
{
    [Header("Default Location Data")]
    [SerializeField]
    public string LocationName;
    [SerializeField]
    private bool DefaultCurrentLocationCheck;
    [SerializeField]
    private bool DefaultDestinationCheck;

    public bool CurrentLocationCheck;
    public bool CurrentDestinationCheck;

    public bool CurrCLCheck
    {
        get { return CurrentLocationCheck; }
        set { CurrentLocationCheck = value; }
    }

    public bool CurrDCheck
    {
        get { return CurrentDestinationCheck; }
        set { CurrentDestinationCheck = value; }
    }

    public void Reset()
    {
        CurrentLocationCheck = DefaultCurrentLocationCheck;
        CurrentDestinationCheck = DefaultDestinationCheck;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
