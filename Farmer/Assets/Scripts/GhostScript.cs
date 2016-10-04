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
		if (isGhost)
			CenterOfScreen();
	}

	// Poruszanie 
	public void MoveGhost(float up, float down, float left, float right)
	{
		transform.position = new Vector3(transform.position.x - left + right, 0.5f, transform.position.z + up - down);
	}

	public void Rotate()
	{
		transform.Rotate(new Vector3(0f, 90, 0f));
		SnapToGrid(transform.position);
	}

	private void CenterOfScreen()
	{
		RaycastHit hitInfo;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

		if (Physics.Raycast(ray, out hitInfo))
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

			if (rotation.y == 0 || rotation.y == 180 || rotation.y == -180)
			{
				// Jeżeli obiekt stoi pionowo
				Vector3 przesuniecieSkala = new Vector3(collider.size.x / 2, 0f, -(collider.size.z / 2));
				Vector3 przesuniecieCube = new Vector3(hitTransform.localScale.x / 2, 0f, hitTransform.localScale.z / 2);

				//// Liczenie przesunięcia na podstawie zmniejszonego rozmiaru collidera ( żeby nie dotykał krawędzi innych budynków )
				float przesuniecieColliderX = (Mathf.Ceil(collider.size.x) - collider.size.x) / 2f;
				float przesuniecieColliderZ = (Mathf.Ceil(collider.size.z) - collider.size.z) / 2f;
				Vector3 przesuniecieCollider = new Vector3(przesuniecieColliderX, 0f, -przesuniecieColliderZ);

				transform.position = pozycjaTrafienia + przesuniecieSkala - przesuniecieCube + przesuniecieCollider;
			}
			else
			{
				// Jeżeli obiekt stoi poziomo
				Vector3 przesuniecieSkala = new Vector3(collider.size.z / 2, 0f, -(collider.size.x / 2));
				Vector3 przesuniecieCube = new Vector3(hitTransform.localScale.x / 2, 0f, hitTransform.localScale.z / 2);

				//// Liczenie przesunięcia na podstawie zmniejszonego rozmiaru collidera ( żeby nie dotykał krawędzi innych budynków )
				float przesuniecieColliderX = (Mathf.Ceil(collider.size.x) - collider.size.x) / 2f;
				float przesuniecieColliderZ = (Mathf.Ceil(collider.size.z) - collider.size.z) / 2f;
				Vector3 przesuniecieCollider = new Vector3(przesuniecieColliderZ, 0f, -przesuniecieColliderX);

				transform.position = pozycjaTrafienia + przesuniecieSkala - przesuniecieCube + przesuniecieCollider;
			}
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
			}
		}
	}
}
