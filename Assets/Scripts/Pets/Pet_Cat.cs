using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_Cat : Pet
{
    [Tooltip("Reduce all sources of draining. Amt * StaminaDrainReduction.")]
    [Range(0, 1)]public float StaminaDrainReduction;

}
