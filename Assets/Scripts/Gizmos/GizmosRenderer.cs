using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosRenderer : MonoBehaviour
{
    [Multiline] //ignore this, it's for recording what the gizmo renderer is supposed to visualize
    [SerializeField] private string Purpose = "Nil :(";

    [Header("Settings")]
    public GameGizmos gizmo;
    [SerializeField] private GameObject source;
    [SerializeField] private float radius = 1f;
    [SerializeField] private Color colour = Color.red;
    
    private void OnDrawGizmos(){
        
        gizmo.Draw(source, radius, colour);
    }
}
