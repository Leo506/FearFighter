using UnityEngine;

public interface IChangingDirection {
	Vector2 ChangePlayerDirection(Vector2 dir, Vector3 normal, ref int rebounds);
}