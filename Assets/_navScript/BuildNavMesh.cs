using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BuildNavMesh : MonoBehaviour {
    NavMeshSurface navSurf;
	// Use this for initialization
	void Awake () {
        navSurf = GetComponent<NavMeshSurface>();
       
    }
    public void BuildNavMeshFunc()
    {
        navSurf.BuildNavMesh();
    }
	// Update is called once per frame
	void Update () {
        
	}
}
