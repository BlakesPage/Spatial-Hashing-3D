using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.zero;
    public float Speed = 10f;
    public float time = 5.0f;

    private float timer = 0;
    private bool move = false;

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > time)
        {
            move = !move;
            timer = 0;
        }

        if(move == true)
        {
            transform.position += -moveDirection * Speed * Time.deltaTime;
        }
        else
        {
            transform.position += moveDirection * Speed * Time.deltaTime;
        }
    }
}
