using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMech {
    public List<MechUnit> mechList=new List<MechUnit>();
    static EditMech instance=null;
    public static EditMech GetInstance()
    {
        if (instance!=null)
        {
            instance = new EditMech();
        }
        return instance;
    }
    void GenerateMech()
    {
        mechList.Add(new MechUnit());
    }
}
