using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButton : MonoBehaviour {
    [SerializeField]
    BuildID bID;
    UIBuildManager bUIManager;
    
    public UnityEngine.UI.Button button;
    public BuildID BID { private set { bID = value; }  get { return bID; } }
    private void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
        bUIManager = GameObject.Find("Parent").GetComponentInChildren<UIBuildManager>();
        
    }
    public void OnClick()
    {
        bUIManager.SelectBuildButton=this;
    }
}
