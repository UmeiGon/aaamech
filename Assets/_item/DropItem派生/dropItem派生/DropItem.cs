using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour {
    public int dropValue = 1;
    public float pickUpDelay = 1.0f;
    private PlayerItemManager pItemMane;
    private GameObject pare;
 
    public bool IsLife { private set; get; }
    private void Awake()
    {
        IsLife = true;
    }
    private void Start()
    {
       
        Init();
        StartCoroutine(TimerUpdate());
    }
    IEnumerator TimerUpdate()
    {
        while (true)
        {
            if (!IsLife) Destroy(gameObject);
            pickUpDelay -= Time.deltaTime;
            yield return null;
        }
    }
    virtual protected void Init()
    {
        pare = GameObject.Find("Parent");
        pare.GetComponentInChildren<DropItemManager>().dropItemList.Add(this);
    }
    public  void Picked() {
        if (pickUpDelay > 0)return;
       
        IsLife = false;
    }
}
