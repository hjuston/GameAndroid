using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
	public Tile CurrentTile;

    public GameObject cube;
    public float SelectedCubeX = 0f;
    public float SelectedCubeZ = 0f;

    public Material IdleTileMaterial;
    public Material SelectedTileMaterial;


    public float CubeSize = 1;
    public float buildingWidht = 3f;
    public float buildingHeight = 2f;
    
    bool CheckCubes(GameObject focusedTile)
    {
        Debug.Log("Pozycja przesłanego: X=" + focusedTile.transform.position.x + " Z=" + focusedTile.transform.position.z);
        for (float x = focusedTile.transform.position.x ; x <= focusedTile.transform.position.x + buildingHeight; x++)
        {
            Debug.Log("PETLA 1: X=" + x);
            for (float z = focusedTile.transform.position.z ; z >= focusedTile.transform.position.z - buildingWidht; z--)
            {
                Debug.Log("PETLA 2: Z=" + z);
                Vector3 cubePosition = new Vector3(focusedTile.transform.position.x - x, focusedTile.transform.position.y, focusedTile.transform.position.z - z);

                // Puszczanie raya żeby sprawdzić czy na pozycji obok jest cube
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.position - cubePosition);

                Debug.Log(cubePosition);
                Debug.Log("Casting ray");
                Debug.DrawRay(ray.origin, ray.direction * 10000, Color.red, 20f);

                RaycastHit hitInfo;

                if(!Physics.Raycast(ray, out hitInfo))
                {
                    return false;
                }
                else
                {
                    hitInfo.collider.gameObject.GetComponent<Renderer>().material = SelectedTileMaterial;
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

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.tag == "selectable")
                {
                    //Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 20f);
                    // Sprawdzanie czy kratki które są obok w podanych wymiarach są wolne

                    SelectedCubeX = hitInfo.collider.gameObject.transform.localPosition.x;
                    SelectedCubeZ = hitInfo.collider.gameObject.transform.localPosition.z;

                    if (CheckCubes(hitInfo.collider.gameObject))
                    {
                        Debug.Log("JEST OK");
                    }
                    else
                    {
                        Debug.Log("NIE JEST OK");
                    }




                    if (hitInfo.collider.gameObject != cube)
                    {
                        if (cube != null)
                            cube.GetComponent<Renderer>().material = IdleTileMaterial;

                        cube = hitInfo.collider.gameObject;
                        cube.GetComponent<Renderer>().material = SelectedTileMaterial;
                    }
                }
                else
                {
                    if (cube != null)
                        cube.GetComponent<Renderer>().material = IdleTileMaterial;

                    cube = null;
                }
            }
            else
            {
                if (cube != null)
                    cube.GetComponent<Renderer>().material = IdleTileMaterial;

                cube = null;
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
