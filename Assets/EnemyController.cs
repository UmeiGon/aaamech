using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour {
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
    bool returnBase=false;
    private void Start()
    {
        myUnit = GetComponent<CharacterUnit>();
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
        var p =GameObject.Find("Parent");
        unitlists=p.GetComponentInChildren<UnitLists>();
        //StartCoroutine(EnemyUpdate());
    }
    private void Update()
    {
        TargetSelect();
        HomingAndAttack();
    }
    //IEnumerator EnemyUpdate()
    //{
    //    while (true) {
    //        TargetSelect();
    //        HomingAndAttack();
    //        yield return null;
    //    }
    //}
    void HomingAndAttack()
    {
        var startDis = Vector3.Distance(transform.position, startPos);
        if (startDis >= homingMAxRange)
        {
            returnBase = true;
            targetUnit = null;
        }
        if (targetUnit!=null&& Vector3.Distance(transform.position,targetUnit.transform.position)<= AttackRange)
        {
            if(!attackEffect.isPlaying)attackEffect.Play();
            if(!agent.isStopped)agent.isStopped=true;
            Quaternion rot = Quaternion.LookRotation(targetUnit.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 20);
            agent.velocity = Vector3.zero;
            targetUnit.SetDamage(myUnit.attack*Time.deltaTime,myUnit);
        }
        else
        {
            if(attackEffect.isPlaying)attackEffect.Stop();
            if(agent.isStopped)agent.isStopped=false;
            if (returnBase)
            {
                agent.SetDestination(startPos);
                if (startDis<=15.0f)
                {
                    returnBase = false;
                }
            }
            else if(targetUnit!=null)
            {
                agent.SetDestination(targetUnit.transform.position);
            }
            else
            {
                targetUnit = null;
                returnBase = true;
            }      
        }

      
    }
    void TargetSelect()
    {
        if (targetUnit==null)
        {
            var u=unitlists.NearUnitSearch(myUnit, unitlists.playerList);
            //一番近いunitがsesorrange以下だった場合
            if (u!=null&&Vector3.Distance(transform.position,u.transform.position)<=sensorRange)
            {
                targetUnit = u;
            }
        }
    }

}
