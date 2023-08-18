using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameGizmos : ScriptableObject
{
    public abstract void Draw(GameObject s, float rad, Color c);
}
