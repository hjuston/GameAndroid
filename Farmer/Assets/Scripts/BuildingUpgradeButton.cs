using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingUpgradeButton : MonoBehaviour
{
    Upgrade _upgrade;

    public void InitializeButton(Upgrade upgrade)
    {
        this._upgrade = upgrade;
        
        Button btn = gameObject.GetComponentInChildren<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => BuyUpgrade());
        }
    }

    public void BuyUpgrade()
    {
        Helper.GetGameManager().BuyBuildingUpgrade(_upgrade);
    }
}
