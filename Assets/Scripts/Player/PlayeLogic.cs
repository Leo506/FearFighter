using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeLogic : MonoBehaviour
{

	PlayerInput input;
	PlayerMovement move;

    // Start is called before the first frame update
    void Start()
    {
    	input = GetComponent<PlayerInput>();
    	move = GetComponent<PlayerMovement>();

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
}
