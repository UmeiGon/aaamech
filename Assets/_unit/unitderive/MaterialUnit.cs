using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialUnit : Unit {


  
    public MaterialID matTag;
    public override void Death()
    {
        base.Death();
    }
    // Use this for initialization
    protected override void Init()
    {
        base.Init();
    }
    void Start()
    {
        Init();
    }
}
