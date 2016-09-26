using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
	public Tile CurrentTile;

    public Material IdleTileMaterial;
    public Material SelectedTileMaterial;

	void Update()
	{
        // Sprawdzanie, czy użytkownik kliknął na pole
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;

			if (Physics.Raycast(ray, out hitInfo))
			{
				if (hitInfo.collider.gameObject.tag == "BuildingTile")
				{
					SelectTile(hitInfo.collider.gameObject);
				}
			}
		}
	}


    /// <summary>
    /// Metoda która powoduje zaznaczenie wybranego pola
    /// </summary>
    /// <param name="tile"></param>
	public void SelectTile(GameObject tile)
	{
		if (tile != null)
		{
			Tile buildingTile = tile.GetComponent<Tile>();
			if (buildingTile != null)
			{
				if (CurrentTile != buildingTile)
				{
                    if (CurrentTile != null)
                        CurrentTile.gameObject.GetComponent<Renderer>().material = IdleTileMaterial;

					CurrentTile = buildingTile;
                    CurrentTile.gameObject.GetComponent<Renderer>().material = SelectedTileMaterial;
                }
                
                Helper.GetGUIManager().SetBuildingInfo(CurrentTile.Building);
            }
        }
        else
        {
            if (CurrentTile != null)
            {
                CurrentTile.gameObject.GetComponent<Renderer>().material = IdleTileMaterial;
                CurrentTile = null;
            }
        }
    }


    /// <summary>
    /// Metoda, która tworzy budynek na aktualnie wybranym polu
    /// </summary>
    /// <param name="type"></param>
	public void SetBuildingOnCurrentTile(BuildingType type)
	{
        if(CurrentTile.Building != null)
        {
            Debug.Log("Na wybranym polu aktualnie znajduje się budynek!");
        }
        else
        {
            Building building = Helper.GetBuildingManager().GetBuildingByType(type).GetCopy();

            if (Helper.GetGameManager().GetCurrentMoney() >= building.GetCost())
            {
                // Odejmij gotówkę
                Helper.GetGameManager().SpendMoney(building.GetCost());
                
                // Postaw budynek na danym polu
                CurrentTile.SetBuilding(building);
                
                // Dodaj budynek do listy budynków generujących gotówkę
                Helper.GetGameManager().AddBuildingToCurrentBuildingList(building);
                
                // Odśwież panel wyświetlający informację o polu
                Helper.GetGUIManager().SetBuildingInfo(CurrentTile.Building);
            }
            else
            {
                Debug.Log("Brak wystaczającej ilości gotówki.");
            }
        }
	}
}
