using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour
{
	// InfoPanel
	public Text MoneyLabel;
	public Text MoneyGenerateText;


	// Upgrades panel
	public GameObject UpgradePanel;

	// building name & income
	public Text BuildingNameLabel;
	public Text BuildingIncomeLabel;

	// sell cost & upgrade cost
	public Text UpgradeCostLabel;

	// panel with upgrades
	public GameObject BuildingButtonPanel;

	// number of upgrades
	public Text UpgradeNumberText;

	// sell and upgrade button
	public Button SellButton;
	public Button UpgradeButton;

	void Start()
	{
        InitializeStandardBuildingsPanelButtons();
	}

    #region ** EditMode panel
    [Header("EditMode panel")]
    public GameObject EditPanel;
    
    public GameObject BuildingsPanel;
    public GameObject BuildingInfoPanel;

    public GameObject GhostPositionGroup;


    bool editModePanelVisible = false;
    public void ToggleEditPanel()
    {
        editModePanelVisible = !editModePanelVisible;

        if(editModePanelVisible == true)
        {
            DisplayUpgradesPanel(false);
        }

        Helper.GetGameManager().SetEditMode(editModePanelVisible);
        EditPanel.SetActive(editModePanelVisible);
    }

    public void SetGhostPositionGroupVisible(bool visible)
    {
        GhostPositionGroup.SetActive(visible);
    }

    /// <summary>
    /// Wyświetlenie menu budynków
    /// </summary>
    public void ShowBuildingsPanel()
    {
        Animator animator = BuildingsPanel.GetComponent<Animator>();
        if(animator != null)
        {
            bool isVisible = animator.GetBool("IsVisible");
            if(!isVisible)
                animator.SetBool("IsVisible", true);
        }
    }

    /// <summary>
    /// Ukrywanie menu budynków
    /// </summary>
    public void HideBuildingsPanel()
    {
        Animator animator = BuildingsPanel.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsVisible", false);
        }
    }

    /// <summary>
    /// Metoda inicjalizuje panel Standardowych budowli - wypełnia go przyciskami
    /// </summary>
    public GameObject StandardBuildingsContent;
    public void InitializeStandardBuildingsPanelButtons()
    {
        GameObject[] standardBuildings = BuildingsDatabase.GetBuildingsByType(BuildingType.Standard);
        Dictionary<GameObject, Building> temp = new Dictionary<GameObject, Building>();

        // Pobieranie listy budynków
        foreach (GameObject building in standardBuildings)
        {
            Building buildingScript = building.GetComponent<Building>();

            if(buildingScript != null)
            {
                temp.Add(building, buildingScript);
            }
        }

        // Inicjalizowanie budynków
        foreach(KeyValuePair<GameObject, Building> building in temp.OrderBy(x => x.Value.iBaseCost))
        {
            // Tworznie obiektu przycisku i wrzucanie go do kontenera
            GameObject buildingButton = Button.Instantiate(BuildingInfoPanel);
            buildingButton.transform.SetParent(StandardBuildingsContent.transform, false);

            BuildingInfoPanel infoPanelScript = buildingButton.GetComponent<BuildingInfoPanel>();
            if (infoPanelScript != null)
            {
                infoPanelScript.Initialize(building.Value, building.Key);
            }
        }
    }
	#endregion

	#region ** GameStats panel
    [Header("Game stats")]
	public GameObject ExperiencePanel;
	public void SetExperiencePanelValue(float percentage)
	{
		ExperiencePanel.transform.localScale = new Vector3( percentage, ExperiencePanel.transform.localScale.y, ExperiencePanel.transform.localScale.z);
	}

    public Text LevelText;
    public void SetLevelValue(int level)
    {
        LevelText.text = level.ToString();
    }

    public Text ExperienceText;
    public void SetExperienceValue(int current, int required)
    {
        ExperienceText.text = string.Format("{0}/{1}", current, required);
    }
    #endregion

    /// <summary>
    /// Metoda zamyka panel informacji o budowli. Wywoływane przez przycisk Close
    /// </summary>
    public void ClosePanel()
	{
		UpgradePanel.SetActive(false);
	}

	/// <summary>
	/// Metoda ustawia informacje o budynku w panelu informacji.
	/// </summary>
	/// <param name="building"></param>
	public void SetBuildingInfo(Building building)
	{
		if (building == null)
		{
            DisplayUpgradesPanel(false);
		}
		else
		{
            BuildingNameLabel.text = building.Name;
			UpgradeNumberText.text = building.BuildingLevel.ToString();

			BuildingIncomeLabel.text = Helper.GetDisplayableValue(building.GetIncome());

			UpgradeCostLabel.text = Helper.GetDisplayableValue(building.GetCost());

            Helper.GetGameManager().SetCurrentlySelectedBuilding(building);

            DisplayUpgradesPanel(true);
        }
	}

    public void SetBuildingUpgradeCostInfo(Building building, int levels)
    {
        if (building == null)
        {
            DisplayUpgradesPanel(false);
        }
        else
        {
            UpgradeCostLabel.text = Helper.GetDisplayableValue(building.CalculateCostForNextXLevels(levels));
        }
    }


    /// <summary>
    /// Metoda wyświetla informacje o gotówce generowanej w ciągu jednej sekundy.
    /// </summary>
    /// <param name="money"></param>
    public void SetMoneyGenerateInfo(BigInteger money)
	{
		MoneyGenerateText.text = Helper.GetDisplayableValue(money);
	}


	/// <summary>
	/// Metoda ustawia informacje o aktualnej gotówce.
	/// </summary>
	/// <param name="building"></param>
	public void SetMoneyInfo(BigInteger money)
	{
		MoneyLabel.text = Helper.GetDisplayableValue(money);
	}

	/// <summary>
	/// Metoda ukrywa/pokazuje menu z upgradeami
	/// </summary>
	public void DisplayUpgradesPanel(bool visible)
	{
		Animator animator = UpgradePanel.GetComponent<Animator>();
		if(animator != null)
		{
			animator.SetBool("IsOpen", visible);
		}
	}
}
