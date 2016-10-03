using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingInfoPanel : MonoBehaviour
{
    public Text BuildingNameText;
    public Text BuildingDescriptionText;
    public Text BuildingCostText;
    public Image BuildingImage;
    public Button BuyBuildingButton;

    private GameObject buildingPrefab;

    public void Initialize(Building building, GameObject buildingPrefab)
    {
        BuildingNameText.text = building.Name;
        BuildingDescriptionText.text = building.Description;
        BuildingCostText.text = string.Format("$ {0}", Helper.GetDisplayableValue(building.GetCost()));

        BuildingImage.sprite = building.BuildingImage;

        this.buildingPrefab = buildingPrefab;
        BuyBuildingButton.onClick.AddListener(() => SpawnGhost());
    }

    void SpawnGhost()
    {
        Building buildingScript = buildingPrefab.GetComponent<Building>();
        if(buildingScript != null)
        {
            if(Helper.GetGameManager().GetCurrentMoney() >= buildingScript.GetCost())
            {
                Helper.GetGridManager().SpawnGhostObject(buildingPrefab);
            }
        }
    }
}
