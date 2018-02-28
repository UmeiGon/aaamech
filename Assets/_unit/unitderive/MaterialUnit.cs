using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialUnit : Unit {


  
    public MaterialID matTag;
    //死んだときにitemのstoneをdropする。
    public GameObject dropObject;
    public override void Death()
    {
        //死ぬ時にitemをdropするか否か
        if (dropObject)
        {
           
            dropObject.gameObject.SetActive(true);
            dropObject.transform.parent = GameObject.Find("Parent").transform;
        }
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
