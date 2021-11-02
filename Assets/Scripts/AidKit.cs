using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidKit : BonusItem
{
    public override void ActivateBonusItem()
    {
        base.ActivateBonusItem();
        // Увеличение количества храбрости
        Destroy(this.gameObject);
    }
}
