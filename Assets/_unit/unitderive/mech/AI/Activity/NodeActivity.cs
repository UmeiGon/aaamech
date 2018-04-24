using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeActivity {
    //自分を持っているコントローラー
    public MechController mechCon = null;
    //ノード切り替わりのタイミングで呼ぶ
    public abstract void ChangeTrigger();
    public abstract void MoveUpdate();
}
