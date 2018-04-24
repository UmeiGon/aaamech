using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Reflection;


public class AISaveLoad
{
    static AISaveLoad inst;
    public static AISaveLoad GetInstance()
    {
        if (inst == null)
        {
            inst = new AISaveLoad();
        }
        return inst;
    }
    //ai_packをcsvに変換してセーブ
    public void SaveAITree(AIPackage ai_pack, string file_name)
    {
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + file_name + "saveData" + ".csv", false, Encoding.GetEncoding("Shift_JIS"));
        Debug.Log(file_name+"でセーブ");
        var filedFlag = (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        //firstcommand
        sw.WriteLine(ai_pack.firstCommandID);
        sw.WriteLine("node");
        foreach (var i in ai_pack.nodeDataList)
        {
            List<string> slist = new List<string> { i.commandNumber.ToString(), i.localPos.x.ToString(), i.localPos.y.ToString(), };
            var str2 = string.Join(",", slist.ToArray());
            sw.WriteLine(str2);
            List<string> proSlist = new List<string> { i.programID.ToString() };
            //もしint型のフィールドがあればセーブデータに追加する。
            foreach (var f in i.programValues)
            {
                proSlist.Add(f.ToString());
            }

            var str3 = string.Join(",", proSlist.ToArray());
            sw.WriteLine(str3);
        }
        sw.WriteLine("edge");
        //edge
        int y = 0;
        foreach (var i in ai_pack.edgeDataList)
        {
            List<string> slist = new List<string> { i.edgeNumber.ToString(), i.preCommandNumber.ToString(), i.nextCommandNumber.ToString(), };
            var str2 = string.Join(",", slist.ToArray());
            sw.WriteLine(str2);
            List<string> checkerSlist = new List<string> { i.checkrID.ToString(), };

            foreach (var f in i.checkerValues)
            {
                checkerSlist.Add(f.ToString());
            }
            var str4 = string.Join(",", checkerSlist.ToArray());
            sw.WriteLine(str4);

        }
        sw.Close();
    }
    public AIPackage LoadAITree(string file_name)
    {
        StreamReader sr = null;
        try
        {
            sr = new StreamReader(Application.persistentDataPath + file_name + "saveData" + ".csv",Encoding.GetEncoding("Shift_JIS"));
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("ファイルがない");
            return null;
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("ディレクトリがない");
            return null;
        }
        List<NodeSaveData> comaData = new List<NodeSaveData>();
        string line;
        int firstId = int.Parse(sr.ReadLine());
        sr.ReadLine();
        //nullなるまでループ
        while ((line = sr.ReadLine()) != null)
        {
            if (line == "edge") break;
            string[] comaArray = line.Split(',');
            var c = new NodeSaveData()
            {
                commandNumber = int.Parse(comaArray[0]),
                localPos = new Vector2(float.Parse(comaArray[1]), float.Parse(comaArray[2])),
            };
            line = sr.ReadLine();
            string[] proArray = line.Split(',');
            //1番目はid
            c.programID = int.Parse(proArray[0]);
            for (int i = 1; i < proArray.Length; i++)
            {
                c.programValues.Add(int.Parse(proArray[i]));

            }
            comaData.Add(c);
        }

        //edge読み込み
        List<EdgeSaveData> edgeData = new List<EdgeSaveData>();
        while ((line = sr.ReadLine()) != null)
        {
            string[] comaArray = line.Split(',');
            var e = new EdgeSaveData()
            {
                edgeNumber = int.Parse(comaArray[0]),
                preCommandNumber = int.Parse(comaArray[1]),
                nextCommandNumber = int.Parse(comaArray[2]),
            };

            //next
            line = sr.ReadLine();
            string[] nextArray = line.Split(',');
            //1番目はid
            e.checkrID = int.Parse(nextArray[0]);
            for (int i = 1; i < nextArray.Length; i++)
            {
                e.checkerValues.Add(int.Parse(nextArray[i]));
            }
            edgeData.Add(e);
        }
        sr.Close();
        return new AIPackage()
        {
            edgeDataList = edgeData,
            nodeDataList = comaData,
            firstCommandID = firstId,
        };
    }
}
