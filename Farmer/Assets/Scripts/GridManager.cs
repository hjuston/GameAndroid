using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    // Tymczasowy prefab który należy utworzyć jako ghost object
    public GameObject Cube;

    // Ghost object i jego materiał
    private GameObject _ghostObject;
    private Material _ghostObjectOriginalMaterial;
    private Color _ghostObjectOriginalColor;
    private GameObject GhostObject {
        get { return _ghostObject; }
        set { _ghostObject = value; /*_ghostObjectOriginalColor = (value == null) ? Color.white : value.GetComponent<Renderer>().material.color;*/ } }

    // Przycisk potwierdzenia do postawienia budynku
    public GameObject GhostFollowingButton;

    /// <summary>
    /// Metoda tworzy obiekt Ghost
    /// </summary>
    public void SpawnGhostObject(GameObject prefab)
    {
        if (GhostObject == null)
        {
            GhostObject = GameObject.Instantiate<GameObject>(prefab);

            // Activate following button to move object
            GhostFollowingButton.SetActive(true);
            GhostFollowingButtonScript ghostFollowScript = GhostFollowingButton.GetComponent<GhostFollowingButtonScript>();
            ghostFollowScript.SetGhostObject(GhostObject);

            GhostScript ghostScript = GhostObject.GetComponent<GhostScript>();
            ghostScript.isGhost = true;
        }
    }

    /// <summary>
    /// Zamienia obiekt Ghost na budynek
    /// </summary>
    public void DropObject()
    {
        GhostScript ghost = GhostObject.GetComponent<GhostScript>();
        if (ghost.canPlace)
        {
            // Place object
            ghost.isGhost = false;
            //ghost.GetComponent<Renderer>().material = _ghostObjectOriginalMaterial;
            //ghost.GetComponent<Renderer>().material.SetColor("_Color", _ghostObjectOriginalColor);

            // Hide following button to place button
            GhostFollowingButtonScript ghostFollowScript = GhostFollowingButton.GetComponent<GhostFollowingButtonScript>();
            ghostFollowScript.UnsetGhostObject();
            GhostFollowingButton.SetActive(false);

            // Ustawianie budynku jako child grupy budynków
            GhostObject.transform.SetParent(Helper.GetBuildingsGroup().transform);

            GhostObject = null;
            // Informowanie budowli, że została wybudowana na prawdę
            //Building buildingScript = GhostObject.GetComponent<Building>();
            //if(buildingScript != null)
            //{
            //    buildingScript.IsPlacedForReal = true;
            //}
        }
    }

    /// <summary>
    /// Metoda powoduje anulowanie stawiania budynku
    /// </summary>
    public void CancelGhost()
    {
        // Hide following button to place button
        GhostFollowingButtonScript ghostFollowScript = GhostFollowingButton.GetComponent<GhostFollowingButtonScript>();
        ghostFollowScript.UnsetGhostObject();
        GhostFollowingButton.SetActive(false);

        GameObject.Destroy(GhostObject);
        GhostObject = null;
    }

    public void RotateGhost()
    {
        if (GhostObject != null)
        {
            GhostScript ghostScript = GhostObject.GetComponent<GhostScript>();
            ghostScript.Rotate();
        }
    }

    // Poruszanie obiektem Ghost
    private void GhostMove(float up, float down, float left, float right)
    {
        if (GhostObject != null)
        {
            GhostScript ghostScript = GhostObject.GetComponent<GhostScript>();
            ghostScript.MoveGhost(up, down, left, right);
        }
    }

    public void GhostMoveUp() { GhostMove(1, 0, 0, 0); }
    public void GhostMoveDown() { GhostMove(0, 1, 0, 0); }
    public void GhostMoveLeft() { GhostMove(0, 0, 1, 0); }
    public void GhostMoveRight() { GhostMove(0, 0, 0, 1); }
}
