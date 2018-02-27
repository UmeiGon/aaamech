using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemProgram : CommandProgram
{
    public override void ChangeTrigger()
    {
        mech.mechCon.mode = MechController.Mode.PickUpItem;
    }
    public override void Move()
    {
        
    }
}
