using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] _floorPrefabs;
    [SerializeField] GameObject[] _wallPrefabs;
    [SerializeField] GameObject[] _obstacles;
    [SerializeField] float _roomOffset = 10.15f;
    public float timeToChange;
    GameObject currentRoom, nextRoom;

    private void Start()
    {
        currentRoom = CreateRoom();
        
        currentRoom.transform.position = Vector3.zero;

    }

    IEnumerator ChangeRooms()
    {
        var deltaY = (nextRoom.transform.position - currentRoom.transform.position).magnitude / 100;
        for (int i = 0; i < 100; i++)
        {
            nextRoom.transform.Translate(0, -deltaY, 0);
            currentRoom.transform.Translate(0, -deltaY, 0);
            yield return new WaitForSeconds(timeToChange / 100);
        }

        Destroy(currentRoom.gameObject);
        currentRoom = nextRoom;
        GenerateObstacles();
    }


    public void GenerateRoom()
    {
        nextRoom = CreateRoom();
        nextRoom.transform.position = new Vector3(0, _roomOffset, 0);
        StartCoroutine(ChangeRooms());
    }

    GameObject CreateRoom()
    {
        GameObject room = new GameObject("Room");

        GameObject floor = _floorPrefabs[Random.Range(0, _floorPrefabs.Length)];
        GameObject wall = _wallPrefabs[Random.Range(0, _wallPrefabs.Length)];

        Instantiate(floor, room.transform);
        Instantiate(wall, room.transform);

        return room;
    }

    void GenerateObstacles()
    {
        Instantiate(_obstacles[Random.Range(0, _obstacles.Length)]);
    }
}
