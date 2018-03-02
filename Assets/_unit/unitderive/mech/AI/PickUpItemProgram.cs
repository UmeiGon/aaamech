using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemProgram : CommandProgram
{
   public  int pickItemID=0;
    public override void ChangeTrigger()
    {
        mechCon.mode = MechController.Mode.PickUpItem;
    }
    public override void Move()
    {
        if (mechCon.targetUnit == null)
        {
            mechCon.targetUnit=mechCon.unitList.NearUnitSearch(mechCon.myUnit, mechCon.unitList.matList.FindAll(x => x.matTag == (MaterialID)pickItemID));
        }
     
    }
}
