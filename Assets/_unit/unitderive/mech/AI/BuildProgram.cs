﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildProgram : CommandProgram {

    public override void ChangeTrigger()
    {
        mechCon.mode = MechController.Mode.Build;
    }
    public override void Move()
    {
        mechCon.unitList.NearUnitSearch(mechCon.myUnit,mechCon.unitList.buildList);
    }
}
