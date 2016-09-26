using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Building  {

    public string Name;
    
    public BigInteger BaseCost;
    public float CostMultiplier;

    public BigInteger BaseIncome;

    public GameObject Prefab;
    public GameObject ButtonPrefab;

    public BuildingType Type;
    public int UpgradeNumber = 0;

    /// <summary>
    /// Metoda oblicza koszt budowli na podstawie poziomu ulepszenia
    /// </summary>
    /// <returns></returns>
    public BigInteger GetCost()
    {
        // Wzór BaseCost * CostMultiplier ^ (UpgradeNumber)
        return BaseCost * ((float)Math.Pow(CostMultiplier, UpgradeNumber+1));
    }


    /// <summary>
    /// Metoda oblicza generowany przychód na podstawie poziomu ulepszenia
    /// </summary>
    /// <returns></returns>
    public BigInteger GetIncome()
    {
        // Wzór BaseIncome  * UpgradeNumber - np. 5, 10, 15, 20
        return BaseIncome * new BigInteger(UpgradeNumber == 0 ? 1 : UpgradeNumber);
    }


    /// <summary>
    /// Metoda oblicza kwotę, za którą można sprzedać budynek
    /// </summary>
    /// <returns></returns>
    public BigInteger GetSellPrice()
    {
        return BaseCost / 3;
    }

    /// <summary>
    /// Metoda ulepsza budynek
    /// </summary>
    public void Upgrade()
    {
        UpgradeNumber++;
    }

    public Building GetCopy()
    {
        return this.MemberwiseClone() as Building;
    }
}
