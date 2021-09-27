using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] GameObject _floorPrefab, _wallPrefab;
    public float xOffset;
    public float yOffset;

    public void GenerateRoom()
    {
        GameObject room = new GameObject("Room");
        for (int i = -1; i <= 3; i++)
        {
            for (int j = -1; j <= 9; j++)
            {
                if (i < 0 || j < 0 || i == 3 || j == 9)
                {
                    var wll = Instantiate(_wallPrefab, room.transform);
                    wll.transform.localPosition = new Vector2((i - 1) * xOffset, (j - 4) * yOffset);
                }
                else
                {
                    GameObject obj;

                    if (Random.Range(0, 3) == 1)
                        obj = Instantiate(_wallPrefab, room.transform);
                    else
                        obj = Instantiate(_floorPrefab, room.transform);

                    obj.transform.localPosition = new Vector2((i - 1) * xOffset, (j - 4) * yOffset);
                }


            }
        }
    }
}
