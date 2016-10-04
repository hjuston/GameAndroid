using UnityEngine;

public class GhostFollowingButtonScript : MonoBehaviour
{
    private GameObject _ghostObject;
    public GameObject SellButtonPanel;

    void Update()
    {
        if (_ghostObject != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(_ghostObject.transform.position);
        }
    }

    public void SetGhostObject(GameObject ghost)
    {
        _ghostObject = ghost;

        Building building = ghost.GetComponent<Building>();
        if (building != null)
        {
            SellButtonPanel.gameObject.SetActive(building.IsPlacedForReal);
        }
    }

    public void UnsetGhostObject()
    {
        _ghostObject = null;
        transform.position = Vector3.zero;
    }
}
