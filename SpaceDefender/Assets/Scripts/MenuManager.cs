using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu; 
    [SerializeField] private GameObject defeatMenu;
    [SerializeField] private GameObject victoryMenu;



    private bool isGamePaused = false;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private GoldManager goldManager;

    public Text phaseText;
    public Text scoreText;

    //Buy Rifle Items
    [SerializeField] private Button buyRifleButton; 
    [SerializeField] private Text buyRifleButtonText;

    //Buy Wall Items
    [SerializeField] private Button buyWallButton;
    [SerializeField] private Text buyWallButtonText;

    //Buy Bomb Items
    [SerializeField] private Button buyBombButton;
    [SerializeField] private Text buyBombButtonText;

    //Buy Bow Upgrade Items
    [SerializeField] private Button buyBowUpgradeButton;
    [SerializeField] private Text buyBowUpgradeButtonText;

    //Buy Wall(LVL 2) Items
    [SerializeField] private Button buyWallLvl2Button;
    [SerializeField] private Text buyWallLvl2ButtonText;

    //Ses
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip defeatSound;  
    private AudioSource audioSource; 

    void Start()
    {
        OpenMainMenu();
        audioSource = GetComponent<AudioSource>(); 

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                ResumeGame();
            else
                OpenPauseMenu();
        }
        

        if(goldManager !=  null)
        {
            scoreText.text = goldManager.txtScore.text;
        }
    }

    public void buyRifle()
    {
        if(weaponManager != null && goldManager != null)
        {

            if (goldManager.buyWithScore(600))
            {
                weaponManager.isRifleActive = true;
                buyRifleButton.interactable = false;
                buyRifleButtonText.color = Color.green;
            }
         
        }
    }

    public void buyWall()
    {
        if(goldManager != null)
        {
            if (goldManager.buyWithScore(150))
            {
                buyWallButton.interactable = false;
                buyWallButtonText.color = Color.green;

                GameObject lvl1Defence = GameObject.FindGameObjectWithTag("LVL1Defence");

                if (lvl1Defence != null)
                {
                    lvl1Defence.transform.position = new Vector3(-30.5f, -2.8f, 0.3153515f);
                    Debug.Log("LVL1Defence pozisyonu ayarlandý.");
                }
                else
                {
                    Debug.LogWarning("LVL1Defence tag'ine sahip bir GameObject bulunamadý.");
                }
            }
        }
    }

    public void buyWallLvl2()
    {
        if (goldManager != null)
        {
            if (goldManager.buyWithScore(400))
            {
                buyWallLvl2Button.interactable = false;
                buyWallLvl2ButtonText.color = Color.green;

                GameObject lvl2Defence = GameObject.FindGameObjectWithTag("LVL2Defence");

                if (lvl2Defence != null)
                {
                    lvl2Defence.transform.position = new Vector3(-20f, 0f, 0f);
                    Debug.Log("LVL2Defence pozisyonu ayarlandý.");
                }
                else
                {
                    Debug.LogWarning("LVL2Defence tag'ine sahip bir GameObject bulunamadý.");
                }
            }
        }
    }

    public void buyBomb()
    {
        if (weaponManager !=  null && goldManager != null)
        {
            if (goldManager.buyWithScore(125))
            {
                buyBombButton.interactable = false;
                buyBombButtonText.color = Color.green;

                GameObject bombs = GameObject.FindGameObjectWithTag("BombsPlace");

                if (bombs != null)
                {
                    weaponManager.isBombActive = true;
                    bombs.transform.position = new Vector3(1f, 7f, 0f);
                    Debug.Log("Bombalarýn pozisyonu ayarlandý.");
                }
                else
                {
                    Debug.LogWarning("BombsPlace tag'ine sahip bir GameObject bulunamadý.");
                }
            }
        }
    }

    public void buyUpgradeBow()
    {
        if(weaponManager != null && goldManager != null)
        {
            if (goldManager.buyWithScore(200))
            {
                buyBowUpgradeButton.interactable = false;
                buyBowUpgradeButtonText.color = Color.green;
                weaponManager.okHiz += 25;
                weaponManager.okHasar += 15;
            }
        }
    }

    public void StartGame()
    {
        gameManager.isGameStarted = true;

        Time.timeScale = 1f;

        mainMenu.SetActive(false);

    }

    public void QuitGame()
    {
        Debug.Log("Oyun kapatýlýyor...");
        Application.Quit();
    }


    public void OpenShopMenu(float duration)
    {
        CloseAllMenus(); 
        shopMenu.transform.position = new Vector3(0, 0, 0); 
        shopMenu.SetActive(true);  

        Time.timeScale = 0f;  

        StartCoroutine(CloseShopMenuAfterDelay(duration));
    }

    private IEnumerator CloseShopMenuAfterDelay(float duration)
    {
        
        StartCoroutine(DisplayBreakTime(duration));

        
        yield return new WaitForSecondsRealtime(duration);

        
        CloseAllMenus();
    }

    private IEnumerator DisplayBreakTime(float duration)
    {
        float remainingBreakTime = duration;

        while (remainingBreakTime > 0)
        {
            int minutes = Mathf.FloorToInt(remainingBreakTime / 60);
            int seconds = Mathf.FloorToInt(remainingBreakTime % 60);
            phaseText.text = $"{minutes:D2}:{seconds:D2}"; 

            
            remainingBreakTime -= Time.unscaledDeltaTime; 

            yield return null;  
        }

    }


    public void OpenPauseMenu()
    {
        CloseAllMenus();
        pauseMenu.SetActive(true);
        pauseMenu.transform.position = new Vector3(0, 0, 0); 
        Time.timeScale = 0f; 
        isGamePaused = true;
    }

    public void OpenMainMenu()
    {
        CloseAllMenus();
        mainMenu.SetActive(true);
        mainMenu.transform.position = new Vector3(0, 0, 0);  
        Time.timeScale = 0f; 
    }

    public void OpenDefeatMenu()
    {
        CloseAllMenus();
        defeatMenu.SetActive(true);
        defeatMenu.transform.position = new Vector3(0, 0, 0);
        Time.timeScale = 0f;

        PlaySound(defeatSound);
    }
    public void OpenVictoryMenu()
    {
        CloseAllMenus();
        victoryMenu.SetActive(true);
        victoryMenu.transform.position = new Vector3(0, 0, 0);
        Time.timeScale = 0f;

        PlaySound(victorySound);
    }

    public void CloseAllMenus()
    {
        shopMenu?.SetActive(false);
        pauseMenu?.SetActive(false);
        mainMenu?.SetActive(false);
        Time.timeScale = 1f; 
        isGamePaused = false;
    }

    public void ResumeGame()
    {
        CloseAllMenus();
    }

    public void RestartGame()
    {
        
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
