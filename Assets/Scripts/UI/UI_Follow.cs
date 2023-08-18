using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Follow : MonoBehaviour
{
    //Script to cause a UI object to follow a Game Object (aka stay on it)
    [SerializeField] Transform objectToFollow;
    RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }


    private void Update() {
        if (objectToFollow != null) //position based on the parent's pos
            rectTransform.localPosition = new Vector3(0, objectToFollow.transform.GetComponent<Collider2D>().bounds.size.y / 2, 0);
    }
}
