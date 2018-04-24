using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivedAttackedCheckerInput : CheckerInput {
    protected override void ActiveTrigger() { }
    protected override void Init()
    {
        EdgeName = EdgeDataBase.GetInstance().FindNodeData<ReceivedAttackEdgeChecker>().EdgeName;
    }
}
