using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] scriptsToDisable;
    [SerializeField] private AudioSource backgroundAudioSource; // Arka plan sesi için AudioSource
    [SerializeField] private AudioClip backgroundAudioClip; // Arka plan ses dosyasý

    public bool isGameStarted = false;

    void Start()
    {
        // Eðer oyun baþlamadýysa, scriptleri devre dýþý býrak ve sesi çalma
        if (!isGameStarted)
        {
            foreach (MonoBehaviour script in scriptsToDisable)
            {
                script.enabled = false;
            }

            // Arka plan sesini ayarla ama oynatma
            if (backgroundAudioSource != null && backgroundAudioClip != null)
            {
                backgroundAudioSource.clip = backgroundAudioClip;
                backgroundAudioSource.loop = true; // Sesin sürekli tekrar etmesini istiyorsanýz
            }
        }
    }

    void Update()
    {
        if (isGameStarted)
        {
            // Scriptleri etkinleþtir
            foreach (MonoBehaviour script in scriptsToDisable)
            {
                script.enabled = true;
            }

            // Eðer oyun baþladýysa, sesi çalmaya baþla
            if (backgroundAudioSource != null && !backgroundAudioSource.isPlaying)
            {
                backgroundAudioSource.Play();
            }
        }
        else
        {
            // Oyun durursa arka plan sesini durdur
            if (backgroundAudioSource != null && backgroundAudioSource.isPlaying)
            {
                backgroundAudioSource.Pause();
            }
        }
    }
}
