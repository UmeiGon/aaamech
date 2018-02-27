using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class AISaveLoad : MonoBehaviour
{
    public struct CommandData{
        public int commandID;
        public Vector2 localPos;
        public int programID;
        List<int> edgeIDList;
    }
    public struct EdgeData {
        public int edgeID;
        public int preCommandID;
        public int nextCommandID;
        public int preCheckID;
        public int nextCheckID;
    }
    public void SaveAITree(MechAITree _tree)
    {

    }
    // データ型の定義
    public struct HipData
    {
        public int hipId;
        public Vector3 pos;
        public Color color;
        public float magnitude; // 等級

        public HipData(int _id, Vector3 _pos, Color _color, float _magnitude)
        {
            hipId = _id;
            pos = _pos;
            color = _color;
            magnitude = _magnitude;
        }
        public HipData(HipData _data)
        {
            hipId = _data.hipId;
            pos = _data.pos;
            color = _data.color;
            magnitude = _data.magnitude;
        }
    }

    // 読み込むデータ
    [SerializeField] TextAsset lightFile = null;
    // 読み込んだデータを格納するリスト
    public List<HipData> hipList;

    // Use this for initialization
    void Start()
    {
        hipList = createHipList(lightFile);
    }

    // Update is called once per frame
    void Update()
    {
        if (hipList != null)
        {
            foreach (HipData hip in hipList)
            {
                Debug.DrawLine(Vector3.zero, hip.pos * 500f, Color.white * (1f / hip.magnitude));
            }
        }
    }

    // ファイルからデータを読み込みデータリストに格納する
    List<HipData> createHipList(TextAsset _lightsFile)
    {
        List<HipData> list = new List<HipData>();
        StringReader sr = new StringReader(_lightsFile.text);
        while (sr.Peek() > -1)
        {
            string lineStr = sr.ReadLine();
            HipData data;
            if (stringToHipData(lineStr, out data))
            {
                list.Add(data);
            }
        }
        sr.Close();
        return list;
    }

    // CSV文字列からデータ型に変換
    bool stringToHipData(string _hipStr, out HipData data)
    {
        bool ret = true;
        data = new HipData();
        // カンマ区切りのデータを文字列の配列に変換
        string[] dataArr = _hipStr.Split(',');
        try
        {
            // 文字列をint,floatに変換する
            int hipId = int.Parse(dataArr[0]);
            float hlH = float.Parse(dataArr[1]);
            float hlM = float.Parse(dataArr[2]);
            float hlS = float.Parse(dataArr[3]);
            int hsSgn = int.Parse(dataArr[4]);
            float hsH = float.Parse(dataArr[5]);
            float hsM = float.Parse(dataArr[6]);
            float hsS = float.Parse(dataArr[7]);
            float mag = float.Parse(dataArr[8]);
            Color col = Color.gray;
            float hDeg = (360f / 24f) * (hlH + hlM / 60f + hlS / 3600f);
            float sDeg = (hsH + hsM / 60f + hsS / 3600f) * (hsSgn == 0 ? -1f : 1f);
            Quaternion rotL = Quaternion.AngleAxis(hDeg, Vector3.up);
            Quaternion rotS = Quaternion.AngleAxis(sDeg, Vector3.right);
            Vector3 pos = rotL * rotS * Vector3.forward;
            data = new HipData(hipId, pos, Color.white, mag);
        }
        catch
        {
            ret = false;
            Debug.Log("data err");
        }
        return ret;
    }
}
