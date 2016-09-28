using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private BigInteger _currentMoney = new BigInteger("1231231");
    private BigInteger _generateMoneyCount = new BigInteger("0");

    private GameObject GhostObject;
    public GameObject Cube;

    public bool CanPlace;

    void Start()
    {
        // Zliczanie gotówki
        InvokeRepeating("CollectMoney", 0f, 1f);
    }
    
    void Update()
    {
        if (GhostObject != null && Input.GetMouseButtonDown(0)) DropObject();
    }

    bool canPlace = false;
    public void SpawnGhostObject()
    {
        GhostObject = GameObject.Instantiate<GameObject>(Cube);
        GhostScript ghost = GhostObject.GetComponent<GhostScript>();
        ghost.isGhost = true;

        Renderer r = GhostObject.GetComponent<Renderer>();
        r.material.SetColor("_Color", new Color(r.material.color.r, r.material.color.g, r.material.color.b, 0.10f));
    }

    void DropObject()
    {
        GhostScript ghost = GhostObject.GetComponent<GhostScript>();
        Debug.Log(ghost.canPlace);
        if (ghost.canPlace)
        {
            ghost.isGhost = false;

            GameObject temp = GameObject.Instantiate<GameObject>(Cube);
            ghost.GetComponent<Renderer>().material.SetColor("_Color", temp.GetComponent<Renderer>().material.color);
            Destroy(temp);
        }
    }

    /// <summary>
    /// Metoda zlicza przychody z budynków i dodaje je do aktualnej gotówki
    /// </summary>
    void CollectMoney()
    {
        _currentMoney += _generateMoneyCount;

        Helper.GetGUIManager().SetMoneyInfo(_currentMoney);
    }

    /// <summary>
    /// Metoda dodaje do listy aktualnych budynków budynek przesłany w parametrze.
    /// Lista używana jest do zliczania gotówki.
    /// </summary>
    /// <param name="building"></param>
    public void AddBuildingToCurrentBuildingList(Building building)
    {
        _generateMoneyCount += building.GetIncome();
        Helper.GetGUIManager().SetMoneyGenerateInfo(_generateMoneyCount);
    }


    /// <summary>
    /// Metoda usuwa budynek z listy aktualnych budynków.
    /// Lista używana jest do zliczania gotówki.
    /// </summary>
    /// <param name="building"></param>
    public void RemoveBuildingFromCurrentBuildingList(Building building)
    {
        _generateMoneyCount -= building.GetIncome();
        Helper.GetGUIManager().SetMoneyGenerateInfo(_generateMoneyCount);
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
    /// Metoda powoduje zmniejszenie aktualnej gotówki (np. w wypadku kupna budynku).
    /// </summary>
    /// <param name="money"></param>
    public void SpendMoney(BigInteger money)
    {
        this._currentMoney = _currentMoney - money;
        Helper.GetGUIManager().SetMoneyInfo(_currentMoney);
    }


    /// <summary>
    /// Metoda, która powoduje sprzedanie budynku. Wywoływane przez przycisk SellButton
    /// </summary>
    public void SellBuilding()
    {
        // Przypisanie gotówki
        BigInteger sellPrice = Helper.GetTileManager().CurrentTile.Building.GetSellPrice();

        this._currentMoney += sellPrice;
        Helper.GetGUIManager().SetMoneyInfo(this._currentMoney);

        // Usuwanie budynku z listy budynków generujących gotówkę
        RemoveBuildingFromCurrentBuildingList(Helper.GetTileManager().CurrentTile.Building);

        // Wyczyszczenie Tile
        Helper.GetTileManager().CurrentTile.SetBuilding(null);
    }


    /// <summary>
    /// Metoda, która powoduje ulepszenie budynku. Wywołane przez przycisk UpgradeButton
    /// </summary>
    public void UpgradeBuilding()
    {
        BigInteger upgradePrice = Helper.GetTileManager().CurrentTile.Building.GetCost();
        
        if (_currentMoney >= upgradePrice)
        {
            // Zakup ulepszenia i zmiana generowanego przychodu
            _currentMoney -= upgradePrice;

            RemoveBuildingFromCurrentBuildingList(Helper.GetTileManager().CurrentTile.Building);
            Helper.GetTileManager().CurrentTile.Building.Upgrade();
            AddBuildingToCurrentBuildingList(Helper.GetTileManager().CurrentTile.Building);

            // Wyświetlanie informacji o ulepszeniu w panelu
            Helper.GetGUIManager().SetBuildingInfo(Helper.GetTileManager().CurrentTile.Building);
            Helper.GetGUIManager().SetMoneyGenerateInfo(_generateMoneyCount);
            Helper.GetGUIManager().SetMoneyInfo(_currentMoney);
        }
        else
        {
            Debug.Log("Brak środków do zakupu tego ulepszenia.");
        }
    }
}
