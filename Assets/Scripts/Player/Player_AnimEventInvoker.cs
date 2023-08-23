using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_AnimEventInvoker : MonoBehaviour
{
    public UnityEvent OnDeathAnimOver, StartMelee, StartRanged, DoneAttacking;

    public void DeathAnimOver() {
        OnDeathAnimOver?.Invoke();
    }
    public void TriggerMelee() {
        StartMelee?.Invoke();
    }
    public void TriggerRanged() {
        StartRanged.Invoke();
    }
    public void TriggerDoneAttack() {
        DoneAttacking?.Invoke();
    }
}
