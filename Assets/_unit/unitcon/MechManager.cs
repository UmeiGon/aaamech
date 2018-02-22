using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechManager : MonoBehaviour
{
    private UnitLists unitList;
    public Unit selectUnit;
    List<MechUnit> mechList;
    public List<MechUnit> selectMechList = new List<MechUnit>();
    public delegate void SelectTriggerFuncs();
    public SelectTriggerFuncs selectTriggerFuncs;
    public List<MechUnit> MechList{ get { return mechList; } }
    void Start()
    {
        unitList= GameObject.Find("Parent").GetComponentInChildren<UnitLists>();
        mechList = unitList.playerMechList;
        StartCoroutine(Command());
    }
    void CommandAttack()
    {

        //foreach (var i in selectMechList)
        //{
        //    var mech_con = i.GetComponent<mechController>();
        //    mech_con.mode = mechController.Mode.Attack;
            
        //    mech_con.SetTarget(selectUnit);
        //}
    }
  
    IEnumerator Command()
    {
        //while (true)
        //{
        //    if (Input.GetKeyDown(KeyCode.Alpha1) && selectUnit)
        //    {
        //        CommandAttack();
        //    }
        //    yield return null;
        //}
        yield return null;
    }
 
    // Update is called once per frame

}
