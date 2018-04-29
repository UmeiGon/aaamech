using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameClearManager : MonoBehaviour
{
    Unit ClearUnit;
    [SerializeField]
    Canvas clearCanvas;
    [SerializeField]
    Text clearComment;
    private void Start()
    {
        var obj=GameObject.Find("ClearUnit");
        if(obj)ClearUnit = obj.GetComponent<Unit>();
        if (ClearUnit)
        {
            StartCoroutine(ClearCheckRoutine());
        }
    }
    IEnumerator ClearCheckRoutine()
    {
        while (true)
        {
            if (ClearUnit == null)
            {
                StartCoroutine(ClearRoutine());
            }
            yield return null;
        }
    }
    IEnumerator ClearRoutine()
    {
        clearCanvas.gameObject.SetActive(true);
        clearComment.text = "ゲームクリア";
        yield return new WaitForSeconds(2.5f);
        Clear();
        yield return null;

    }
    void Clear()
    {
        GoTitle();
    }
    public void GoTitle()
    {
        SceneManager.LoadScene("Title");
    }
    public void ResetStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);

        // 加算シーンの読み込み
        SceneManager.LoadScene("CanvasAndManager", LoadSceneMode.Additive);
    }
}
