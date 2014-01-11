using UnityEngine;
using System.Collections;

public class InvaderController : MonoBehaviour
{
	private new Transform transform;

	public int identity = 0;

	public float unitsPerSecond = 1f;
	public Vector3 targetPosition = Vector3.zero;

	public float sizeFactor = 2f;

	private bool live = true;

	void Start ()
	{
		transform = GetComponent<Transform>();
	}

	void Update ()
	{
		if(GameStateController.IsSessionInProgress && !GameStateController.IsPaused)
		{
			Vector3 step = (targetPosition - transform.position).normalized * Time.deltaTime * unitsPerSecond;
			transform.position = transform.position + step;
		}
		else if(!GameStateController.IsSessionInProgress)
		{
			GameObject.Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(live)
		{
			SpinnerSlice slice = other.GetComponent<SpinnerSlice>();
			bool hit = slice != null && slice.identity == identity;

			if(hit)
			{
				float size = transform.localScale.magnitude * sizeFactor;
				float deathTime = size / unitsPerSecond;
				GameObject.Destroy(gameObject, deathTime);
				GameStateController.Reward();
			}
			else
			{
				GameObject.Destroy(gameObject, 3f);
				unitsPerSecond = -unitsPerSecond;
				GameStateController.Penalize();
			}

			live = false;
		}
	}
}
