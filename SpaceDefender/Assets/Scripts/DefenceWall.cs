using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceWall : MonoBehaviour
{
    public int maxHealth; 
    private int currentHealth; 
    public SpriteRenderer healthBarRenderer; 
    private Color targetColor; 
    private float colorLerpSpeed = 5f;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

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
    }

    private void Update()
    {
        
        if (healthBarRenderer != null)
        {
            healthBarRenderer.color = Color.Lerp(healthBarRenderer.color, targetColor, colorLerpSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        TakeDamage(10); 
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log($"{gameObject.name} hasarý aldý! Kalan can: {currentHealth}");

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

    void Die()
    {
        Debug.Log($"{gameObject.name} yýkýldý!");
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
