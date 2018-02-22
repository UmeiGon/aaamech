using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButton : MonoBehaviour {
    [SerializeField]
    BuildID bID;
    [SerializeField]
    BuildManager bManager;
    public UnityEngine.UI.Button button;
    public BuildID BID { private set { bID = value; }  get { return bID; } }
    private void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
        bManager = GameObject.Find("Parent").GetComponentInChildren<BuildManager>();
    }
    public void OnClick()
    {
        bManager.BuildNum = bID;
    }
}
