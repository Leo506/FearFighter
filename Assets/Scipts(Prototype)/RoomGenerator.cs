using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] _floorPrefabs;
    [SerializeField] GameObject[] _wallPrefabs;
    GameObject fl, wll;

    private void Start()
    {
        GenerateRoom();
    }

    public void GenerateRoom()
    {
        if (fl != null || wll != null)
        {
            Destroy(fl.gameObject);
            Destroy(wll.gameObject);
        }
        GameObject floor = _floorPrefabs[Random.Range(0, _floorPrefabs.Length)];
        GameObject wall = _wallPrefabs[Random.Range(0, _wallPrefabs.Length)];
        fl = Instantiate(floor);
        wll = Instantiate(wall);
    }
}
