using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandProgram {
    public MechUnit mech = null;
    public string commandName="default";
    //ノード切り替わりのタイミングで呼ぶ
    public abstract void ChangeTrigger();
    public abstract void Move();
}
