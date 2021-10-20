using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyWall : Wall
{
    public override Vector2 ChangePlayerDirection(Vector2 dir, Vector3 normal, ref int rebounds) {
    	rebounds = 1;
    	return Vector2.zero;
    }
}
