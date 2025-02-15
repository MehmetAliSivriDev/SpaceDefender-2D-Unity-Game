using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public Transform MermiYer;
    public SpriteRenderer healthBarRenderer;
    public int maxHealth;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color targetColor; 
    private float colorLerpSpeed = 2f; 
    [SerializeField] private WeaponManager weaponManager;
    private Color originalColor; 
    [SerializeField] private MenuManager menuManager;
    private Animator animator;
    public GameObject rifleUsage;
    public GameObject bowUsage;

    void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBarRenderer = healthBarRenderer.GetComponent<SpriteRenderer>();

        healthBarRenderer.color = Color.green;
        targetColor = Color.green;

        originalColor = spriteRenderer.color;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        float hMovement = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        float vMovement = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(hMovement));
        }
        else if (Input.GetAxisRaw("Vertical") != 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(vMovement));
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        Vector3 move = new Vector3(hMovement, vMovement, 0);
        transform.Translate(move);

        
        if (hMovement < 0) 
        {
            spriteRenderer.flipX = false; 
            FlipChildren(false); 
        }
        else if (hMovement > 0) 
        {
            spriteRenderer.flipX = true; 
            FlipChildren(true);
        }

        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (weaponManager.GetUserCanAttack())
            {
                weaponManager.FireProjectile(vec);
                StartCoroutine(weaponManager.StartUserAttackTimer());
            }
        }

        
        healthBarRenderer.color = Color.Lerp(healthBarRenderer.color, targetColor, colorLerpSpeed * Time.deltaTime);
    }

    
    private void FlipChildren(bool flip)
    {
        Transform mermiYerTransform = transform.Find("MermiYer");

        if (mermiYerTransform != null)
        {
            Vector3 position = mermiYerTransform.localPosition;

            position.x = flip ? 1.56f : -1.56f;

            mermiYerTransform.localPosition = position;

        }

        if (weaponManager != null)
        {
            if (weaponManager.isArrow)
            {

                rifleUsage.SetActive(false);
                bowUsage.SetActive(true);

                Transform handTransform = transform.Find("BowUsage").Find("Hand"); 
                Transform weaponTransform = transform.Find("BowUsage").Find("Weapon"); 

                if (handTransform != null)
                {
                    Vector3 scale = handTransform.localScale;
                    Vector3 position = handTransform.localPosition;

                    position.x = 0;
                    scale.x = flip ? -0.8201074f : 0.8201074f;

                    handTransform.localPosition = position;
                    handTransform.localScale = scale;
                }

                if (weaponTransform != null)
                {
                    Vector3 scale = weaponTransform.localScale;
                    Vector3 position = weaponTransform.localPosition;

                    position.x = flip ? 0.5f : -0.5f;
                    scale.x = flip ? -0.8201074f : 0.8201074f;

                    weaponTransform.localPosition = position;
                    weaponTransform.localScale = scale;
                }
            }
            else
            {
                rifleUsage.SetActive(true);
                bowUsage.SetActive(false);

                Transform handTransform = transform.Find("RifleUsage").Find("Hand"); // "Hand" nesnesini bul
                Transform weaponTransform = transform.Find("RifleUsage").Find("Weapon"); // "Weapon" nesnesini bul
                Transform rifleFireTransform = transform.Find("RifleUsage").Find("RifleFire"); // "Weapon" nesnesini bul

                if (handTransform != null)
                {
                    Vector3 scale = handTransform.localScale;
                    Vector3 position = handTransform.localPosition;

                    position.x = 0;
                    scale.x = flip ? -0.8201074f : 0.8201074f;

                    handTransform.localPosition = position;
                    handTransform.localScale = scale;
                }

                if (weaponTransform != null)
                {
                    Vector3 scale = weaponTransform.localScale;
                    Vector3 position = weaponTransform.localPosition;

                    position.x = flip ? 0.5f : -0.5f;
                    scale.x = flip ? -0.8201074f : 0.8201074f;

                    weaponTransform.localPosition = position;
                    weaponTransform.localScale = scale;
                }

                if (rifleFireTransform != null)
                {
                    Vector3 scale = rifleFireTransform.localScale;
                    Vector3 position = rifleFireTransform.localPosition;

                    position.x = flip ? 1.53118f : -1.5311f;
                    scale.x = flip ? -0.8201074f : 0.8201074f;

                    rifleFireTransform.localPosition = position;
                    rifleFireTransform.localScale = scale;
                }
            }
        }
    }


    public void TakeDamage(int damage)
    {
        Debug.Log("Damage: " + damage + " | Max Health : " + maxHealth + " | Current Health : " + currentHealth);

        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0; 

        float healthPercent = (float)currentHealth / maxHealth;
        targetColor = Color.Lerp(Color.red, Color.green, healthPercent); 

        StartCoroutine(FlashDamageEffect());

        if (currentHealth == 0)
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



    void Die()
    {
        Debug.Log("Karakter öldü!");
        
    }
}