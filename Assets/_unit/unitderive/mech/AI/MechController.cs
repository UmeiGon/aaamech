using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class MechController : MonoBehaviour
{

    public NavMeshAgent agent;
    MechAITree aiTree = null;
    public Unit targetUnit;
    public MechUnit myUnit;
    CommandNode currentCommand;
    public UnitListCabinet unitList;
    NavMeshObstacle navObs;
    Vector3 basePos;
    public ParticleSystem AttackEffect;
    public enum Mode
    {
        Attack, Idle, Chase
    }
    public Mode MechMode { get; private set; }
    public bool attackFlag = false;

    void Start()
    {
        basePos = transform.position;
        MechMode = Mode.Idle;
       
        agent = GetComponent<NavMeshAgent>();
        navObs = GetComponent<NavMeshObstacle>();
        unitList = CompornentUtility.FindCompornentOnScene<UnitListCabinet>();
        currentCommand = aiTree.firstNode;
        Debug.Log("ssa");
        if (aiTree != null)
        {
            if (currentCommand.activity != null) currentCommand.activity.ChangeTrigger();
        }
        StartCoroutine(MoveRoutine());
    }
    public bool TargetAttackRangeInCheck()
    {
        if (!targetUnit) return false;
        return (Vector3.Distance(transform.position, targetUnit.transform.position) <= (myUnit.TotalAttackRange + targetUnit.radius));
    }
    public bool TargetRangeInCheck(float _range)
    {
        if (!targetUnit) return false;
        return (Vector3.Distance(targetUnit.transform.position, transform.position) <= _range);
    }
    public void SetMode(Mode _mode)
    {
        MechMode = _mode;
        switch (_mode)
        {
            case Mode.Attack:
                AttackEffect.Play();
                agent.isStopped = true;
                break;
            case Mode.Idle:
                AttackEffect.Stop();
                agent.isStopped = true;
                break;
            case Mode.Chase:
                AttackEffect.Stop();
                agent.isStopped = false;
                break;
            default:
                break;
        }
    }
    public void SetAITree(MechAITree ai_tree)
    {
        aiTree = ai_tree;
        myUnit = GetComponent<MechUnit>();
        foreach (var i in aiTree.nodeList)
        {
            if (i.activity != null) i.activity.mechCon = this;
        }
        foreach (var i in aiTree.edgeList)
        {
            if (i.checker != null) i.checker.MechCon = this;
        }
        Debug.Log("koi");
    }
    IEnumerator MoveRoutine()
    {
        while (true)
        {
            AIUpdate();
            switch (MechMode)
            {
                case Mode.Attack:
                    if (!targetUnit) break;
                    Quaternion rot = Quaternion.LookRotation(targetUnit.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 20);
                    agent.velocity = Vector3.zero;
                    targetUnit.SetDamage(Time.deltaTime * myUnit.attack, myUnit);
                    break;
                case Mode.Idle:
                    break;
                case Mode.Chase:
                    if (!targetUnit) break;
                    agent.SetDestination(targetUnit.transform.position);
                    break;
                default:
                    break;
            }
            yield return null;
        }
    }
    void AIUpdate()
    {
        bool changed = false;
        foreach (var i in currentCommand.edges)
        {
            if (i.checker.Check())
            {
                currentCommand = i.next;
                changed = true;
                break;
            }
        }
        if (changed)
        {
            targetUnit = null;
            if (currentCommand.activity != null) currentCommand.activity.ChangeTrigger();
        }
        if (currentCommand.activity != null) currentCommand.activity.MoveUpdate();
    }

}
