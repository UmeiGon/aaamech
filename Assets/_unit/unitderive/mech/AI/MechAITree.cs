using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechAITree {
    public string treeName;
    public List<Command> commandList=new List<Command>();
    public List<CommandEdge> edgeList = new List<CommandEdge>();
    delegate void FirstChanged();
    FirstChanged firstChanged;
    public Command firstCommand;
}
