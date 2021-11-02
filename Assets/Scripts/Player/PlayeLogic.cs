using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayeLogic : MonoBehaviour
{
    public float maxHP= 100;

    float playerHP;
    public float PlayerHP
    {
        get
        {
            return playerHP;
        }

        set
        {
            playerHP += value;
            if (playerHP > maxHP)
                playerHP = maxHP;
            uiController.ShowPlayerHP(maxHP, playerHP);
        }
    }
    UIController uiController;
	PlayerInput input;
	PlayerMovement move;

    // Start is called before the first frame update
    void Start()
    {
    	input = GetComponent<PlayerInput>();
    	move = GetComponent<PlayerMovement>();
        uiController = FindObjectOfType<UIController>();

        playerHP = maxHP;

    	input.EndInputEvent += EndInput;
    	move.EndMove += EndMove;
    }

    void EndInput()
    {
        input.CanInput = false;
        move.SetDir(input.GetDir());
    }

    void EndMove()
    {
        input.CanInput = true;
    }

    public void EndRound()
    {
        move.roundEnd = true;
        move.EndMove -= EndMove;
        
        move.SetDir(Vector2.zero);
        Debug.Log("Уровень зачищен. Ввод невозможен. roundEnd = " + move.roundEnd);
    }

    public void GetDamage(float value)
    {
        playerHP -= value;
        if (playerHP <= 0)
            Destroy(this.gameObject);

        float tmp = 0.25f / 100f;

        uiController.ShowPlayerHP(100, playerHP);
    }
}
