using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNewGame : MonoBehaviour
{
    public static bool isNewGame;

    private void Awake(){
        isNewGame = false;
    }
}
