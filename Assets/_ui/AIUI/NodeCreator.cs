using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class NodeCreator : MonoBehaviour, IPointerDownHandler
{
    AITreeGenerator aITreeGenerator;
    Action NodeCreatedAction;
    public void AddNodeCreatedAction(Action node_created_action)
    {
        NodeCreatedAction += node_created_action;
    }
    private void Start()
    {
        aITreeGenerator = CompornentUtility.FindCompornentOnScene<AITreeGenerator>();
    }
    public void OnPointerDown(PointerEventData e)
    {
        if (Input.GetKeyDown(KeyCode.Mouse2) && aITreeGenerator.CanEdit)
        {
            aITreeGenerator.CreateDefaultNode(e.position);
            if(NodeCreatedAction!=null)NodeCreatedAction();
        }
    }

}
