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
    Vector3 basePos;
    public ParticleSystem AttackEffect;
    public enum Mode
    {
        Attack, PickUpItem,Build, Idle,ReturnBase
    }
    public Mode mode;
    public bool attackFlag = false;
    
    void Start()
    {
        basePos = transform.position;
        mode = Mode.Idle;
        myUnit = GetComponent<MechUnit>();
        agent = GetComponent<NavMeshAgent>();
        navObs = GetComponent<NavMeshObstacle>();
        var pare = GameObject.Find("Parent");
        dropItemMane = pare.GetComponentInChildren<DropItemManager>();
        unitList = pare.GetComponentInChildren<UnitLists>();
        if (aiTree != null)
        {
            if(nowCommand.program!=null)nowCommand.program.ChangeTrigger();
        }
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
            case Mode.PickUpItem:
                if (_unit is MaterialUnit)
                {
                    targetUnit = _unit;
                }
                break;
            default:
                break;
        }
        if (targetUnit!=null)
        {
            agent.updatePosition = true;
            agent.SetDestination(targetUnit.transform.position);
        }
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
            if (i.checker != null) i.checker.mech = this;
        }
        nowCommand = aiTree.firstCommand;
    }
    void AIUpdate()
    {
        bool changed=false;
        foreach (var i in nowCommand.edges)
        {
            //preが自分でnextがちゃんとある場合
            if (i.pre== nowCommand && i.next != null &&i.checker!=null &&i.checker.Check())
            {
                nowCommand = i.next;
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
    private void Update()
    {

        AIUpdate();
        //アイテム収集処理（仮）
        //dropItemMane.GetDropItems(transform.position);
        if (mode == Mode.ReturnBase)
        {
            agent.SetDestination(basePos);
            if (AttackEffect.isPlaying) AttackEffect.Stop();
            if (!agent.isStopped) agent.isStopped = false;
        }
        else if (mode != Mode.Idle)
        {
            if (targetUnit != null)
            {
                //前のtargetpositionと違っていたら更新
                agent.SetDestination(targetUnit.transform.position);

                var dis = Vector3.Distance(transform.position, targetUnit.transform.position);
                //attack中
                if (dis <= (45.0f + targetUnit.radius))
                {
                    //トリガー的処理
                    if (!AttackEffect.isPlaying)
                    {
                        AttackEffect.Play();
                        Debug.Log("attack");
                    }
                    if (agent.isStopped) agent.isStopped = true;
                    Quaternion rot = Quaternion.LookRotation(targetUnit.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 20);
                    agent.velocity = Vector3.zero;
                    targetUnit.SetDamage(Time.deltaTime * myUnit.attack, myUnit);
                }
                else
                {
                    if (AttackEffect.isPlaying)
                    {
                        AttackEffect.Stop();
                        Debug.Log("stop");
                    }
                }
            }
            else
            {
                if (AttackEffect.isPlaying) AttackEffect.Stop();
                if (!agent.isStopped) agent.isStopped = false;

            }
        }
    }

}
