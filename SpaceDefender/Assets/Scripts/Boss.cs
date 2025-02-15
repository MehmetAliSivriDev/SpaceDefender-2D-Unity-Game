using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float speed;
    public int damage; 
    public int maxHealth; 
    private int currentHealth;
    private Transform target; 
    public bool isSpawn = false;
    public int spawnStage = 3; 
    public Font customFont; 
    public SpriteRenderer healthBarRenderer;
    private Color targetColor; 
    private float colorLerpSpeed = 5f; 
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private MenuManager menuManager;
    private float attackCooldown = 0.5f;
    private float lastAttackTime = 0f;
    private bool isAttacking = false;


    void Start()
    {
        
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        
        if (healthBarRenderer != null)
        {
            healthBarRenderer.color = Color.green;
            targetColor = Color.green; 
        }

        
        target = GameObject.FindGameObjectWithTag("Station")?.transform;

        
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        
        if (isSpawn && target != null)
        {
            
            
            Vector2 targetPosition = target.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            
            if (healthBarRenderer != null)
            {
                healthBarRenderer.color = Color.Lerp(healthBarRenderer.color, targetColor, colorLerpSpeed * Time.deltaTime);
            }      

        }
    }

    public void TakeDamage(int damageAmount)
    {
        
        currentHealth -= damageAmount;

        UpdateHealthBar();

        StartCoroutine(FlashDamageEffect());

        
        if (currentHealth <= 0)
        {
            menuManager.OpenVictoryMenu();
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
        
        if (collision.gameObject.CompareTag("Station"))
        {
            if (!isAttacking) 
            {
                TriggerAttackAnimation();
                Station station = collision.gameObject.GetComponent<Station>();
                if (station != null)
                {
                    station.TakeDamage(damage);
                }
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isAttacking) 
            {
                TriggerAttackAnimation();
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (collision.gameObject.CompareTag("Station"))
            {
                if (!isAttacking) 
                {
                    TriggerAttackAnimation();
                    Station station = collision.gameObject.GetComponent<Station>();
                    if (station != null)
                    {
                        station.TakeDamage(damage);
                    }
                }
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                if (!isAttacking) 
                {
                    TriggerAttackAnimation();
                    Player player = collision.gameObject.GetComponent<Player>();
                    if (player != null)
                    {
                        player.TakeDamage(damage);
                    }
                }
            }

            if (collision.gameObject.CompareTag("DefenceWall"))
            {
                if (!isAttacking) 
                {
                    TriggerAttackAnimation();
                    DefenceWall defenceWall = collision.gameObject.GetComponent<DefenceWall>();
                    if (defenceWall != null)
                    {
                        defenceWall.TakeDamage(damage);
                    }
                }
            }

            
            lastAttackTime = Time.time;
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarRenderer != null)
        {
            
            float healthPercent = (float)currentHealth / maxHealth;

            
            targetColor = Color.Lerp(Color.red, Color.green, healthPercent);
        }
    }

    private void TriggerAttackAnimation()
    {
        if (animator != null && !isAttacking)
        {
            isAttacking = true; 
            animator.SetBool("isAttack", true);

            StartCoroutine(ResetAttackTrigger());
        }
    }

    private IEnumerator ResetAttackTrigger()
    {
        
        
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        if (animator != null)
        {
            animator.SetBool("isAttack", false); 
        }

        isAttacking = false; 
    }
}
