using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLvl : MonoBehaviour
{
    [SerializeField] GameObject[] _biomsSprites;
    [SerializeField] GameObject[] _obstacles;
    [SerializeField] float _offset;
    [SerializeField] int _lengthOfLvl;

    private void Start()
    {
        Create();
    }

    void Create()
    {
        for (int i = 0; i < _lengthOfLvl; i++)
        {
            CreateBiomCell(i);
        }
    }

    void CreateBiomCell(int index)
    {
        int figureIndex = Random.Range(0, _biomsSprites.Length);
        GameObject figure = Instantiate(_biomsSprites[figureIndex], this.transform);
        figure.transform.localPosition = new Vector2(0, index * _offset);
        CreateObstacle(figure);
    }

    void CreateObstacle(GameObject biomCell)
    {
        var a = Random.Range(0, 10);
        if (a == 1)
        {
            GameObject obstacle = Instantiate(_obstacles[Random.Range(0, _obstacles.Length)], biomCell.transform);
            obstacle.transform.localScale = new Vector2(obstacle.transform.localScale.x / biomCell.transform.localScale.x, obstacle.transform.localScale.y / biomCell.transform.localScale.y);
            Debug.Log("Препятствие создано!");
        }

        Debug.Log(a);
    }
}
