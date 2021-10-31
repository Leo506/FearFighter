using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IChangingDirection
{

    public Vector2 ChangePlayerDirection(Vector2 dir, Vector3 normal, ref int rebounds) {
    	if (rebounds > 0) {
    		rebounds--;
    		return (Vector2)Vector3.Reflect(dir, normal).normalized;
    	} else {
    		rebounds = 1;
    		return Vector2.zero;
    	}
    }
}
