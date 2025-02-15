using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public float speed; 
    public int damage; 
    public int maxHealth; 
    private int currentHealth; 
    private Transform target; 
    public bool isClone = false;
    [SerializeField]
    private GoldManager goldManager;
    public SpriteRenderer healthBarRenderer; 
    private Color targetColor; 
    private float colorLerpSpeed = 5f; 
    private Color originalColor;
    private SpriteRenderer spriteRenderer; 

    //Animasyon
    private Animator animator; 
    private bool isDying = false; 

    //Ses
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip spawnSound;


    void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        animator = GetComponent<Animator>();

        if (healthBarRenderer != null)
        {
            healthBarRenderer.color = Color.green; 
            targetColor = Color.green;
        }

        if (isClone)
        {
            if (audioSource != null && spawnSound != null)
            {
                audioSource.PlayOneShot(spawnSound);
            }

            target = GameObject.FindGameObjectWithTag("Station")?.transform; 
        }
        if (goldManager == null)
        {
            goldManager = FindObjectOfType<GoldManager>();
            if (goldManager == null)
            {
                Debug.LogError("GoldManager bulunamadý! GoldManager bileþenini sahneye eklediðinizden emin olun.");
            }
        }

        
    }

    void Update()
    {
        if (isClone && target != null && !isDying)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (healthBarRenderer != null)
            {
                healthBarRenderer.color = Color.Lerp(healthBarRenderer.color, targetColor, colorLerpSpeed * Time.deltaTime);
            }
        }

    }

    public void TakeDamage(int damageAmount)
    {
        if (!isClone || isDying) return; 

        Debug.Log("Damage: " + damageAmount + " | Max Health : " + maxHealth + " | Current Health : " + currentHealth);

        currentHealth -= damageAmount;

        UpdateHealthBar();

        StartCoroutine(FlashDamageEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashDamageEffect()
    {
        
        spriteRenderer.color = new Color(1f, 0.5f, 0.5f);

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = originalColor;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isClone || isDying) return;

        if (collision.gameObject.CompareTag("Station"))
        {
            Station station = collision.gameObject.GetComponent<Station>();
            if (station != null)
            {
                station.TakeDamage(damage);
            }

        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

        }

        if (collision.gameObject.CompareTag("DefenceWall"))
        {
            DefenceWall defenceWall = collision.gameObject.GetComponent<DefenceWall>();
            if (defenceWall != null)
            {
                defenceWall.TakeDamage(damage);
            }

        }
    }

    private float attackCooldown = 0.5f; 
    private float lastAttackTime = 0f; 

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!isClone || isDying) return;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (collision.gameObject.CompareTag("Station"))
            {
                Station station = collision.gameObject.GetComponent<Station>();
                if (station != null)
                {
                    station.TakeDamage(damage);
                }
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }

            if (collision.gameObject.CompareTag("DefenceWall"))
            {
                DefenceWall defenceWall = collision.gameObject.GetComponent<DefenceWall>();
                if (defenceWall != null)
                {
                    defenceWall.TakeDamage(damage);
                }
            }

            lastAttackTime = Time.time;
        }
    }


    void Die()
    {
        if (!isClone || isDying) return; 

        isDying = true;
        Debug.Log("Skeleton öldü!");

        animator.SetTrigger("Die");
        animator.speed = 10f; 

        if (goldManager != null)
        {
            goldManager.UpdateScore(12);
        }
        else
        {
            Debug.LogWarning("GoldManager atanmamýþ! Puan güncellenemedi.");
        }
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        animator.speed = 8f; 
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length / animator.speed);

        Destroy(gameObject);
    }

    void UpdateHealthBar()
    {
        if (healthBarRenderer != null)
        {
            
            float healthPercent = (float)currentHealth / maxHealth;

            targetColor = Color.Lerp(Color.red, Color.green, healthPercent);
        }
    }
}
