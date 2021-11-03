using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] Image _damagePanel;
    [SerializeField] Canvas _endGameCanvas;
    
    public void ShowPlayerHP(float maxHP, float currentHP)
    {
        float tmp = 0.25f / maxHP;
        _damagePanel.color = new Color(1, 0, 0, (maxHP - currentHP) * tmp);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        _endGameCanvas.enabled = true;
    }

    public void TryAgain(bool yes)
    {
        if (yes)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainScene");
        }
        else
            Application.Quit();
    }
}
