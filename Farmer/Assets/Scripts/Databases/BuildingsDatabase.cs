﻿using UnityEngine;
using System.Collections;

public static class BuildingsDatabase {

    private static GameObject[] _standardBuildings;
    private static Building[] _infrastructureBuildings;
    private static Building[] _prestigeBuildings;
    private static bool _isLoaded = false;

   public static void LoadDatabase()
    {
        _standardBuildings = Resources.LoadAll<GameObject>(@"Buildings/Standard");
        InitializeBase(_standardBuildings);

        _infrastructureBuildings = Resources.LoadAll<Building>(@"Buildings/Infrastructure");
        _prestigeBuildings = Resources.LoadAll<Building>(@"Buildings/Prestige");
        _isLoaded = true;
    }

    static void InitializeBase(GameObject[] buildings)
    {
        foreach(GameObject building in buildings)
        {
            Building buildingScript = building.GetComponent<Building>();
            if(buildingScript != null)
            {
                buildingScript.InitializeBase();
            }
        }
    }


    static void VerifyDatabase()
    {
        if (!_isLoaded) LoadDatabase();
    }

    public static GameObject[] GetBuildingsByType(BuildingType type)
    {
        VerifyDatabase();

        switch(type)
        {
            case BuildingType.Infrastructure: return null;
            case BuildingType.Prestige: return null;
            case BuildingType.Standard: return _standardBuildings;
            default: return null;
        }
    }
}
