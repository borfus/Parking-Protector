using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMovement : MonoBehaviour
{
    public float speed = 2.0f;
    public float delta = 1.5f; // Amount to move up and down
    private Vector3 startPos;
    private float startTime;

    private Vector3 direction = Vector3.left;

    void Start()
    {
        startTime = Time.time;
        startPos = transform.position;
    }

    void Update()
    {
        MoveObject();
    }

    void MoveObject()
    {
        Vector3 v = startPos;
        v.y += delta * Mathf.Sin(Time.time * speed);
        v.x += ((startTime - Time.time) * speed);
        transform.position = v;
    }
}