using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {

    public BuildingType Type;

	void Start()
	{
		Button btn = gameObject.GetComponentInChildren<Button>();
		if(btn != null)
		{
			btn.onClick.AddListener(() => Helper.GetTileManager().SetBuildingOnCurrentTile(Type));
		}
	}


    /// <summary>
    /// Metoda zwraca koszt budynku
    /// </summary>
    /// <returns></returns>
    public BigInteger GetBuildingCost()
    {
        return Helper.GetBuildingManager().GetBuildingByType(Type).GetCost();
    }
}
