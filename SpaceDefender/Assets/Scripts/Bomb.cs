using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage;
    private Animator animator;
    private bool hasExploded = false; 

    private void Start()
    {
        animator = GetComponent<Animator>();

        
        if (animator != null)
        {
            animator.ResetTrigger("hasExploded");
        }
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.B) && !hasExploded)
        {
            hasExploded = true;
            Explode();
        }
    }

    private void Explode()
    {
        
        if (animator != null)
        {
            animator.SetTrigger("hasExploded");
            Debug.Log("Patlama animasyonu tetiklendi.");

            
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        
        yield return new WaitForSeconds(animationLength);

        Debug.Log("Animasyon tamamlandý, bomba yok ediliyor.");
        Destroy(gameObject);
    }

    
    public void ApplyDamage()
    {
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                
                Boss boss = enemy.GetComponent<Boss>();
                if (boss != null)
                {
                    boss.TakeDamage(damage);
                    continue;
                }

                
                Skeleton skeleton = enemy.GetComponent<Skeleton>();
                if (skeleton != null)
                {
                    skeleton.TakeDamage(damage);
                    continue;
                }

                Ghost ghost = enemy.GetComponent<Ghost>();
                if (ghost != null)
                {
                    ghost.TakeDamage(damage);
                }
            }
        }

        Debug.Log("Bombanýn hasarý uygulandý!");
    }
}