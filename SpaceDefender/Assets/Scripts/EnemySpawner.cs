using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float initialSpawnRate = 1f;
    [SerializeField] private GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public Text phaseText; 
    private bool canSpawn = true;
    private bool isPaused = false;

    private int currentPhase = 1;
    private float currentSpawnRate;
    private float elapsedTime = 0f;

    //Normal Süreler
    private float[] phaseDurations = { 150f, 180f, 210f };
    private float pauseDuration = 30f; 

    //Hýzlandýrýlmýþ Süreler
    //private float[] phaseDurations = { 10f, 13f, 16f }; 
    //private float pauseDuration = 5f;

    [SerializeField]
    private MenuManager menuManager;

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        StartCoroutine(PhaseManager());
       
    }


    private void SpawnBoss()
    {
        GameObject bossPrefab = Resources.Load<GameObject>("Boss");
        Transform bossSpawnPoint = spawnPoints[0];

        if (bossPrefab != null && bossSpawnPoint != null)
        {
            GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            Boss bossScript = boss.GetComponent<Boss>();
            if (bossScript != null)
            {
                bossScript.isSpawn = true;
            }
        }
        else
        {
            Debug.LogError("Boss prefab'i veya spawn noktasý eksik!");
        }
    }
    private IEnumerator Spawner()
    {

        while (canSpawn)
        {
            yield return new WaitForSeconds(currentSpawnRate);

            if (!isPaused)
            {
                int rand = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyToSpawn = enemyPrefabs[rand];

                if (enemyToSpawn != null)
                {
                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    GameObject enemy = Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
                    enemy.tag = "Enemy";

                    
                    SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
                    Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                    if (rb != null) rb.gravityScale = 0;
                    if (spriteRenderer != null) spriteRenderer.sortingLayerName = "Foreground";

                    if (enemyToSpawn.gameObject.name.Contains("Skeleton"))
                    {
                        Skeleton enemyScript = enemy.GetComponent<Skeleton>();
                        if (enemyScript != null)
                        {
                            enemyScript.isClone = true;
                            enemyScript.damage += currentPhase * 2;
                            enemyScript.speed += currentPhase * 1.5f;   
                        }
                    }
                    else if (enemyToSpawn.gameObject.name.Contains("Ghost"))
                    {
                        Ghost enemyScript = enemy.GetComponent<Ghost>();
                        if (enemyScript != null)
                        {
                            enemyScript.isClone = true;
                            enemyScript.damage += currentPhase * 2;
                            enemyScript.speed += currentPhase * 1.5f;  
                        }
                    }
                }
                else
                {
                    Debug.Log("Enemy Null Error!");
                }
            }
        }
        
    }

    private IEnumerator PhaseManager()
    {
        while (currentPhase <= phaseDurations.Length)
        {
            float phaseDuration = phaseDurations[currentPhase - 1];
            canSpawn = true;
            isPaused = false;

            StartCoroutine(Spawner());

            
            float remainingTime = phaseDuration;

            while (remainingTime > 0)
            {
                UpdatePhaseText(remainingTime);
                remainingTime -= Time.deltaTime;
                yield return null;
            }

            isPaused = true;
            canSpawn = false;
            StopCoroutine(Spawner());

            
            ClearEnemies();

            Debug.Log($"Aþama {currentPhase} sona erdi. {pauseDuration} saniye bekleniyor...");
            
            menuManager.OpenShopMenu(pauseDuration);

            

            currentPhase++;
            if (currentPhase > phaseDurations.Length)
            {
                Debug.Log("Boss Geliyor !!!");
                phaseText.text = "3. aþama tamamlandý!";
                phaseText.text = "Boss Geliyor !!!!";
                
                GameObject boss = GameObject.FindWithTag("Boss");
                if (boss != null)
                {
                    Boss bossScript = boss.GetComponent<Boss>();
                    if (bossScript != null)
                    {
                        bossScript.isSpawn = true; 
                    }
                }

                GameObject bossBlock = GameObject.FindGameObjectWithTag("BossBlock");

                if (bossBlock != null)
                {
                    Destroy(bossBlock);
                }

                yield break;
            }

            currentSpawnRate *= 0.8f;
            Debug.Log($"Aþama {currentPhase}: Spawnlanma hýzý arttý ve düþmanlar güçlendi!");
        }
    }

    private void ClearEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Skeleton skeletonScript = enemy.GetComponent<Skeleton>();
            Ghost ghostScript = enemy.GetComponent<Ghost>();

            if ((skeletonScript != null && skeletonScript.isClone) ||
                (ghostScript != null && ghostScript.isClone))
            {
                Destroy(enemy);
            }
        }
    }

    private void UpdatePhaseText(float remainingTime)
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        phaseText.text = $"{minutes:D2}:{seconds:D2}";
    }
}
