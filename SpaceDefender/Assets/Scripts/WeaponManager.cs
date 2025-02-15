using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject OkPrefab;
    public GameObject MermiPrefab;  
    public Transform launchPoint;  

    //Hýz
    public float okHiz; 
    public float mermiHiz; 

    //Hasar
    public int okHasar;  
    public int mermiHasar;

    //Bekleme Süreleri
    public float okBeklemeSure;
    public float mermiBeklemeSure;

    public bool isArrow = true;

    [HideInInspector]
    public bool isRifleActive = false;

    private bool isPlayerCanAttack = true;

    // Bomba
    public int bombaHasar;
    public bool isBombActive;
    public List<GameObject> bombObjects;

    //Weapons
    public GameObject rifleUsage;
    public GameObject bowUsage;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bombSound;

    [SerializeField] private AudioClip arrowSound;
    [SerializeField] private AudioClip bulletSound; 
    void Update()
    {
        if (isBombActive)
        {
            if (Input.GetKeyDown(KeyCode.B)) {
                ActivateBomb();
                isBombActive = false;

                if (audioSource != null && bombSound != null)
                {
                    audioSource.PlayOneShot(bombSound);
                }

            }
        }



        if (isRifleActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) // OK seç
            {
                rifleUsage.SetActive(false);
                bowUsage.SetActive(true);

                GameObject arrowSquare = GameObject.FindGameObjectWithTag("ArrowSquare");
                GameObject rifleSquare = GameObject.FindGameObjectWithTag("RifleSquare");

                if (arrowSquare != null && rifleSquare != null)
                {
                    Renderer rendererArrow = arrowSquare.GetComponent<Renderer>();
                    Renderer rendererRifle = rifleSquare.GetComponent<Renderer>();
                    if (rendererArrow != null && rendererRifle != null)
                    {
                        rendererArrow.material.color = Color.yellow;
                        Color customColor = new Color(189f / 255f, 189f / 255f, 189f / 255f, 255f / 255f);
                        rendererRifle.material.color = customColor;
                    }
                }
                isArrow = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))  // Mermi seç
            {
                rifleUsage.SetActive(true);
                bowUsage.SetActive(false);

                GameObject rifleSquare = GameObject.FindGameObjectWithTag("RifleSquare");
                GameObject arrowSquare = GameObject.FindGameObjectWithTag("ArrowSquare");
                if (arrowSquare != null && rifleSquare != null)
                {
                    Renderer rendererRifle = rifleSquare.GetComponent<Renderer>();
                    Renderer rendererArrow = arrowSquare.GetComponent<Renderer>();

                    if (rendererRifle != null && rendererArrow != null)
                    {
                        rendererRifle.material.color = Color.yellow;
                        Color customColor = new Color(189f / 255f, 189f / 255f, 189f / 255f, 255f / 255f);
                        rendererArrow.material.color = customColor;
                    }
                }
                isArrow = false;
            }
        }
        else
        {
            GameObject arrowSquare = GameObject.FindGameObjectWithTag("ArrowSquare");
            GameObject rifleSquare = GameObject.FindGameObjectWithTag("RifleSquare");

            if (arrowSquare != null && rifleSquare != null)
            {
                Renderer rendererArrow = arrowSquare.GetComponent<Renderer>();
                Renderer rendererRifle = rifleSquare.GetComponent<Renderer>();
                if (rendererArrow != null && rendererRifle != null)
                {
                    rendererArrow.material.color = Color.yellow;
                    Color customColor = new Color(189f / 255f, 189f / 255f, 189f / 255f, 255f / 255f);
                    rendererRifle.material.color = customColor;
                }
            }
        }

    }

    public IEnumerator StartUserAttackTimer()
    {

        if (isArrow)
        {
            isPlayerCanAttack = false;
            yield return new WaitForSeconds(okBeklemeSure);
            isPlayerCanAttack = true;
        }
        else
        {
            isPlayerCanAttack = false;
            yield return new WaitForSeconds(mermiBeklemeSure);
            isPlayerCanAttack = true;
        }
       
    }

    public bool GetUserCanAttack()
    {
        return isPlayerCanAttack;
    }

    public void FireProjectile(Vector2 targetPosition)
    {
        if (isArrow)
        {
            GameObject spawnedProjectile = Instantiate(OkPrefab, launchPoint.position, Quaternion.identity);

            ProjectileMovement projectileMovement = spawnedProjectile.GetComponent<ProjectileMovement>();
            if (projectileMovement != null)
            {
                projectileMovement.speed = okHiz;
                projectileMovement.damage = okHasar;
                projectileMovement.targetPosition = targetPosition;
                projectileMovement.isLaunch = true;
            }
            else
            {
                Debug.Log("Ok Özellikleri Deðiþtirilemedi.");
            }

            SpriteRenderer spriteRenderer = spawnedProjectile.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Foreground";
            }

            if (audioSource != null && arrowSound != null)
            {
                audioSource.PlayOneShot(arrowSound);
            }
        }
        else
        {
            GameObject spawnedProjectile = Instantiate(MermiPrefab, launchPoint.position, Quaternion.identity);

            ProjectileMovement projectileMovement = spawnedProjectile.GetComponent<ProjectileMovement>();
            if (projectileMovement != null)
            {
                projectileMovement.speed = mermiHiz;
                projectileMovement.damage = mermiHasar;
                projectileMovement.targetPosition = targetPosition;
                projectileMovement.isLaunch = true;
            }
            else
            {
                Debug.Log("Mermi Özellikleri Deðiþtirilemedi.");
            }
            

            SpriteRenderer spriteRenderer = spawnedProjectile.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Foreground";
            }

            if (audioSource != null && bulletSound != null)
            {
                audioSource.PlayOneShot(bulletSound);
            }
        }
    }
    private void ActivateBomb()
    {
        foreach (var bombObject in bombObjects)
        {
            Bomb bombScript = bombObject.GetComponent<Bomb>();  

            if(bombScript != null)
            {
                bombScript.ApplyDamage();
            }
        }
    }
}
