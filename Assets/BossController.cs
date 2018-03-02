using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossController : MonoBehaviour {

    NavMeshAgent agent;
    CharacterUnit myUnit;
    UnitLists unitlists;
    [SerializeField]
    float sensorRange;
    [SerializeField]
    float homingMAxRange;
    [SerializeField]
    float AttackRange;
    [SerializeField]
    ParticleSystem attackEffect;
    Unit targetUnit;
    Vector3 startPos;
    bool returnBase = false;
    private void Start()
    {
        myUnit = GetComponent<CharacterUnit>();
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
        var p = GameObject.Find("Parent");
        unitlists = p.GetComponentInChildren<UnitLists>();
        //StartCoroutine(EnemyUpdate());
    }
    private void Update()
    {
        TargetSelect();
        HomingAndAttack();
    }
    void HomingAndAttack()
    {
        var startDis = Vector3.Distance(transform.position, startPos);
        if (startDis >= homingMAxRange)
        {
            returnBase = true;
        }
        if (targetUnit != null && Vector3.Distance(transform.position, targetUnit.transform.position) <= AttackRange)
        {
            if (!attackEffect.isPlaying) attackEffect.Play();
            if (!agent.isStopped) agent.isStopped = true;
            var diff = targetUnit.transform.position - transform.position;
            diff.y = 0;
            //頭割りダメージ
            var tLists=unitlists.playerList.FindAll(x=>Vector3.Distance(x.transform.position,transform.position)<= AttackRange+30);
            float damage = myUnit.attack / tLists.Count;
            foreach (var i in tLists)
            {
                i.SetDamage(damage*Time.deltaTime,myUnit);
            }
        }
        else
        {
            if (attackEffect.isPlaying) attackEffect.Stop();
            if (agent.isStopped) agent.isStopped = false;
            if (returnBase)
            {
                agent.SetDestination(startPos);
                if (startDis <= AttackRange)
                {
                    returnBase = false;
                }
            }
            else if (targetUnit != null)
            {
                agent.SetDestination(targetUnit.transform.position);
            }
            else
            {
                returnBase = true;
            }
        }


    }
    void TargetSelect()
    {
        if (targetUnit == null)
        {
            var u = unitlists.NearUnitSearch(myUnit, unitlists.playerList);
            //一番近いunitがsesorrange以下だった場合
            if (u != null && Vector3.Distance(transform.position, u.transform.position) <= sensorRange)
            {
                targetUnit = u;
            }
        }
    }
}
