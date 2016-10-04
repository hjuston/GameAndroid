using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    private BigInteger _currentMoney = new BigInteger("123123331");

    private bool EditMode = false;

    private Building selectedBuilding;

    public void SetCurrentlySelectedBuilding(Building building)
    {
        selectedBuilding = building;
    }

    void Start()
    {
        // Wczytywanie bazy danych budynków
        BuildingsDatabase.LoadDatabase();

        InvokeRepeating("CollectMoney", 0f, 1f);
    }

    void Update()
    {
        // Sprawdzanie czy budynek został kliknięty? Jeżeli tak to wyświetlić menu ulepszeń
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (!IsEditMode() && hitInfo.collider.tag == "Building")
                {
                    Building buildingScript = hitInfo.collider.GetComponent<Building>();
                    if (buildingScript != null)
                    {
                        Helper.GetGUIManager().BuildingMode_SetBuildingInfo(buildingScript);

                        // Wyświetlanie markera
                        Helper.GetGUIManager().BuildingMode_CurrentBuildingMarkerShow();
                        Helper.GetGUIManager().BuildingMode_CurrentBuildingMarkerSetPosition(hitInfo.collider.transform.position);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Metoda zwraca informację, czy gra znajduje się w trybie edycji
    /// </summary>
    /// <returns></returns>
    public bool IsEditMode()
    {
        return EditMode;
    }

    /// <summary>
    /// Metoda zwiększa ilość gotówki (np. w przypadku sprzedania budynku)
    /// </summary>
    /// <param name="money"></param>
    public void AddMoney(BigInteger money)
    {
        _currentMoney += money;
        Helper.GetGUIManager().GameStats_SetCurrentMoneyInfo(_currentMoney);
    }


    /// <summary>
    /// Metoda zlicza przychody z budynków i dodaje je do aktualnej gotówki
    /// </summary>
    void CollectMoney()
    {
        // Zliczanie gotówki jeżeli gra nie jest w trybie edycji budowli
        if (!EditMode)
        {
            _currentMoney += GetCurrentIncome();//_generateMoneyCount;
            Helper.GetGUIManager().GameStats_SetCurrentMoneyInfo(_currentMoney);
        }
    }

    public BigInteger GetCurrentIncome()
    {
        BigInteger income = new BigInteger("0");

        GameObject buildingGroup = GameObject.FindGameObjectWithTag(CONSTS.BuildingsGroupTag);
        if(buildingGroup != null)
        {
            Building[] buildings = buildingGroup.GetComponentsInChildren<Building>();

            foreach (Building building in buildings)
            {
                income += building.GetIncome();
            }
        }

        return income;
    }

    /// <summary>
    /// Metoda zwraca aktualną ilość gotówki.
    /// </summary>
    /// <returns></returns>
    public BigInteger GetCurrentMoney()
    {
        return _currentMoney;
    }


    /// <summary>
    /// Metoda zmniejsza ilosć gotówki
    /// </summary>
    /// <returns></returns>
    public void SpendMoney(BigInteger money)
    {
        _currentMoney -= money;
        Helper.GetGUIManager().GameStats_SetCurrentMoneyInfo(_currentMoney);
    }

    /// <summary>
    /// Metoda włącza tryb edycji
    /// </summary>
    public void SetEditMode(bool val)
    {
        EditMode = val;
    }


    public void UpgradeBuilding()
    {
        BigInteger buildingCost = selectedBuilding.CalculateCostForNextXLevels(upgradeBy);

        if(_currentMoney >= buildingCost)
        {
            SpendMoney(buildingCost);
            selectedBuilding.Upgrade(upgradeBy);

            Helper.GetGameStats().AddExperience(selectedBuilding.BuildingUpgardeExperience * upgradeBy);

            Helper.GetGUIManager().BuildingMode_SetBuildingInfo(selectedBuilding);
            if(upgradeBy != 1)
            {
                Helper.GetGUIManager().BuildingMode_UpdateBuildingLevelCostInfo(selectedBuilding, upgradeBy);
            }

            Helper.GetGUIManager().GameStats_SetIncomeInfo(GetCurrentIncome());
        }
    }

    int upgradeBy = 1;
    public void ToggleUpgradeBy(int levels)
    {
        upgradeBy = levels;
        Helper.GetGUIManager().BuildingMode_UpdateBuildingLevelCostInfo(selectedBuilding, upgradeBy);
    }

    public void BuyBuildingUpgrade(Upgrade upgrade)
    {
        if(selectedBuilding != null && selectedBuilding.Upgrades.Contains(upgrade))
        {
            if(_currentMoney >= new BigInteger(upgrade.Cost) && selectedBuilding.BuildingLevel >= upgrade.RequiredLevel)
            {
                // Activate upgrade
                if (selectedBuilding.ActivateUpgrade(upgrade))
                {
                    SpendMoney(new BigInteger(upgrade.Cost));

                    Helper.GetGUIManager().BuildingMode_SetBuildingInfo(selectedBuilding);
                    if (upgradeBy != 1)
                    {
                        Helper.GetGUIManager().BuildingMode_UpdateBuildingLevelCostInfo(selectedBuilding, upgradeBy);
                    }

                    Helper.GetGUIManager().GameStats_SetIncomeInfo(GetCurrentIncome());
                }
            }
        }
    }
}
