using UnityEngine;
using System.Collections;
using System;

public class BuildingManager : MonoBehaviour {

    public Building[] Buildings;
    
    void Start()
    {
        // Inicjalizacja kosztów budynków
        foreach (Building building in Buildings)
        {
            string cost;
            string income;
            GetBuildingDefaultCost(building.Type, out cost, out income);

            building.BaseCost = new BigInteger(cost);
            building.BaseIncome = new BigInteger(income);
        }
    }
    
    private void GetBuildingDefaultCost(BuildingType type, out string cost, out string income)
    {
        switch(type)
        {
            case BuildingType.CerealFarm:
                cost = "40";
                income = "1";
                break;
            case BuildingType.OatFarm:
                cost = "300";
                income = "7";
                break;
            case BuildingType.BarleyFarm:
                cost = "1700";
                income = "25";
                break;
            case BuildingType.FlaxFarm:
                cost = "12000";
                income = "110";
                break;
            case BuildingType.RyeFarm:
                cost = "51000";
                income = "900";
                break;
            case BuildingType.MaltFarm:
                cost = "225000";
                income = "3950";
                break;
            default:
                cost = "10";
                income = "1";
                break;
        }
    }

    public Building[] GetAllBuildings()
    {
        return Buildings;
    }

    public Building GetBuildingByType(BuildingType type)
    {
        foreach(Building building in Buildings)
        {
            if (building.Type == type) return building;
        }

        return null;
    }
}
