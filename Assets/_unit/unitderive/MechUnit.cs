using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechUnit : CharacterUnit{
    Head head;
    Body body;
    Leg leg;
    public GameObject selectBall;
    public MechController mechCon;
    protected override void Init()
    {
        base.Init();
        mechCon = GetComponent<MechController>();

    }
    private void Start()
    {
        Init();
    }
}
