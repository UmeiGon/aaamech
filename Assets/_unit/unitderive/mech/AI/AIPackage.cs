using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
public class CommandSaveData
{
    public int commandNumber;
    public Vector2 localPos;
    public int programID;
    public List<int> programValues = new List<int>();
}
public class EdgeSaveData
{
    public int edgeNumber;
    public int preCommandNumber;
    public int nextCommandNumber;
    public int checkrID;
    public List<int> checkerValues = new List<int>();
}
public class AIPackage  {

    public static AIPackage TreeToPackage(MechAITree _tree)
    {
        AIPackage retPack = new AIPackage();
          var filedFlag = (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        //firstcommand設定
        if (_tree.firstCommand == null)
        {
            retPack.firstCommandID=0;
        }
        else
        {
            retPack.firstCommandID= _tree.commandList.IndexOf(_tree.firstCommand);
        }
        //command設定
        int n = 0;
        foreach (var i in _tree.commandList)
        {
            CommandSaveData cData = new CommandSaveData
            {
                commandNumber = n,
                localPos = i.holder.transform.localPosition,
                programID = NodeDataBase.GetInstance().FindTypeNumber(i.program),
                programValues = new List<int>()
            };
            //もしint型のフィールドがあればデータに追加する。
            if (i.program != null)
            {
                var fields = i.program.GetType().GetFields(filedFlag);
                foreach (var f in fields)
                {
                    if (f.FieldType == typeof(int))
                    {
                        //実態から値を取り出す
                        cData.programValues.Add((int)f.GetValue(i.program));
                    }
                }
            }
            retPack.commandDataList.Add(cData);
            n++;
        }


        //edge設定
        int y = 0;
        foreach (var i in _tree.edgeList)
        {
            //preとnextともにある場合セーブ
            if (i.pre != null && i.next != null)
            {
                EdgeSaveData eData = new EdgeSaveData
                {
                    edgeNumber = y,
                    preCommandNumber = _tree.commandList.IndexOf(i.pre),
                    nextCommandNumber = _tree.commandList.IndexOf(i.next),
                    checkrID = EdgeDataBase.GetInstance().FindTypeNumber(i.checker)
                };
         
                //もしint型のフィールドがあればセーブデータに追加する。
                if (i.checker != null)
                {
                    var fields = i.checker.GetType().GetFields(filedFlag);
                    foreach (var f in fields)
                    {
                        if (f.FieldType == typeof(int))
                        {
                            //実態から値を取り出す
                           eData.checkerValues.Add((int)f.GetValue(i.checker));
                        }
                    }
                }
                retPack.edgeDataList.Add(eData);
                y++;
            }     
        }
        return retPack;
    }
    public int firstCommandID;
    public List<CommandSaveData> commandDataList = new List<CommandSaveData>();
    public List<EdgeSaveData> edgeDataList = new List<EdgeSaveData>();
}
