using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public Vector2 targetPosition;
    private Vector2 direction;
    private Rigidbody2D rb;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public float lifeTime = 5f;
    [HideInInspector]
    public bool isLaunch = false;

    private void Start()
    {
        if (isLaunch)
        {
            direction = (targetPosition - (Vector2)transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            rb = GetComponent<Rigidbody2D>();
        }

        Destroy(gameObject, lifeTime);
        isLaunch = false;
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            Debug.Log("Projectile hit: " + collision.gameObject.name);

            Boss boss = collision.gameObject.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            else
            {

                Skeleton skeleton = collision.gameObject.GetComponent<Skeleton>();
                if (skeleton != null)
                {
                    skeleton.TakeDamage(damage);
                }

                Ghost ghost = collision.gameObject.GetComponent<Ghost>();
                if (ghost != null)
                {
                    ghost.TakeDamage(damage);
                }
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Station"))
        {
            Station station = collision.gameObject.GetComponent<Station>();
            if (station != null)
            {
                station.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("BossBlock"))
        {
            Destroy(gameObject);
        }
    }
}
