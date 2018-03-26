using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 2.0f;

    private Vector3 direction = Vector3.left;

    void Update()
    {
        MoveObject();
    }

    void MoveObject()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
