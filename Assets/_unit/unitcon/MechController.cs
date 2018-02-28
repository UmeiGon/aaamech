using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class MechController : MonoBehaviour
{

    public NavMeshAgent agent;
    MechAITree aiTree=null;
    public Unit targetUnit;
    public MechUnit myUnit;
    Command nowCommand;
    public UnitLists unitList;
    DropItemManager dropItemMane;
    NavMeshObstacle navObs;
    public ParticleSystem AttackEffect;
    public enum Mode
    {
        Attack, PickUpItem,Build, Idle
    }
    public Mode mode;
    public bool attackFlag = false;
    private Vector3 preTargetPos;
    
    void Start()
    {
        mode = Mode.Idle;
        myUnit = GetComponent<MechUnit>();
        agent = GetComponent<NavMeshAgent>();
        navObs = GetComponent<NavMeshObstacle>();
        var pare = GameObject.Find("Parent");
        dropItemMane = pare.GetComponentInChildren<DropItemManager>();
        unitList = pare.GetComponentInChildren<UnitLists>();
        if (aiTree != null)
        {
            nowCommand = aiTree.firstCommand;
            if(nowCommand.program!=null)nowCommand.program.ChangeTrigger();
        }
        StartCoroutine(MechMove());
    }
    public void SetTarget(Unit _unit)
    {
        switch (mode)
        {
            case Mode.Attack:
                if (_unit is CharacterUnit&&_unit.armyTag!=Unit.Army.p1)
                {
                    targetUnit = _unit;
                }
                break;
            case Mode.Build:
                if (_unit is BuildUnit)
                {
                    targetUnit = _unit;
                }
                break;
            case Mode.Idle:
                break;
            case Mode.PickUpItem:
                if (_unit is MaterialUnit)
                {
                    targetUnit = _unit;
                }
                break;
        }
        if (targetUnit!=null)
        {
            agent.updatePosition = true;
            agent.SetDestination(targetUnit.transform.position);
        }
    }
    public void Attack()
    {

    }
    public void SetMode(int _mode)
    {
        targetUnit = null;
        mode = (Mode)_mode;
    }
    public void SetAITree(MechAITree _tree)
    {
        aiTree = _tree;
        foreach (var i in aiTree.commandList)
        {
            if(i.program!=null)i.program.mechCon = this;
        }
        foreach (var i in aiTree.edgeList)
        {
            if (i.nextChecker != null) i.nextChecker.mech = this;
        }
        foreach (var i in aiTree.edgeList)
        {
            if (i.preChecker != null) i.preChecker.mech = this;
        }
    }
    void AIUpdate()
    {
        bool changed=false;
        foreach (var i in nowCommand.edges)
        {
            //preが自分でnextがちゃんとある場合
            if (i.pre== nowCommand && i.next != null &&i.nextChecker!=null &&i.nextChecker.Check())
            {
                nowCommand = i.next;
                changed = true;
                break;
            }
            if (i.next == nowCommand && i.pre != null && i.preChecker!=null && i.preChecker.Check())
            {
                nowCommand = i.pre;
                changed = true; 
                break;
            }
        }
        if (changed)
        {
            targetUnit = null;
            if(nowCommand.program!=null)nowCommand.program.ChangeTrigger();
        }
        if (nowCommand.program != null) nowCommand.program.Move();
    }
    IEnumerator MechMove()
    {


        while (true)
        {
            AIUpdate();
            //アイテム収集処理（仮）
            dropItemMane.GetDropItems(transform.position);

            if (mode != Mode.Idle)
            {
                if (targetUnit!=null)
                {
                    //前のtargetpositionと違っていたら更新
                    if (preTargetPos != targetUnit.transform.position) agent.SetDestination(targetUnit.transform.position);
                    preTargetPos = targetUnit.transform.position;

                    //10.0f以内に到達したら
                    var dis = Vector3.Distance(transform.position, targetUnit.transform.position);
                   
                    //attack中
                    if (dis<=45.0f)
                    {
                        //トリガー的処理
                        if(!AttackEffect.isPlaying)AttackEffect.Play();
                        if (agent.isStopped) agent.isStopped = true;
                        Quaternion rot = Quaternion.LookRotation(targetUnit.transform.position-transform.position);
                        transform.rotation= Quaternion.Slerp(transform.rotation,rot,Time.deltaTime*20);
                        targetUnit.SetDamage(Time.deltaTime * myUnit.attack,myUnit);
                        agent.velocity = Vector3.zero;
                    }
                    else
                    {
                        if (AttackEffect.isPlaying) AttackEffect.Stop();
                    }
                }
                else
                {
                    if (AttackEffect.isPlaying) AttackEffect.Stop();
                    if (!agent.isStopped) agent.isStopped = false;
                   
                }
            }
   

            yield return null;
        }

    }
}
