using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEdgeSetter : MonoBehaviour
{
    [SerializeField]
    float width;
    [SerializeField]
    float height;
    public float GetMiniLimitX()
    {
        return transform.position.x - width / 2;
    }
    public float GetMaxLimitX()
    {
        return transform.position.x + width / 2;
    }
    public float GetMiniLimitZ()
    {
        return transform.position.z - height / 2;
    }
    public float GetMaxiLimitZ()
    {
        return transform.position.x + height / 2;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 100, height));
    }
}
