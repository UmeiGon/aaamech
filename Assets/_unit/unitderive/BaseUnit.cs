using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : Unit
{
    [SerializeField]
    GameObject selectEffect;
    bool selectEffectIsActive;
    public bool SelectEffectIsActive
    {
        get
        {
            return selectEffectIsActive;
        }
        set
        {
            selectEffectIsActive = value;
            selectEffect.SetActive(value);
        }
    }
}
