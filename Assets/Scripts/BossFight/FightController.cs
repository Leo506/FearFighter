using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    public GameObject[] dashBlocks;
    public Pointer pointer;

    int activeBlockIndex = 0;

    private void Start()
    {
        pointer.target = dashBlocks[activeBlockIndex].transform;
    }

    public void ActiveNextBlock()
    {
        activeBlockIndex++;
        if (activeBlockIndex < dashBlocks.Length)
        {
            dashBlocks[activeBlockIndex].SetActive(true);
            pointer.target = dashBlocks[activeBlockIndex].transform;
        }
    }
}
