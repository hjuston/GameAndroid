using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GhostFollowingButtonScript : MonoBehaviour
{
    private GameObject _ghostObject;
    
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
    }

    public void UnsetGhostObject()
    {
        _ghostObject = null;
        transform.position = Vector3.zero;
    }
}
