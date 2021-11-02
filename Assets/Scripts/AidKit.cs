using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidKit : BonusItem
{
    public override void ActivateBonusItem()
    {
        base.ActivateBonusItem();
        FindObjectOfType<PlayeLogic>().PlayerHP += 10;
        Destroy(this.gameObject);
    }
}
