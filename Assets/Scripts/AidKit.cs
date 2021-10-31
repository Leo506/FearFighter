using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidKit : BonusItem
{
    public override void ActivateBonusItem()
    {
        base.ActivateBonusItem();
        // Увеличение количества храбрости
        Debug.Log("Храбрость увеличена");
        Destroy(this.gameObject);
    }
}
