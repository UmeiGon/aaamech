using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GoToGame : MonoBehaviour
{
    public void GoGame()
    {
        // 親シーンの読み込み
        SceneManager.LoadScene("stage_1", LoadSceneMode.Single);

        // 加算シーンの読み込み
        SceneManager.LoadScene("CanvasAndManager", LoadSceneMode.Additive);
    }
}
