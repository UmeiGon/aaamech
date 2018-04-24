using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//LoadしたAItreeの保管庫
public class LoadAIDataCabinet : MonoBehaviour {
    Dictionary<int, MechAITree> loadedTrees=new Dictionary<int, MechAITree>();
    public Dictionary<int, MechAITree> LoadedTrees
    {
        get { return loadedTrees; }
    }
    public void AddLoadTree(int _num,MechAITree ai_tree)
    {
        loadedTrees.Add(_num,ai_tree);
    }
}
