using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostScript : MonoBehaviour
{
    public bool isGhost = false;
    public bool canPlace = true;

	List<Collider> collisions = new List<Collider>();

    void FixedUpdate()
    {
        if (isGhost) MakeGhostFollowMouse();
    }

    void MakeGhostFollowMouse()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        var layerMask = 1 << 8;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            Transform hitTransform = hitInfo.collider.gameObject.transform;
            float x = transform.localScale.x / 2;
            float z = transform.localScale.z / 2;
            transform.position = new Vector3(hitTransform.position.x, 1f, hitTransform.position.z) + new Vector3(x, 0f, -z) - new Vector3(hitTransform.localScale.x /2 - 0.05f, 0f, -(hitTransform.localScale.z/2 - 0.05f));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isGhost)
        {
            if ((other.gameObject.tag == "Player" || other.gameObject.tag == "nonselectable"))
            {
				collisions.Add(other);

                canPlace = false;
                Helper.GetGameManager().CanPlace = false;
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(Color.red.r, Color.red.g, Color.red.b, 0.10f));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isGhost && (other.gameObject.tag == "Player" || other.gameObject.tag == "nonselectable"))
        {
			if (collisions.Contains(other)) collisions.Remove(other);

			if (collisions.Count == 0)
			{
				canPlace = true;
				Helper.GetGameManager().CanPlace = true;
				gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(Color.green.r, Color.green.g, Color.green.b, 0.10f));
			}
        }
    }
}
