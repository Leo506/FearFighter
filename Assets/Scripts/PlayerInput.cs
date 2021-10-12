using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	[SerializeField] float lineLength;
	LineRenderer line;
	bool isPressing = false;
	Vector2 startPoint, endPoint;

	void Start() {
		line = GetComponent<LineRenderer>();
		line.positionCount = 2;
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			line.SetPosition(0, this.transform.position);
			startPoint = GetMousePos();
			isPressing = true;
			Player.canMove = false;
		}

		if (Input.GetMouseButtonUp(0)) {
			isPressing = false;
			Player.canMove = true;
		}

		if (isPressing) {
			endPoint = GetMousePos();
			line.SetPosition(1, CorrectLineLength());
		}
	}


	Vector3 GetMousePos() {
		Vector2 mousePos = Input.mousePosition;
		Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
		return new Vector2(point.x, point.y);
	}


	Vector3 CorrectLineLength() {
		Vector2 dir = endPoint - startPoint;
		if (dir.magnitude > lineLength) {
			return (Vector2)line.GetPosition(0) + dir.normalized * lineLength;
		}

		return (Vector2)line.GetPosition(0) + dir;
	}


	/// <summary>Возвращает направление движения</summary>
	public Vector3 GetDir() {
		if (line.GetPosition(1) != Vector3.zero)
			return (endPoint - startPoint).normalized;

		return Vector3.zero;
	}

}
