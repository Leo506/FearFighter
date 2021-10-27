using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInput : MonoBehaviour
{
	bool canInput = true;
	public bool CanInput {
		get {
			return canInput;
		}

		set {
			canInput = value;
		}
	}

    Vector2 startPoint = Vector2.zero;
    Vector2 endPoint = Vector2.zero;
    Camera cam;
    bool isPressing = false;

    PlayerLine line;
    PlayeLogic logic;
    

    IChangingTime[] toChangeTime;

    public delegate void InputDelegate();
    public event InputDelegate EndInputEvent;

    void Start() {
    	cam = Camera.main;
    	line = GetComponent<PlayerLine>();
    	logic = GetComponent<PlayeLogic>();
    }


    public Vector2 GetDir() {
    	return (endPoint - startPoint).normalized;
    }


    public void StartInput() {
    	startPoint = GetInputPos();
    	isPressing = true;

        toChangeTime = FindObjectsOfType<MonoBehaviour>().OfType<IChangingTime>().ToArray();

        foreach (var item in toChangeTime)
            item.SlowDownTime();
    }


    public void EndInput() {
    	isPressing = false;

        endPoint = GetInputPos();

        EndInputEvent();

    	line.DrawLine(this.gameObject.transform.position, Vector3.zero, Vector3.zero);
        foreach (var item in toChangeTime)
            item.AccelerateTime();

        startPoint = Vector2.zero;
    	endPoint = Vector2.zero;
    }



    Vector2 GetInputPos()
    {
        Vector2 toReturnVector;
#if UNITY_EDITOR
        var mousePos = Input.mousePosition;
        toReturnVector = (Vector2)cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
#else

        toReturnVector = Input.GetTouch(0).position;
#endif

        return toReturnVector;
    }


    void Update() {

    	if (canInput) {
#if UNITY_EDITOR
	    		if (Input.GetMouseButtonDown(0)) {
	    			StartInput();
	    		}

	    		if (Input.GetMouseButtonUp(0)) {
	    			EndInput();
	    		}
#else

            if (Input.touchCount > 0)
                {
                    var touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                        StartInput();

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        EndInput();
                }
#endif

                if (isPressing)
                {
                    endPoint = GetInputPos();
                    line.DrawLine(this.gameObject.transform.position, startPoint, endPoint);
                }

        }
    }

}
