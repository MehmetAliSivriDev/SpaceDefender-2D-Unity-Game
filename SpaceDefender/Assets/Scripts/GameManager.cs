using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] scriptsToDisable;
    [SerializeField] private AudioSource backgroundAudioSource; // Arka plan sesi i�in AudioSource
    [SerializeField] private AudioClip backgroundAudioClip; // Arka plan ses dosyas�

    public bool isGameStarted = false;

    void Start()
    {
        // E�er oyun ba�lamad�ysa, scriptleri devre d��� b�rak ve sesi �alma
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
                backgroundAudioSource.loop = true; // Sesin s�rekli tekrar etmesini istiyorsan�z
            }
        }
    }

    void Update()
    {
        if (isGameStarted)
        {
            // Scriptleri etkinle�tir
            foreach (MonoBehaviour script in scriptsToDisable)
            {
                script.enabled = true;
            }

            // E�er oyun ba�lad�ysa, sesi �almaya ba�la
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
