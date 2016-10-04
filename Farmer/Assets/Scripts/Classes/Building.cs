using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[Serializable]
public class Building : MonoBehaviour {

    // Nazwa i typ
    public string Name;
    public BuildingType BuildingType;
    public string Description;
    public Sprite BuildingImage;

    // Poziom ulepszenia budynku
    public int BuildingLevel = 1;

    // Bazowy przychód i koszt budynku
    public int iBaseIncome;
    public BigInteger BaseIncome;

    public int iBaseCost;
    public BigInteger BaseCost;

    public int BuildingUpgardeExperience;
    public int BuildingBuyExperience;

    // Mnożnik kosztów budynku (1.07 - 1.15)
    public float CostMultiplier;

    // Lista ulepszeń budynku (aktywne i nieaktywne). Na jej podstawie zliczane są bonusy
    public Upgrade[] Upgrades;

    // Właściwość określa, czy budynek został postawiony za pomocą edytora.
    // Jeżeli tak to przy burzeniu należy zwrócić część kwoty.
    public bool IsPlacedForReal = false;

    // Prefaby budynku
    //public GameObject BuildingPrefab;
    public GameObject ButtonPrefab;
    
    public void InitializeBase()
    {
        BaseIncome = new BigInteger(iBaseIncome);
        BaseCost = new BigInteger(iBaseCost);
    }
    
    /// <summary>
    /// Metoda oblicza koszt budowli na podstawie poziomu ulepszenia
    /// </summary>
    /// <returns></returns>
    public BigInteger GetCost()
    {
		// Wzór BaseCost * CostMultiplier ^ (BuildingLevel)
		if (BaseCost == null) InitializeBase();

        return SimulateCost(BuildingLevel);
    }

    private BigInteger SimulateCost(int level)
    {
        return BaseCost * ((float)Math.Pow(CostMultiplier, level));
    }

    /// <summary>
    /// Metoda oblicza generowany przychód na podstawie poziomu ulepszenia
    /// </summary>
    /// <returns></returns>
    public BigInteger GetIncome()
    {
		if (BaseIncome == null) InitializeBase();

		// Wzór BaseIncome  * BuildingLevel
		return BaseIncome * new BigInteger(BuildingLevel);
    }


    /// <summary>
    /// Metoda oblicza kwotę, za którą można sprzedać budynek
    /// </summary>
    /// <returns></returns>
    public BigInteger GetSellPrice()
    {
        return GetCost() / 3;
    }

    /// <summary>
    /// Metoda ulepsza budynek
    /// </summary>
    public void Upgrade(int levels)
    {
        BuildingLevel += levels;
    }

    /// <summary>
    /// Pobiera kopię prefabu (unlinked) - nie wiadomo czy będzie potrzebne?
    /// </summary>
    /// <returns></returns>
    public Building GetCopy()
    {
        return this.MemberwiseClone() as Building;
    }

    public BigInteger CalculateCostForNextXLevels(int levels)
    {
        BigInteger result = new BigInteger("0");

        for(int level = BuildingLevel; level < BuildingLevel + levels; level++)
        {
            BigInteger simulatedCost = SimulateCost(level);
            result += simulatedCost;
        }

        return result;
    }
}
