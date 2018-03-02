using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Reflection;
public class CommandSaveData
{
    public int commandID;
    public Vector2 localPos;
    public int programID;
    public List<int> programValues = new List<int>();
}
public class EdgeSaveData
{
    public int edgeID;
    public int preCommandID;
    public int nextCommandID;
    public int preCheckID;
    public List<int> preValues = new List<int>();
    public int nextCheckID;
    public List<int> nextValues = new List<int>();
}
public class SavePackage
{
    public int firstCommandID;
    public List<CommandSaveData> commandSaveList = new List<CommandSaveData>();
    public List<EdgeSaveData> edgeSaveList = new List<EdgeSaveData>();
}
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

    public void SaveAITree(MechAITree _tree,string file_name)
    {
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + file_name + "saveData" + ".csv", false, Encoding.GetEncoding("Shift_JIS"));
        var filedFlag = (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        //firstcommand
        if (_tree.firstCommand == null)
        {
            sw.WriteLine("0");
        }
        else
        {
            sw.WriteLine(_tree.commandList.IndexOf(_tree.firstCommand).ToString());
        }

        sw.WriteLine("commmand");
        int n = 0;
        foreach (var i in _tree.commandList)
        {
            List<string> slist = new List<string> { n.ToString(), i.holder.localPosition.x.ToString(), i.holder.localPosition.y.ToString(), };
            var str2 = string.Join(",", slist.ToArray());
            sw.WriteLine(str2);
            List<string> proSlist = new List<string> { NodeDataBase.GetInstance().FindTypeNumber(i.program).ToString(), };
            //もしint型のフィールドがあればセーブデータに追加する。
            if (i.program != null)
            {
                var fields = i.program.GetType().GetFields(filedFlag);
                foreach (var f in fields)
                {
                    if (f.FieldType == typeof(int))
                    {
                        //実態から値を取り出す
                        proSlist.Add(f.GetValue(i.program).ToString());
                    }
                }
            }
            var str3 = string.Join(",", proSlist.ToArray());
            sw.WriteLine(str3);
            n++;
        }
        sw.WriteLine("edge");
        //edge
        int y = 0;
        foreach (var i in _tree.edgeList)
        {
            //preとnextともにある場合セーブ
            if (i.pre != null && i.next != null)
            {
                List<string> slist = new List<string> { y.ToString(), _tree.commandList.IndexOf(i.pre).ToString(), _tree.commandList.IndexOf(i.next).ToString(), };
                var str2 = string.Join(",", slist.ToArray());
                sw.WriteLine(str2);
                List<string> preCheckSlist = new List<string> { EdgeDataBase.GetInstance().FindTypeNumber(i.preChecker).ToString(), };
                //もしint型のフィールドがあればセーブデータに追加する。
                if (i.preChecker != null)
                {
                    var fields = i.preChecker.GetType().GetFields(filedFlag);
                    foreach (var f in fields)
                    {
                        if (f.FieldType == typeof(int))
                        {
                            //実態から値を取り出す
                            preCheckSlist.Add(f.GetValue(i.preChecker).ToString());
                        }
                    }
                }
                var str3 = string.Join(",", preCheckSlist.ToArray());
                sw.WriteLine(str3);
                List<string> nextCheckSlist = new List<string> { EdgeDataBase.GetInstance().FindTypeNumber(i.nextChecker).ToString(), };
                //もしint型のフィールドがあればセーブデータに追加する。
                if (i.nextChecker != null)
                {
                    var fields = i.nextChecker.GetType().GetFields(filedFlag);
                    foreach (var f in fields)
                    {
                        if (f.FieldType == typeof(int))
                        {
                            //実態から値を取り出す
                            nextCheckSlist.Add(f.GetValue(i.nextChecker).ToString());
                        }
                    }
                }
                var str4 = string.Join(",", nextCheckSlist.ToArray());
                sw.WriteLine(str4);
            }
            y++;
        }
        sw.Close();
    }
    public SavePackage LoadAITree(string file_name)
    {
        StreamReader sr = new StreamReader(Application.persistentDataPath + file_name + "saveData" + ".csv", Encoding.GetEncoding("Shift_JIS"));
        if (sr == null)
        {
            Debug.Log("ロード失敗");
            return null;
        }
        List<CommandSaveData> comaData = new List<CommandSaveData>();
        string line;
        int firstId = int.Parse(sr.ReadLine());
        sr.ReadLine();
        //nullなるまでループ
        while ((line = sr.ReadLine()) != null)
        {
            if (line == "edge") break;
            string[] comaArray = line.Split(',');
            var c = new CommandSaveData()
            {
                commandID = int.Parse(comaArray[0]),
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
                edgeID = int.Parse(comaArray[0]),
                preCommandID = int.Parse(comaArray[1]),
                nextCommandID = int.Parse(comaArray[2]),
            };

            //pre
            line = sr.ReadLine();
            string[] preArray = line.Split(',');
            //1番目はid
            e.preCheckID = int.Parse(preArray[0]);
            for (int i = 1; i < preArray.Length; i++)
            {
                e.preValues.Add(int.Parse(preArray[i]));
            }
            //next
            line = sr.ReadLine();
            string[] nextArray = line.Split(',');
            //1番目はid
            e.nextCheckID = int.Parse(nextArray[0]);
            for (int i = 1; i < nextArray.Length; i++)
            {
                e.nextValues.Add(int.Parse(nextArray[i]));
            }
            edgeData.Add(e);
        }
        sr.Close();
        return new SavePackage()
        {
            edgeSaveList = edgeData,
            commandSaveList = comaData,
            firstCommandID = firstId,
        };
    }
}
