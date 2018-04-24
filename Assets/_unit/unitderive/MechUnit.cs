using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechUnit : CharacterUnit{
    public GameObject selectBall;
    public MechController mechCon;

    protected override void Init()
    {
        mechCon = GetComponent<MechController>();
    }
}
