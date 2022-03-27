using UnityEngine;

public class ParticleCollision : MonoBehaviour
{

	[HideInInspector]
	public bool isColliding = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Fire"))
			isColliding = true;
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Fire"))
			isColliding = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Fire"))
			isColliding = false;
	}

}
