using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WireSphereGizmos : GameGizmos
{
    public override void Draw(GameObject s, float rad, Color c){
        Gizmos.color = c;
        Gizmos.DrawWireSphere(s.transform.position, rad);
    }
}
