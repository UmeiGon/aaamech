using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseTargetCheckerInput : CheckerInput {
    protected override void ActiveTrigger() { }
    protected override void Init()
    {
        EdgeName = EdgeDataBase.GetInstance().FindNodeData<LoseTargetEdgeChecker>().EdgeName;
    }
}
