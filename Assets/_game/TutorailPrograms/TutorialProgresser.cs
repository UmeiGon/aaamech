using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialProgresser : MonoBehaviour
{
    [SerializeField]
    TutorialRoutine tutorial;
    private void Start()
    {
        if (!TutorialSetter.IsTutorial) return;
        tutorial.StartTutorial();
    }
}
