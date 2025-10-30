using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Trap
{
    [SerializeField] private float speed;
    [SerializeField] private Transform[] movePoint;

    private int health = 100;
    private int i = 0;
    private bool facingLeft = true; // Sprite starts facing left

    protected override void Start()
    {
        base.Start();
        // Start at the first move point
        transform.position = movePoint[0].position;
    }

    private void Update()
    {
        if (movePoint.Length == 0) return;

        Vector3 targetPos = movePoint[i].position;

        // Move towards the current target
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Flip when changing direction
        if (targetPos.x > transform.position.x && facingLeft)
        {
            Flip();
        }
        else if (targetPos.x < transform.position.x && !facingLeft)
        {
            Flip();
        }

        // Check if we reached the target
        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            i = (i + 1) % movePoint.Length; // Loop back to start
        }
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Flip horizontally
        transform.localScale = scale;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}