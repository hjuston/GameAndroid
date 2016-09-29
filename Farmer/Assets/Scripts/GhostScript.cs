using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostScript : MonoBehaviour
{
    public bool isGhost = false;
    public bool canPlace = true;

	List<Collider> collisions = new List<Collider>();

    void Start()
    {
        if(isGhost)
            CenterOfScreen();
    }

    // Poruszanie 
    public void MoveGhost(float up, float down, float left, float right)
    {
        transform.position = new Vector3(transform.position.x - left + right, 1f, transform.position.z + up - down);
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0f, transform.localRotation.y > 0 ? -90 : 90, 0f));
    }

    private void CenterOfScreen()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if(Physics.Raycast(ray, out hitInfo))
        {
            SnapToGrid(hitInfo.point);
        }
    }

    private void SnapToGrid(Vector3 position)
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(Camera.main.transform.position, position - Camera.main.transform.position);

        var layerMask = 1 << 8;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            Transform hitTransform = hitInfo.collider.gameObject.transform;

            Vector3 pozycjaTrafienia = new Vector3(hitTransform.position.x, 1f, hitTransform.position.z);
            Vector3 przesuniecieSkala = new Vector3(-transform.localScale.x / 2, 0f, -transform.localScale.z / 2);
            Vector3 przesunieciePodstawa = new Vector3(-hitTransform.localScale.x / 2, 0f, hitTransform.localScale.z / 2);

            // Pozycja elementu snapuje sie do grida
            transform.position = pozycjaTrafienia + przesuniecieSkala + przesunieciePodstawa;
        }
    }

    /// <summary>
    /// Jeżeli budynek wejdzie w kolizcję z jakimś innym budynkiem lub nonplaceable
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (isGhost)
        {
            if ((other.gameObject.tag == CONSTS.BuildingTag || other.gameObject.tag == CONSTS.NonPlaceableTag))
            {
				collisions.Add(other);

                canPlace = false;
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(Color.red.r, Color.red.g, Color.red.b, 0.50f));
            }
        }
    }

    /// <summary>
    /// Jeżeli budynek wyjdzie z kolizji
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (isGhost && (other.gameObject.tag == CONSTS.BuildingTag || other.gameObject.tag == CONSTS.NonPlaceableTag))
        {
			if (collisions.Contains(other)) collisions.Remove(other);

			if (collisions.Count == 0)
			{
				canPlace = true;
				gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(Color.green.r, Color.green.g, Color.green.b, 0.50f));
			}
        }
    }
}
