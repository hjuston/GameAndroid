using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour {

    //public Buildings Type;

    public Building Building;

	void Start()
	{
		Button btn = gameObject.GetComponentInChildren<Button>();
		if (btn != null)
		{
			btn.onClick.AddListener(() => Helper.GetGridManager().SpawnGhostObject(Building.BuildingPrefab));
		}
	}


	///// <summary>
	///// Metoda zwraca koszt budynku
	///// </summary>
	///// <returns></returns>
	//public BigInteger GetBuildingCost()
	//{
	//    return Helper.GetBuildingManager().GetBuildingByType(Type).GetCost();
	//}
}
