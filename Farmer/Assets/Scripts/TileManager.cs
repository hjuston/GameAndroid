using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
	public Tile CurrentTile;

    public GameObject cube;

    public Material IdleTileMaterial;
    public Material SelectedTileMaterial;
	
    public float CubeSize = 1;
    public float buildingWidht;
    public float buildingHeight;

	public Rotation rotation = Rotation.Horizontal;

	public enum Rotation
	{
		Horizontal = 0,
		Vertical = 1
	}


    bool CheckCubes(GameObject focusedTile)
    {
		float focusedX = focusedTile.transform.position.x;
		float focusedZ = focusedTile.transform.position.z;

		// od miejsca kliknięcia do miejsca kliknięcia + szerokość
		for (float x = focusedX; x <= focusedX + buildingWidht; x++)
		{
			// od miejsca kliknięcia do miejsca kliknięcia - wysokość (kratki zawsze ułożone są w dół
			for(float z = focusedZ; z > focusedZ - buildingHeight; z--)
			{
				Vector3 cubePosition = new Vector3(x, 0f, z);
				Vector3 direction = (cubePosition - Camera.main.transform.position).normalized;

				Ray ray = new Ray(Camera.main.transform.position, direction);
				RaycastHit hitInfo;

				if (Physics.Raycast(ray, out hitInfo))
				{
					hitInfo.collider.gameObject.GetComponent<Renderer>().material = SelectedTileMaterial;
				}
				else
				{
					return false;
				}
			}
		}
		return true;
    }

    void Update()
    {
        // Sprawdzanie, czy użytkownik kliknął na pole
        //if (Input.GetMouseButtonDown(0))
        //{
        //	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //	RaycastHit hitInfo;

        //	if (Physics.Raycast(ray, out hitInfo))
        //	{
        //		if (hitInfo.collider.gameObject.tag == "BuildingTile")
        //		{
        //			SelectTile(hitInfo.collider.gameObject);
        //		}
        //	}
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    RaycastHit hitInfo;

        //    if (Physics.Raycast(ray, out hitInfo))
        //    {
        //        if (hitInfo.collider.tag == "selectable")
        //        {
        //            //Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 20f);
        //            // Sprawdzanie czy kratki które są obok w podanych wymiarach są wolne

        //            if (CheckCubes(hitInfo.collider.gameObject))
        //            {
        //                Debug.Log("JEST OK");
        //            }
        //            else
        //            {
        //                Debug.Log("NIE JEST OK");
        //            }




        //            if (hitInfo.collider.gameObject != cube)
        //            {
        //                if (cube != null)
        //                    cube.GetComponent<Renderer>().material = IdleTileMaterial;

        //                cube = hitInfo.collider.gameObject;
        //                cube.GetComponent<Renderer>().material = SelectedTileMaterial;
        //            }
        //        }
        //        else
        //        {
        //            if (cube != null)
        //                cube.GetComponent<Renderer>().material = IdleTileMaterial;

        //            cube = null;
        //        }
        //    }
        //    else
        //    {
        //        if (cube != null)
        //            cube.GetComponent<Renderer>().material = IdleTileMaterial;

        //        cube = null;
        //    }
        //}
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
