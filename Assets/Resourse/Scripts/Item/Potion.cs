using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion : MonoBehaviour
{
    public abstract void Use();
    public abstract int GetUsageCount();
    public abstract float GetDuration();
}

