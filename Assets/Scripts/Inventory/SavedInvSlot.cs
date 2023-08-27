using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SavedInvSlot
{
    public string ItemName;
    public int ItemCount;

    //constructor
    public SavedInvSlot(string itemname, int itemcount) {
        ItemName = itemname;
        ItemCount = itemcount;
    }
}
