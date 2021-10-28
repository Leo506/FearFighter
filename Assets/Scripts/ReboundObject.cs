using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReboundObject : MonoBehaviour
{
	protected Vector2 direction = Vector2.zero;  	   // Направление движения
	public int availableRebounds = 1;  				   // Доступное количество отскоков


	// Функция отскока
    public void Rebound(Collision2D other) {
    	IChangingDirection changer = other.gameObject.GetComponent<IChangingDirection>();

    	if (changer != null)
    		direction = changer.ChangePlayerDirection(direction, other.contacts[0].normal, ref availableRebounds);

    }
}
