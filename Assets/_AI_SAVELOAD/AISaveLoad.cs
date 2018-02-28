using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
public struct CommandSaveData
{
    public int commandID;
    public Vector2 localPos;
    public int programID;
}
public struct EdgeSaveData
{
    public int edgeID;
    public int preCommandID;
    public int nextCommandID;
    public int preCheckID;
    public int nextCheckID;
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
   
    public void SaveAITree(MechAITree _tree)
    {
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "saveData" + ".csv", false, Encoding.GetEncoding("Shift_JIS"));
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
            List<string> slist = new List<string> { n.ToString(), i.holder.localPosition.x.ToString(), i.holder.localPosition.y.ToString(), NodeDataBase.GetInstance().FindTypeNumber(i.program).ToString(), };
            var str2 = string.Join(",", slist.ToArray());
            sw.WriteLine(str2);
            n++;
        }
        sw.WriteLine("edge");
        //edge
        int y = 0;
        foreach (var i in _tree.edgeList)
        {
            //preとnextともにある場合
            if (i.pre != null && i.next != null)
            {
                List<string> slist = new List<string> { y.ToString(), _tree.commandList.IndexOf(i.pre).ToString(), _tree.commandList.IndexOf(i.next).ToString(), EdgeDataBase.GetInstance().FindTypeNumber(i.preChecker).ToString(), EdgeDataBase.GetInstance().FindTypeNumber(i.nextChecker).ToString() };
                var str2 = string.Join(",", slist.ToArray());
                sw.WriteLine(str2);
            }
            y++;
        }
        sw.Close();
    }
    public SavePackage LoadAITree()
    {
        StreamReader sr = new StreamReader(Application.persistentDataPath + "saveData" + ".csv", Encoding.GetEncoding("Shift_JIS"));
        if (sr == null)
        {
            Debug.Log("ロード失敗");
            return null;
        }
        List<CommandSaveData> comaData = new List<CommandSaveData>();
        string line;
        int firstId = int.Parse(sr.ReadLine());
        sr.ReadLine();
        // 行がnullじゃない間(つまり次の行がある場合は)、処理をする
        while ((line = sr.ReadLine()) != null )
        {
            if (line == "edge") break;
            string[] comaArray = line.Split(',');
            var c = new CommandSaveData()
            {
                commandID = int.Parse(comaArray[0]),
                localPos = new Vector2(float.Parse(comaArray[1]), float.Parse(comaArray[2])),
                programID = int.Parse(comaArray[3])
            };
            comaData.Add(c);

        }
        List<EdgeSaveData> edgeData = new List<EdgeSaveData>();
        while ((line = sr.ReadLine()) != null)
        {
            string[] comaArray = line.Split(',');
            var e = new EdgeSaveData()
            {
                edgeID = int.Parse(comaArray[0]),
                preCommandID = int.Parse(comaArray[1]),
                nextCommandID = int.Parse(comaArray[2]),
                preCheckID = int.Parse(comaArray[3]),
                nextCheckID = int.Parse(comaArray[4]),
            };
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
