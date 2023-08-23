using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimController : MonoBehaviour
{
    public static readonly string[] WalkDirections = { "Player_N_Walk", "Player_NE_Walk", "Player_E_Walk", "Player_SE_Walk", "Player_S_Walk", "Player_SW_Walk", "Player_W_Walk", "Player_NW_Walk" };
    public static readonly string[] MeleeDirections = { "Player_N_Melee", "Player_NE_Melee", "Player_E_Melee", "Player_SE_Melee", "Player_S_Melee", "Player_SW_Melee", "Player_W_Melee", "Player_NW_Melee" };
    public static readonly string[] HurtDirections = { "Player_N_Hurt", "Player_NE_Hurt", "Player_E_Hurt", "Player_SE_Hurt", "Player_S_Hurt", "Player_SW_Hurt", "Player_W_Hurt", "Player_NW_Hurt" };
    public static readonly string[] DeathDirections = { "Player_N_Death", "Player_NE_Death", "Player_E_Death", "Player_SE_Death", "Player_S_Death", "Player_SW_Death", "Player_W_Death", "Player_NW_Death" };

    Animator ar;
    int LastDir;
    // Start is called before the first frame update
    void Start()
    {
        ar = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 dir) {
        string[] dirArray = WalkDirections;
        if (dir.magnitude > .01f)
            LastDir = DirToIndex(dir, 8);

        ar.Play(dirArray[LastDir]);
    }

    public int DirToIndex(Vector2 dir, int NumOfDirs) {
        //get the normalized direction
        Vector2 normDir = dir.normalized;
        //calculate how many degrees one slice is
        float step = 360f / NumOfDirs;
        //calculate how many degress half a slice is.
        //we need this to offset the pie, so that the North (UP) slice is aligned in the center
        float halfstep = step / 2;
        //get the angle from -180 to 180 of the direction vector relative to the Up vector.
        //this will return the angle between dir and North.
        float angle = Vector2.SignedAngle(Vector2.up, normDir);
        //add the halfslice offset
        angle += halfstep;
        //if angle is negative, then let's make it positive by adding 360 to wrap it around.
        if (angle < 0) angle += 360;
        //calculate the amount of steps required to reach this angle
        float stepCount = angle / step;

        if (Mathf.FloorToInt(stepCount)-1 < 0)
            return WalkDirections.Length - 1;
        return Mathf.FloorToInt(stepCount);
    }
}
