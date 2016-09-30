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
        transform.position = new Vector3(transform.position.x - left + right, 0.5f, transform.position.z + up - down);
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0f, /*transform.localRotation.y > 0 ? -90 :*/ 90, 0f));
        SnapToGrid(transform.position);
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

            Vector3 pozycjaTrafienia = new Vector3(hitTransform.position.x, 0.5f, hitTransform.position.z);
            Vector3 rotation = transform.localRotation.eulerAngles;
            BoxCollider collider = gameObject.GetComponent<BoxCollider>();
            
            // Jeżeli jest ułożenie naturalne to rozmiary są odwrócone
            float rozmiarWx = rotation.y == 0 || rotation.y == -180 || rotation.y == 180 ? collider.size.z : collider.size.x;
            float rozmiarWz = rotation.y == 0 || rotation.y == -180 || rotation.y == 180 ? collider.size.x : collider.size.z;

            // Liczenie przesunięcia na podstawie zmniejszonego rozmiaru collidera ( żeby nie dotykał krawędzi innych budynków )
            float przesuniecieColliderX = (Mathf.Ceil(rozmiarWx) - rozmiarWx) / 2f;
            float przesuniecieColliderZ = (Mathf.Ceil(rozmiarWz) - rozmiarWz) / 2f;

            Vector3 przesuniecieSkala;
            if (rozmiarWx == rozmiarWz)
            {
                przesuniecieSkala = new Vector3((float)rozmiarWx / (2f * rozmiarWx), 0f, (float)-rozmiarWz / (2f* rozmiarWz));
            }
            else
            {
                przesuniecieSkala = new Vector3((float)rozmiarWx / 2f, 0f, (float)-rozmiarWz / 2f);
            }

            Vector3 przesunieciePodstawa = new Vector3((float) -hitTransform.localScale.x / 2f, 0f, (float) hitTransform.localScale.z / 2f);
            Vector3 przesuniecieCollider = new Vector3(przesuniecieColliderX, 0f, -przesuniecieColliderZ);

            // Pozycja elementu snapuje sie do grida
            transform.position = pozycjaTrafienia - przesuniecieSkala - przesuniecieCollider;
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
                //gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(Color.red.r, Color.red.g, Color.red.b, 0.50f));
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
				//gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(Color.green.r, Color.green.g, Color.green.b, 0.50f));
			}
        }
    }
}
