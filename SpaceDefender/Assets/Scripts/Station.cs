using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Station : MonoBehaviour
{
    public int maxHealth = 100; 
    private int currentHealth; 
    public SpriteRenderer healthBarRenderer; 
    private Color targetColor; 
    private float colorLerpSpeed = 5f;
    private Color originalColor; 
    private SpriteRenderer spriteRenderer; 
    [SerializeField] private MenuManager menuManager;


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

    void Update()
    {
        
        if (healthBarRenderer != null)
        {
            healthBarRenderer.color = Color.Lerp(healthBarRenderer.color, targetColor, colorLerpSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage; 
        if (currentHealth < 0)
            currentHealth = 0; 

        UpdateHealthBar();

        StartCoroutine(FlashDamageEffect());

        if (currentHealth <= 0)
        {
            menuManager.OpenDefeatMenu();
        }
    }

    private IEnumerator FlashDamageEffect()
    {
       
        spriteRenderer.color = new Color(1f, 0.5f, 0.5f);

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = originalColor;
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
