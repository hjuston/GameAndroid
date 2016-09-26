using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	private Building _building;
	public Building Building { get { return _building; } set { _building = value; } }

	/// <summary>
	/// Metoda przypisuje danej Tile odpowiedni budynek jednocześnie tworząc obiekt Prefab budynku jako child.
	/// </summary>
	/// <param name="building"></param>
	public void SetBuilding(Building building)
	{
		Building = building;

		if (building == null)
		{
			Building = null;
			foreach(Transform child in gameObject.transform)
			{
				Object.Destroy(child.gameObject);
			}

			Helper.GetGUIManager().SetBuildingInfo(building);
		}
		else
		{
			GameObject farm = Instantiate(building.Prefab);
			farm.transform.SetParent(gameObject.transform);
			farm.transform.localPosition = new Vector3(0f, 12, -0.15f);
			farm.transform.localRotation = Quaternion.identity;
		}
	}
}
