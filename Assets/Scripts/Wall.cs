using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	// Данная функция нужна для того, чтобы установить edge radius в 0
    public void FixWall() {
    	foreach (var item in this.gameObject.GetComponents<BoxCollider2D>()) {
    		item.edgeRadius = 0;
    	}
    }
}
