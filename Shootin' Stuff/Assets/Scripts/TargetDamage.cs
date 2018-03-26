using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDamage : MonoBehaviour
{
    public int Hitpoints = 2;
    public Sprite DamagedSprite;
    public float DamageImpactSpeed;

    private int currentHitPoints;
    private float damageImpactSpeedSqr;
    private SpriteRenderer spriteRenderer;
    
    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHitPoints = Hitpoints;
        damageImpactSpeedSqr = DamageImpactSpeed * DamageImpactSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag != "Damager")
        {
            return;
        }
        if(collision.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr)
        {
            return;
        }

        spriteRenderer.sprite = DamagedSprite;
        currentHitPoints--;

        if (currentHitPoints <= 0)
        {
            Kill();
        }
    }

    void Kill()
    {
        Destroy(gameObject);
        spriteRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
