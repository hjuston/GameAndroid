﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private BigInteger _currentMoney = new BigInteger("123123331");
    private BigInteger _generateMoneyCount = new BigInteger("100");

    private bool EditMode = false;


    void Start()
    {
        // Wczytywanie bazy danych budynków
        BuildingsDatabase.LoadDatabase();

        InvokeRepeating("CollectMoney", 0f, 1f);
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
		Helper.GetGUIManager().SetMoneyInfo(_currentMoney);
	}


    /// <summary>
    /// Metoda zlicza przychody z budynków i dodaje je do aktualnej gotówki
    /// </summary>
    void CollectMoney()
    {
        // Zliczanie gotówki jeżeli gra nie jest w trybie edycji budowli
        if (!EditMode)
        {
            _currentMoney += _generateMoneyCount;
            Helper.GetGUIManager().SetMoneyInfo(_currentMoney);
        }

		//Helper.GetGameStats().AddExperience(10);
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
        Helper.GetGUIManager().SetMoneyInfo(_currentMoney);
    }

    /// <summary>
    /// Metoda włącza tryb edycji
    /// </summary>
    public void TurnEditModeOn()
    {
        EditMode = true;
        Helper.GetGUIManager().ToggleEditPanel(true);
    }
}
