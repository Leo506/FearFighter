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

    	input.EndInputEvent += () => { input.CanInput = false; move.SetDir(input.GetDir()); };
    	move.EndMove += () => input.CanInput = true;
    }

    public void EndRound()
    {
        input.CanInput = false;
        move.roundEnd = true;
        move.SetDir(Vector2.zero);
    }
}
