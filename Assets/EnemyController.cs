using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    CharacterUnit myUnit;
    UnitListCabinet unitlists;
    [SerializeField]
    float sensorRange;
    [SerializeField]
    float chaseMaxRange;
    [SerializeField]
    ParticleSystem attackEffect;
    Unit targetUnit;
    Vector3? startPos=null;
    enum EnemyActivity
    {
        ChaseTarget,
        AttackTarget,
        ReturnToBase,
        IdleOnBase,
    }

    EnemyActivity activity = EnemyActivity.IdleOnBase;
    void ChangeActivity(EnemyActivity next_activity)
    {
        if (activity == EnemyActivity.AttackTarget)
        {
           
        }
        switch (next_activity)
        {
            case EnemyActivity.AttackTarget:
                attackEffect.Play();
                agent.isStopped = true;
                break;
            case EnemyActivity.ChaseTarget:
                attackEffect.Stop();
                agent.isStopped = false;
                break;
            case EnemyActivity.ReturnToBase:
                attackEffect.Stop();
                agent.isStopped = false;
                targetUnit = null;
                break;
            case EnemyActivity.IdleOnBase:
                attackEffect.Stop();
                agent.isStopped = true;
                targetUnit = null;
                break;
        }
        Debug.Log(next_activity);
        activity = next_activity;
    }
    private void Start()
    {
        myUnit = GetComponent<CharacterUnit>();
        agent = GetComponent<NavMeshAgent>();
        unitlists = CompornentUtility.FindCompornentOnScene<UnitListCabinet>();
    }
    private void Update()
    {
        Move();
    }
    void CheckReturnToBase()
    {
        var startDis = Vector3.Distance(transform.position, (Vector3)startPos);
        if (startDis >= chaseMaxRange)
        {
            ChangeActivity(EnemyActivity.ReturnToBase);
        }

    }
    void Move()
    {
        if(startPos==null)startPos = transform.position;

        //targetUnitがいない場合帰る
        if (!targetUnit &&( activity != EnemyActivity.ReturnToBase && activity != EnemyActivity.IdleOnBase))
        {
            ChangeActivity(EnemyActivity.ReturnToBase);
        }

        //必要な情報を計算
        float targetDis = 0;
        if (targetUnit)
        {
            targetDis = Vector3.Distance(targetUnit.transform.position, transform.position);
        }
        var startDis = Vector3.Distance((Vector3)startPos, transform.position);

        //各々の状態の動き
        switch (activity)
        {
            case EnemyActivity.AttackTarget:
                //範囲外にターゲットが行った場合追跡
                if (targetUnit && targetDis > myUnit.TotalAttackRange)
                {
                    ChangeActivity(EnemyActivity.ChaseTarget);
                }
                else
                {
                    Quaternion rot = Quaternion.LookRotation(targetUnit.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 20);
                    agent.velocity = Vector3.zero;
                    targetUnit.SetDamage(myUnit.attack * Time.deltaTime, myUnit);
                }
                break;
            case EnemyActivity.ChaseTarget:
                if (targetUnit && targetDis <= myUnit.TotalAttackRange)
                {
                    ChangeActivity(EnemyActivity.AttackTarget);
                }
                else
                {
                    agent.SetDestination(targetUnit.transform.position);
                }
                CheckReturnToBase();
                break;
            case EnemyActivity.ReturnToBase:
                agent.SetDestination((Vector3)startPos);
                if (startDis <= 3.0f)
                {
                    ChangeActivity(EnemyActivity.IdleOnBase);
                }
                break;
            case EnemyActivity.IdleOnBase:
                TargetSelect();
                if (targetUnit)
                {
                    ChangeActivity(EnemyActivity.ChaseTarget);
                }
                break;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, sensorRange);
    }
    void TargetSelect()
    {
        var u = unitlists.SearchNearUnit(myUnit, unitlists.PlayerList);
        //一番近いunitがsesorrange以下だった場合
        if (u != null && Vector3.Distance(transform.position, u.transform.position) <= sensorRange)
        {
            targetUnit = u;
        }
    }

}
