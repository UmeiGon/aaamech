using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProgram : CommandProgram
{
    public override void ChangeTrigger()
    {
        mechCon.mode = MechController.Mode.Attack;
    }
    public override void Move()
    {
    }
}
