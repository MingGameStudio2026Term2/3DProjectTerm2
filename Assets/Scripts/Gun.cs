using UnityEngine;

public class Gun : MonoBehaviour
{
	// How hard the object gets pushed — adjust in Inspector
	public float hitForce = 80f;

	// Maximum distance the shot can reach
	public float hitRange = 15f;

	public ParticleSystem hitEffect;

	void Update()
	{
		// Left mouse click = fire
		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}
	}

	void Shoot()
	{
		// Build a ray starting from the camera, pointing through the center of the screen
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
		RaycastHit hit;

		// Cast the ray — if it hits something within range, 'hit' holds the result
		if (Physics.Raycast(ray, out hit, hitRange))
		{
			if (hitEffect != null)
			{
				ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(effect.gameObject, effect.main.duration);
			}

			// Try to get a Rigidbody from whatever we hit
			Rigidbody rb = hit.rigidbody;

			// Only push it if it has a Rigidbody (so static walls are ignored)
			if (rb != null)
			{
				rb.AddForce(ray.direction * hitForce, ForceMode.Impulse);
			}
		}
	}
}
