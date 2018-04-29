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
        Vector3 _move=new Vector3(0,0,0);
        while (true)
        {
            _move = Vector3.zero;
            if (Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.D))
            {
                _move = transform.right;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                _move = -transform.right;
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                _move = transform.forward;
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                _move = -transform.forward;
            }
            _move.y = 0;
            transform.position+=_move.normalized*Time.unscaledDeltaTime*speed;
            yield return null;
        }
    }
}
