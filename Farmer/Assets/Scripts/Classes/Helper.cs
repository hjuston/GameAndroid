using UnityEngine;
using System.Collections;
using System.Linq;
using System.ComponentModel;

public static class Helper {

	/// <summary>
	/// Metoda zwraca obiekt TileManager.
	/// </summary>
	/// <returns></returns>
	public static GridManager GetGridManager()
	{
		GridManager result = null;

		GameObject gridManagerObject = GameObject.FindGameObjectWithTag(CONSTS.GridManagerTag);
		if (gridManagerObject != null)
		{
			result = gridManagerObject.GetComponent<GridManager>();
		}

		return result;
	}


	/// <summary>
	/// Metoda zwraca obiekt GameManager
	/// </summary>
	/// <returns></returns>
	public static GameManager GetGameManager()
	{
		GameManager result = null;

		GameObject gameManagerObject = GameObject.FindGameObjectWithTag(CONSTS.GameManagerTag);
		if (gameManagerObject != null)
		{
			result = gameManagerObject.GetComponent<GameManager>();
		}

		return result;
	}


	/// <summary>
	/// Metoda zwraca obiekt GUIManager
	/// </summary>
	/// <returns></returns>
	public static GUIManager GetGUIManager()
	{
		GUIManager result = null;

		GameObject guiManagerObject = GameObject.FindGameObjectWithTag(CONSTS.GUIManagerTag);
		if (guiManagerObject != null)
		{
			result = guiManagerObject.GetComponent<GUIManager>();
		}

		return result;
	}

    public static string GetDisplayableValue(BigInteger value)
    {
        int tripleGroupsCount = ((value.ToString().Length) / 3);
        if (value.ToString().Length % 3 > 0)
        {
            tripleGroupsCount++;
        }

        int howManyNumbersDisplay = value.ToString().Length % 3 == 0 ? 3 : value.ToString().Length % 3;

        CurrencySize size = (CurrencySize)(tripleGroupsCount == 0 ? 1 : tripleGroupsCount);
        if (size >= CurrencySize.K)
        {
            return string.Format("{0}.{1} {2}", value.ToString().Substring(0, howManyNumbersDisplay), value.ToString().Substring(howManyNumbersDisplay, 2), GetEnumDescription(size));
        }
        else
        {
            return string.Format("{0} {1}", value.ToString().Substring(0, howManyNumbersDisplay), GetEnumDescription(size));
        }
    }



    /// <summary>
    /// Metoda pobiera opis wielkości liczy
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetEnumDescription(CurrencySize value)
    {
        object[] attributes = typeof(CurrencySize).
            GetMember(value.ToString())
            .FirstOrDefault()
            .GetCustomAttributes(typeof(DescriptionAttribute),
            false);

        return attributes.Any() ? ((DescriptionAttribute)attributes[0]).Description : value.ToString();
    }

    /// <summary>
    /// Metoda zwraca grupę (pusty gameobject) zawierającą budynki
    /// </summary>
    /// <returns></returns>
    public static GameObject GetBuildingsGroup()
    {
        return GameObject.FindGameObjectWithTag(CONSTS.BuildingsGroupTag);
    }

	/// <summary>
	/// Metoda zwraca obiekt GameStats
	/// </summary>
	/// <returns></returns>
	public static GameStats GetGameStats()
	{
		GameStats result = null;

		GameObject gameStatsObject = GameObject.FindGameObjectWithTag(CONSTS.GameStatsTag);
		if (gameStatsObject != null)
		{
			result = gameStatsObject.GetComponent<GameStats>();
		}

		return result;
	}
}
