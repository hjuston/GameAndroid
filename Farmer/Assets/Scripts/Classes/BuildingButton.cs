using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public GameObject building;

	public void InitializeEvent(GameObject prefab)
	{
        building = prefab;

		Button btn = gameObject.GetComponentInChildren<Button>();
		if (btn != null)
		{
            btn.onClick.AddListener(() => SpawnGhost());
		}
	}

    private void SpawnGhost()
    {
        Building buildingScript = building.GetComponent<Building>();
        if(buildingScript != null)
        {
			buildingScript.InitializeBase();

            BigInteger currentMoney = Helper.GetGameManager().GetCurrentMoney();
            BigInteger cost = buildingScript.GetCost();

            if (currentMoney >= cost)
            {
                Helper.GetGridManager().SpawnGhostObject(building);
            }
            else
            {
                Debug.Log("Brak gotówki -- IMPLEMENT MESSAGE");
            }
        }

        
    }
}
