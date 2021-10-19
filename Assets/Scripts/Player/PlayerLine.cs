using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLine : MonoBehaviour
{

	[SerializeField] LineRenderer line;
	public float lineLength;

    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 2;
    }

    
    public void DrawLine(Vector3 pos1, Vector3 pos2, Vector3 pos3) {
    	line.SetPosition(0, pos1);
    	line.SetPosition(1, CorrectEndLinePoint(pos2, pos3));
    }


    Vector3 CorrectEndLinePoint(Vector3 pos1, Vector3 pos2) {
    	Vector2 dir = pos2 - pos1;
		if (dir.magnitude > lineLength) {
			return (Vector2)line.GetPosition(0) + dir.normalized * lineLength;
		}

		return (Vector2)line.GetPosition(0) + dir;
    }
}
