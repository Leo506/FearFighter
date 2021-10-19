using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Vector2 startPoint = Vector2.zero;
    Vector2 endPoint = Vector2.zero;
    Camera cam;
    bool isPressing = false;

    PlayerLine line;
    PlayerMovement movement;

    void Start() {
    	cam = Camera.main;
    	line = GetComponent<PlayerLine>();
    	movement = GetComponent<PlayerMovement>();
    }


    public Vector2 GetDir() {
    	#if UNITY_EDITOR
    		return (endPoint - startPoint).normalized;
    	#endif
    }


    public void StartInput() {
    	startPoint = Vector2.zero;
    	endPoint = Vector2.zero;
    	isPressing = true;

    	#if UNITY_EDITOR
    		startPoint = GetMousePos();
    	#endif
    }


    public void EndInput() {
    	isPressing = false;
    	#if UNITY_EDITOR
    		endPoint = GetMousePos();
    	#endif

    	movement.SetDir(GetDir());
    	movement.inMove = true;
    	line.DrawLine(this.gameObject.transform.position, Vector3.zero, Vector3.zero);
    }


    Vector2 GetMousePos() {
    	var mousePos = Input.mousePosition;
		return (Vector2)cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
    }


    void Update() {

    	// Если персонаж не в движении
    	if (!movement.inMove) {
	    	#if UNITY_EDITOR
	    		if (Input.GetMouseButtonDown(0)) {
	    			StartInput();
	    		}

	    		if (Input.GetMouseButtonUp(0)) {
	    			EndInput();
	    		}

	    		if (isPressing) {
	    			endPoint = GetMousePos();
	    			line.DrawLine(this.gameObject.transform.position, startPoint, endPoint);
	    		}
	    	#endif
    	}
    }
}
