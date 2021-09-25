using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLvl : MonoBehaviour
{
    [SerializeField] GameObject[] _biomsSprites;
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
            int figureIndex = Random.Range(0, _biomsSprites.Length);
            GameObject figure = Instantiate(_biomsSprites[figureIndex], this.transform);
            figure.transform.localPosition = new Vector2(0, i * _offset);
            Debug.Log(i * _offset);
        }
    }
}
