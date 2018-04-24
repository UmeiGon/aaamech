using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialProgresser : MonoBehaviour
{
    [SerializeField]
    bool gameIsTutorial;
    bool GameIsTutorial
    {
        get { return gameIsTutorial; }
        set { gameIsTutorial = value; }
    }
    private void Awake()
    {
        if (!gameIsTutorial) return;
        CompornentUtility.FindCompornentOnScene<SaveDataButtonManager>().ChangeArticleName("Tutorial");
    }
}
