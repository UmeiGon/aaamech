using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  BuilderSuperClass
{
    GameObject buildObj;
    //ビルドが終わったらfalseを返す
    public abstract bool BuilderUpdate();
}
