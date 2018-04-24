using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnit : Unit {
    public CharacterType charaType;
    [SerializeField]
    float attackRange = 15.0f;
    public float TotalAttackRange
    {
        get { return radius + attackRange; }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255,0,0,0.2f);
        Gizmos.DrawSphere(transform.position, radius);
        Gizmos.color = new Color(0,0,255,0.2f);
        Gizmos.DrawSphere(transform.position, TotalAttackRange);
    }
}
