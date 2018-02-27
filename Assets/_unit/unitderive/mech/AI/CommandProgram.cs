using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandProgram {
    //自分を持っているこんとろーらー
    public MechController mechCon = null;
    public delegate void NameChanged(string _name);
    public NameChanged nameChanged;
    //ノード切り替わりのタイミングで呼ぶ
    public abstract void ChangeTrigger();
    public abstract void Move();
}
