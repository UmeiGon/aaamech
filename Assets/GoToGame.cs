using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GoToGame : MonoBehaviour
{
    [SerializeField]
    string nextSceneName;
    [SerializeField]
    Button button;
    private void Start()
    {
        button.onClick.AddListener(GoGame);
    }
    public void GoGame()
    {
        TutorialSetter.IsTutorial=(nextSceneName == "tutorial_1");
        // 親シーンの読み込み
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);

        // 加算シーンの読み込み
        SceneManager.LoadScene("CanvasAndManager", LoadSceneMode.Additive);
    }
}
