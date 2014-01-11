using UnityEngine;
using System.Collections;

public class InvaderSpawner : MonoBehaviour
{
	public float radius = 2f;
	public float baseTime = 1f;
	private float countdown = 1f;
	
	public float difficultyCurve = 0.1f;

	public GameObject[] prefabs;

	void ResetClock()
	{
		countdown = baseTime / Mathf.Pow((float)(GameStateController.Hits + 1), difficultyCurve);
	}
	
	void Update ()
	{
		if(GameStateController.IsSessionInProgress && !GameStateController.IsPaused)
		{
			countdown -= Time.deltaTime;

			if(countdown < 0f && prefabs.Length > 0)
			{
				GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

				float radians = (Random.value * Mathf.PI * 2f);
				Vector3 offset = new Vector3 {
					x = Mathf.Sin (radians) * radius,
					z = Mathf.Cos (radians) * radius
				};

				GameObject.Instantiate(prefab, transform.position + offset, prefab.transform.rotation);

				ResetClock();
			}
		}
	}
}
