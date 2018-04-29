using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResourceCollectionTutorial : TutorialRoutine
{
    [SerializeField]
    Button openAIButton;
    [SerializeField]
    SaveDataButton save1Button;
    AITreeGenerator aITreeGenerator;
    NodeCreator nodeCreator;
    bool nodeCreated;
    Unit clearDueUnit;
    [SerializeField]
    Button SaveLoadChangeButton;
    [SerializeField]
    Button soriteButton;
    [SerializeField]
    Button closeCanvasButton;
    [SerializeField]
    Text commentText;
    [SerializeField]
    Canvas commentCanvas;
    [SerializeField]
    float commentSpeed=0.1f;
    bool AIOpenClicked;
    public override void Init()
    {
        commentCanvas.gameObject.SetActive(true);
        aITreeGenerator = CompornentUtility.FindCompornentOnScene<AITreeGenerator>();
        nodeCreator = CompornentUtility.FindCompornentOnScene<NodeCreator>();
        nodeCreator.AddNodeCreatedAction(NodeCreatedAction);
        soriteButton.onClick.AddListener(SoriteClickedAction);
        SaveLoadChangeButton.onClick.AddListener(SaveLoadClickedAction);
        openAIButton.onClick.AddListener(AIClickedAction);
        if (clearDueUnit == null)
        {
            clearDueUnit = GameObject.Find("ClearEnemy").GetComponent<Unit>();
        }
    }
    IEnumerator CurrentRoutine;
    void CommentApplyStart(string _comment)
    {
        if (CurrentRoutine != null) StopCoroutine(CurrentRoutine);
        CurrentRoutine = CommentApplyRoutine(_comment);
        StartCoroutine(CurrentRoutine);
    }
    IEnumerator CommentApplyRoutine(string _comment)
    {
        commentText.text = "";
        foreach (var i in _comment)
        {
            commentText.text += i;
            yield return new WaitForSeconds(commentSpeed);
        }
    }

    public override IEnumerator TutorialUpdate()
    {
        yield return StartCoroutine(AIOpenedCheck());
        yield return StartCoroutine(SaveDataClickedCheck());
        yield return StartCoroutine(EditClickedCheck());
        yield return StartCoroutine(NodeCreatedCheck());
        yield return StartCoroutine(SaveLoadClickedCheck());
        yield return StartCoroutine(SoriteClickedCheck());
        yield return StartCoroutine(UnitBrokenCheck());
        TutorialEnd();
        yield return null;
    }
    void TutorialEnd()
    {
        CompornentUtility.FindCompornentOnScene<GameClearManager>().GoTitle();
    }
    IEnumerator AIOpenedCheck()
    {
        CommentApplyStart("このゲームはメカのAIを自分で作成し敵を倒すゲームです。右上のボタンを押してメカを作りましょう。");
        while (true)
        {
            if (AIOpenClicked)
            {
                yield break;
            }
            yield return null;
        }
    }
    void AIClickedAction()
    {
        AIOpenClicked = true ;
    }
    IEnumerator SaveDataClickedCheck()
    {
        CommentApplyStart("この画面はAIのデータを保存する画面です。一番上のデータを押してください。");

        while (true)
        {
            if (save1Button.IsSelected)
            {
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator EditClickedCheck()
    {
        CommentApplyStart("編集ボタンを押すことで、保存しているデータを編集することが出来ます。");
        while (true)
        {

            if (aITreeGenerator.CanEdit)
            {
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator NodeCreatedCheck()
    {
        CommentApplyStart("右半分の黒い画面上でマウスのホイールボタンをクリックすることで、AIのノードを作ることが出来ます。");
        while (true)
        {
            if (nodeCreated)
            {
                yield break;
            }
            yield return null;
        }
    }
    void NodeCreatedAction()
    {
        nodeCreated = true;
    }
    bool saveLoadClicked;
    IEnumerator SaveLoadClickedCheck()
    {
        CommentApplyStart("セーブ&ロードボタンを押してAIセーブデータの画面に戻れます。");
        while (true)
        {
            if (saveLoadClicked)
            {
                yield break;
            }
            yield return null;
        }
    }
    void SaveLoadClickedAction()
    {
        saveLoadClicked = true;
    }

    bool soriteClicked;
    IEnumerator SoriteClickedCheck()
    {
        CompornentUtility.FindCompornentOnScene<ItemManager>().itemDataTable[(int)ItemID.Iron].Value = 15;
        CommentApplyStart("出撃ボタンを押すことで、鉄を消費してメカが出撃します。");
        while (true)
        {
            if (soriteClicked)
            {
                yield break;
            }
            yield return null;
        }
    }
    void SoriteClickedAction()
    {
        soriteClicked = true;
    }
    IEnumerator UnitBrokenCheck()
    {
        CommentApplyStart("敵に向かってメカが出撃しました。");
        while (true)
        {
            if (clearDueUnit == null)
            {
                commentText.text = "メカが敵を倒しステージをクリアしました。チュートリアルはこれで終わります。お疲れ様でした。";
                yield return new WaitForSeconds(3.0f);
                yield break;
            }
            yield return null;
        }
    }
}
