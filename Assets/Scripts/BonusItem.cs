 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusItem : MonoBehaviour
{
    [SerializeField] Pointer pointer;
    Pointer currentPointer;
    public virtual void ActivateBonusItem()
    {
        Destroy(currentPointer.gameObject);
    }

    public void Init()
    {
        currentPointer = Instantiate(pointer);
        currentPointer.target = this.transform;
    }
}
