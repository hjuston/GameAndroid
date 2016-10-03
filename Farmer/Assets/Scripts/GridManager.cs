using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
	// Tymczasowy prefab który należy utworzyć jako ghost object
	public GameObject Cube;

	// Ghost object i jego materiał
	private GameObject _ghostObject;
	private GameObject GhostObject
	{
		get { return _ghostObject; }
		set { _ghostObject = value; /*_ghostObjectOriginalColor = (value == null) ? Color.white : value.GetComponent<Renderer>().material.color;*/ }
	}

	// Przycisk potwierdzenia do postawienia budynku
	public GameObject GhostFollowingButton;

	void Update()
	{
		// Sprawdzanie czy w editMode został kliknięty budynek - jeżeli tak to ustawia go jako tymczasowego ghostmode
		// można go wtedy przenieść lub sprzedać
		if (Helper.GetGameManager().IsEditMode())
		{
			if(Input.GetMouseButtonDown(0))
			{
				RaycastHit hitInfo;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if(Physics.Raycast(ray, out hitInfo))
				{
					if (hitInfo.collider.tag == CONSTS.BuildingTag && GhostObject == null)
					{
						GhostObject = hitInfo.collider.gameObject;
						ActivateGhostObject();
					}
				}
			}
		}
	}

	/// <summary>
	/// Metoda tworzy obiekt Ghost
	/// </summary>
	public void SpawnGhostObject(GameObject prefab)
	{
		if (GhostObject == null)
		{
			GhostObject = GameObject.Instantiate<GameObject>(prefab);
			ActivateGhostObject();
		}
	}

	private void ActivateGhostObject()
	{
		// Activate following button to move object
		GhostFollowingButton.SetActive(true);
		GhostFollowingButtonScript ghostFollowScript = GhostFollowingButton.GetComponent<GhostFollowingButtonScript>();
		ghostFollowScript.SetGhostObject(GhostObject);

		GhostScript ghostScript = GhostObject.GetComponent<GhostScript>();
		ghostScript.isGhost = true;

        Helper.GetGUIManager().SetGhostPositionGroupVisible(true);
	}

	/// <summary>
	/// Zamienia obiekt Ghost na budynek
	/// </summary>
	public void DropObject()
	{
		GhostScript ghost = GhostObject.GetComponent<GhostScript>();
		if (ghost.canPlace)
		{
			// Place object
			ghost.isGhost = false;

			// Hide following button to place button
			GhostFollowingButtonScript ghostFollowScript = GhostFollowingButton.GetComponent<GhostFollowingButtonScript>();
			ghostFollowScript.UnsetGhostObject();
			GhostFollowingButton.SetActive(false);

			// Ustawianie budynku jako child grupy budynków
			GhostObject.transform.SetParent(Helper.GetBuildingsGroup().transform);

			// Informowanie budowli, że została wybudowana
			Building buildingScript = GhostObject.GetComponent<Building>();
			if (buildingScript != null)
			{
				if(!buildingScript.IsPlacedForReal)
				{
					buildingScript.IsPlacedForReal = true;
					
					// Wydanie pieniędzy
					Helper.GetGameManager().SpendMoney(buildingScript.GetCost());
                    Helper.GetGameStats().AddExperience(buildingScript.BuildingBuyExperience);
				}
			}

			GhostObject = null;
            Helper.GetGUIManager().SetGhostPositionGroupVisible(false);
            Helper.GetGUIManager().SetMoneyGenerateInfo(Helper.GetGameManager().GetCurrentIncome());
        }
	}

	/// <summary>
	/// Metoda powoduje anulowanie stawiania lub sprzedania budynku
	/// </summary>
	public void CancelGhost()
	{
		GhostFollowingButtonScript ghostFollowScript = GhostFollowingButton.GetComponent<GhostFollowingButtonScript>();
		ghostFollowScript.UnsetGhostObject();
		GhostFollowingButton.SetActive(false);

        Building buildingScript = GhostObject.GetComponent<Building>();
        if (buildingScript != null)
        {
            if (!buildingScript.IsPlacedForReal)
            {
                GameObject.Destroy(GhostObject);
            }
        }

		GhostObject = null;
	}


    /// <summary>
    /// Jeżeli ghost jest wybudowanym wczesniej budynkiem to go sprzedaje. ( po kliknięciu przycisku sell )
    /// </summary>
    public void SellGhost()
    {
        GhostFollowingButtonScript ghostFollowScript = GhostFollowingButton.GetComponent<GhostFollowingButtonScript>();
        ghostFollowScript.UnsetGhostObject();
        GhostFollowingButton.SetActive(false);

        Building buildingScript = GhostObject.GetComponent<Building>();
        if (buildingScript != null)
        {
            if (buildingScript.IsPlacedForReal)
            {
                Helper.GetGameManager().AddMoney(buildingScript.GetSellPrice());
            }
        }
    }

	public void RotateGhost()
	{
		if (GhostObject != null)
		{
			GhostScript ghostScript = GhostObject.GetComponent<GhostScript>();
			ghostScript.Rotate();
		}
	}

	// Poruszanie obiektem Ghost
	private void GhostMove(float up, float down, float left, float right)
	{
		if (GhostObject != null)
		{
			GhostScript ghostScript = GhostObject.GetComponent<GhostScript>();
			ghostScript.MoveGhost(up, down, left, right);
		}
	}

	public void GhostMoveUp() { GhostMove(1, 0, 0, 0); }
	public void GhostMoveDown() { GhostMove(0, 1, 0, 0); }
	public void GhostMoveLeft() { GhostMove(0, 0, 1, 0); }
	public void GhostMoveRight() { GhostMove(0, 0, 0, 1); }
}
