﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElasticWall : MonoBehaviour, IChangingDirection
{
    public Vector2 ChangePlayerDirection(Vector2 dir, Vector3 normal, ref int rebounds) {
    	return (Vector2)Vector3.Reflect(dir, normal).normalized;
    }
}
