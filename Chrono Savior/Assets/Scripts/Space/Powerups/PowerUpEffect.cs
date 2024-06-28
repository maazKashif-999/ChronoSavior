using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PowerUpEffect : ScriptableObject
{
    public float speed = 1.5f;
    public abstract void Apply(GameObject target);
}
