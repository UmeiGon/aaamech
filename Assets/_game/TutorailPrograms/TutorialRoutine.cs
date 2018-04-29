using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialRoutine : MonoBehaviour {
    public abstract IEnumerator TutorialUpdate();
    public virtual void Init() { }
    public  void StartTutorial()
    {
        Init();
        StartCoroutine(TutorialUpdate());
    }
}
