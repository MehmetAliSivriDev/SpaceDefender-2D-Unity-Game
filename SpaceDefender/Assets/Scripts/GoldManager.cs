using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    [SerializeField]
    public Text txtScore;  
    private int score = 0; 
    
    public bool buyWithScore(int amount)
    {
        if(score >= amount)
        {
            score -= amount;
            txtScore.text = score.ToString();
            return true;
        }
        return false;
    }
    
    
    public void UpdateScore(int amount)
    {
        score += amount; 
        txtScore.text = score.ToString(); 

        Debug.Log("Skor Güncellendi: " + score);
    }

    private void Start()
    {
       
            UpdateScore(0);
       
    }
}
