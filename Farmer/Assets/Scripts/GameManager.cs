using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private BigInteger _currentMoney = new BigInteger("1231231");
    private BigInteger _generateMoneyCount = new BigInteger("0");

    private bool EditMode = false;


    void Start()
    {
        InvokeRepeating("CollectMoney", 0f, 1f);
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
    /// Metoda włącza tryb edycji
    /// </summary>
    public void TurnEditModeOn()
    {
        EditMode = true;
        Helper.GetGUIManager().ToggleEditPanel(true);
    }
}
