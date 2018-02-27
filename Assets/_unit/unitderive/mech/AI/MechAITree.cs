using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechAITree {
    public List<Command> commandList=new List<Command>();
    public List<CommandEdge> edgeList = new List<CommandEdge>();
    public Command firstCommand;
}
