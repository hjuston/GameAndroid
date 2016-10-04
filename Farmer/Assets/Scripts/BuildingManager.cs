using UnityEngine;
using System.Collections;
using System;

public class BuildingManager : MonoBehaviour {

    public Building[] StandardBuildings;
    public Building[] InfrastructureBuildings;
    public Building[] PrestigeBuildings;

    //void Start()
    //{
    //    // Inicjalizacja kosztów budynków
    //    foreach (Building building in Buildings)
    //    {
    //        string cost;
    //        string income;
    //        GetBuildingDefaultCost(building.Type, out cost, out income);

    //        building.BaseCost = new BigInteger(cost);
    //        building.BaseIncome = new BigInteger(income);
    //    }
    //}

    //private void GetBuildingDefaultCost(Buildings type, out string cost, out string income)
    //{
    //    switch(type)
    //    {
    //        case global::Buildings.SmallHouse:
    //            cost = "40";
    //            income = "1";
    //            break;
    //        case global::Buildings.BigHouse:
    //            cost = "300";
    //            income = "7";
    //            break;
    //        case global::Buildings.Cinema:
    //            cost = "1700";
    //            income = "25";
    //            break;
    //        case global::Buildings.Hotel:
    //            cost = "12000";
    //            income = "110";
    //            break;
    //        case global::Buildings.Sludgework:
    //            cost = "51000";
    //            income = "900";
    //            break;
    //        default:
    //            cost = "10";
    //            income = "1";
    //            break;
    //    }
    //}

    //public Building[] GetAllBuildings()
    //{
    //    return Buildings;
    //}

    //public Building GetBuildingByType(Buildings type)
    //{
    //    foreach(Building building in Buildings)
    //    {
    //        if (building.Type == type) return building;
    //    }

    //    return null;
    //}
}
