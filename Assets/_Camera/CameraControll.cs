using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour {
    private void Start()
    {
        StartCoroutine(CameraControllUpdate());
    }
    IEnumerator CameraControllUpdate()
    {
        float  speed = 105.0f;
        Vector3 _move=new Vector3(0,0,0);
        while (true)
        {
            _move = Vector3.zero;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _move = transform.right;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _move = -transform.right;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _move = transform.forward;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _move = -transform.forward;
            }
            _move.y = 0;
            transform.position+=_move.normalized*Time.unscaledDeltaTime*speed;
            yield return null;
        }
    }
}
