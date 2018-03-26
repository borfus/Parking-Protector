using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFollow : MonoBehaviour
{
    public Transform Projectile;
    public Transform FarLeft;
    public Transform FarRight;

    // Update is called once per frame
    void Update()
    {
        //Vector3 newPosition = transform.position;
        //newPosition.x = Projectile.position.x;
        //newPosition.x = Mathf.Clamp(newPosition.x, FarLeft.position.x, FarRight.position.x);
        //transform.position = newPosition;
    }
}
