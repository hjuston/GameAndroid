using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	public Text SellCostLabel;
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
		// Tworzenie przycisków do tworzenia farm
		InitializeBuildingButtons();
	}

	/// <summary>
	/// Metoda zamyka panel informacji o budowli. Wywoływane przez przycisk Close
	/// </summary>
	public void ClosePanel()
	{
		Helper.GetTileManager().SelectTile(null);
		UpgradePanel.SetActive(false);
	}

	/// <summary>
	/// Metoda ustawia informacje o budynku w panelu informacji.
	/// </summary>
	/// <param name="building"></param>
	public void SetBuildingInfo(Building building)
	{
		UpgradePanel.SetActive(true);
		if (building == null)
		{
			BuildingNameLabel.text = "none";
			UpgradeNumberText.text = "";

			BuildingIncomeLabel.text = "none";

			UpgradeCostLabel.text = "";
			SellCostLabel.text = "";

			SetSellButtonAvailable(false);
			SetUpgradeButtonAvailable(false);
		}
		else
		{

			BuildingNameLabel.text = building.Name;
			UpgradeNumberText.text = building.UpgradeNumber.ToString();

			BuildingIncomeLabel.text = Helper.GetDisplayableValue(building.GetIncome());

			UpgradeCostLabel.text = Helper.GetDisplayableValue(building.GetCost());
			SellCostLabel.text = Helper.GetDisplayableValue(building.GetSellPrice());

			SetSellButtonAvailable(true);
			SetUpgradeButtonAvailable(true);
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

		// Ukrywanie/wyświetlanie przycisków w zależności od gotówki
		DisplayAvailableBuildingButtons(money);
	}


	/// <summary>
	/// Metoda ukrywa lub wyświetla przycisku w zależności od ilości gotówki
	/// </summary>
	void DisplayAvailableBuildingButtons(BigInteger money)
	{
		Button[] upgradeButtons = BuildingButtonPanel.GetComponentsInChildren<Button>(true);
		foreach (Button upgradeButton in upgradeButtons)
		{
			UpgradeButton script = GetButtonScript(upgradeButton);
			if (script != null)
			{
				upgradeButton.gameObject.SetActive(script.GetBuildingCost() > money ? false : true);
			}
		}
	}

	UpgradeButton GetButtonScript(Button btn)
	{
		UpgradeButton result = null;

		if (btn != null)
		{
			result = btn.GetComponentInParent<UpgradeButton>();
		}

		return result;
	}

	/// <summary>
	/// Tworzenie przycisków za pomocą których można umieszczać budynki.
	/// </summary>
	void InitializeBuildingButtons()
	{
		Building[] buildings = Helper.GetBuildingManager().GetAllBuildings();
		foreach (Building building in buildings)
		{
			GameObject buildingButton = Button.Instantiate(building.ButtonPrefab);
			buildingButton.transform.SetParent(BuildingButtonPanel.transform);

			// Ustawianie nazwy przycisku
			Text btnText = buildingButton.GetComponentInChildren<Text>();
			if (btnText != null)
			{
				btnText.text = building.Name.Substring(0, 1);
			}
		}
	}


	/// <summary>
	/// Metoda aktywuje lub dezaktywuje przycisk SellButton. W przypadku wyświetlenia zmieniany jest jego opis
	/// uwzględniając cenę sprzedaży budynku.
	/// </summary>
	/// <param name="available"></param>
	/// <param name="buildingCost"></param>
	public void SetSellButtonAvailable(bool available)
	{
		SellButton.gameObject.SetActive(available);
		if (available)
		{
			BigInteger sellPrice = Helper.GetTileManager().CurrentTile.Building.GetSellPrice();
		}
	}


	/// <summary>
	/// Metoda aktywuje lub dezaktywuje przycisk UpgradeButton. W przypadku wyświetlenia zmieniany jest jego opis
	/// uwzględniając cenę ulepszenia budynku.
	/// </summary>
	/// <param name="available"></param>
	public void SetUpgradeButtonAvailable(bool available)
	{
		UpgradeButton.gameObject.SetActive(available);
		if (available)
		{
			BigInteger upgradePrice = Helper.GetTileManager().CurrentTile.Building.GetCost();
		}
	}

	/// <summary>
	/// Metoda ukrywa/pokazuje menu z upgradeami
	/// </summary>
	bool isOpen = false;
	public void ToggleMenu()
	{
		Animator animator = UpgradePanel.GetComponent<Animator>();
		if(animator != null)
		{
			isOpen = !isOpen;
			animator.SetBool("IsOpen", isOpen);
		}
	}
}
