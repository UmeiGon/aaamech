using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitHpBar : MonoBehaviour {
    Unit myUnit;
    [SerializeField]
    Slider mySlider;
    private void Start()
    {
        myUnit = GetComponent<Unit>();
        StartCoroutine(UnitHpBarApply());
    }
    IEnumerator UnitHpBarApply()
    {
        while (true)
        {
            mySlider.transform.LookAt(Camera.main.transform);
            mySlider.value = myUnit.Helth/myUnit.maxHelth;
            yield return null;
        }
    }
}
