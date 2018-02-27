using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildProgram : CommandProgram {

    public override void ChangeTrigger()
    {
        mechCon.mode = MechController.Mode.Build;
    }
    public override void Move()
    {
        
    }
}
