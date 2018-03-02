using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnBaseProgram : CommandProgram {
    public override void ChangeTrigger()
    {
        mechCon.mode = MechController.Mode.ReturnBase;
    }
    public override void Move()
    {
    }
}
