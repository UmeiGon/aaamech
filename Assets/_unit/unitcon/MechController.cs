using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class MechController : MonoBehaviour
{

    NavMeshAgent agent;
    public Unit targetUnit;
    public MechUnit myUnit;
    UnitLists unitList;
    DropItemManager dropItemMane;
    NavMeshObstacle navObs;
    [SerializeField] ParticleSystem AttackEffect;
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
    public void SetMode(int _mode)
    {
        targetUnit = null;
        mode = (Mode)_mode;
    }
    IEnumerator MechMove()
    {
        while (true)
        {
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
                        targetUnit.Helth -= Time.deltaTime * myUnit.attack;
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
                    switch (mode)
                    {
                        case Mode.Attack:
                            targetUnit = unitList.NearUnitSearch(myUnit, unitList.enemyList);
                            break;
                        case Mode.PickUpItem:          
                            targetUnit = unitList.NearUnitSearch(myUnit, unitList.matList);
                            break;
                        case Mode.Build:
                            targetUnit = unitList.NearUnitSearch(myUnit, unitList.buildList);
                            break;
                    }
                   
                }
            }
   

            yield return null;
        }

    }
}
