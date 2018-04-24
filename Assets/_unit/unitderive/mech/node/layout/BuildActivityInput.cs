﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildActivityInput : ActivityInput {
    protected override void ActiveTrigger() { }
    protected override void Init()
    {
        AcitivityName = NodeDataBase.GetInstance().FindNodeData<BuildNodeActivity>().NodeName;
    }
}
