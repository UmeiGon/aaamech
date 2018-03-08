using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SpwanEnemyTower : MonoBehaviour {
    [SerializeField]
    GameObject spwanUnitPre;
    [SerializeField]
    int maxSpwanValue;
    [SerializeField]
    float spwanTime;
    [SerializeField]
    float spwanRange;
    GameObject[] spwanList;
    Transform parentObj;
    private void Start()
    {
        spwanList = new GameObject[maxSpwanValue];
        parentObj = GameObject.Find("Parent").transform;
        StartCoroutine(SpwanUpdate());
    }
    IEnumerator SpwanUpdate()
    {
        while (true)

        {
            for(int i=0;i<spwanList.Length;i++){
                if (spwanList[i] == null)
                {
                    yield return new WaitForSeconds(spwanTime);
                    double d = ((float)i / (float)(maxSpwanValue)) * Math.PI * 2;
                    Vector3 s_pos = new Vector3((float)Math.Cos(d)*spwanRange, 0, (float)Math.Sin(d) * spwanRange);
                    spwanList[i]=Instantiate(spwanUnitPre, transform.position+s_pos, new Quaternion(), parentObj); 
                }
            }
            yield return null;
        }
    }

}
