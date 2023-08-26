using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeteoriteUI : MonoBehaviour
{
    public Item meteorite;
    public TMP_Text amountOfMeteorites;
    private void Start()
    {
        amountOfMeteorites.text = meteorite.amount.ToString();
    }

    private void Update()
    {
        amountOfMeteorites.text = meteorite.amount.ToString();
    }

}
