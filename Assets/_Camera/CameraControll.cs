using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour {
    public float speed=105.0f;
    private void Start()
    {
        StartCoroutine(CameraControllRoutine());
    }
    IEnumerator CameraControllRoutine()
    {
        
        while (true)
        {
            Vector3 move = Vector3.zero;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                move += transform.right;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                move += -transform.right;
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                move += transform.forward;
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                move += -transform.forward;
            }
            move.y = 0;
            transform.position += move.normalized * Time.unscaledDeltaTime * speed;
            yield return null;
        }
    }
}
