using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	private new Transform transform;

	public float Duration = 1f;
	public float Amplitude = 0.1f;
	public AnimationCurve AmplitudeCurve;
	public float WobbleCount = 3f;

	float shakeage = 0f;
	Vector2 direction = Vector2.zero;

	int lastObservedMisses = 0;

	void Start ()
	{
		transform = gameObject.GetComponent<Transform>();
	}
	
	void Shake ()
	{
		var invaders = FindObjectsOfType<InvaderController>();
		InvaderController nearest = invaders.Length > 0 ? invaders[0] : null;

		if(invaders.Length > 1)
		{
			foreach(var invader in invaders)
			{
				if(nearest.transform.position.sqrMagnitude > invader.transform.position.sqrMagnitude)
				{
					nearest = invader;
				}
			}
		}

		shakeage = Duration;

		if(nearest == null)
		{
			direction = Random.insideUnitCircle.normalized;
		}
		else
		{
			Vector3 v = nearest.transform.position;
			direction = new Vector2 { x = v.x, y = v.z };
			direction.Normalize();
		}
	}

	void Update ()
	{
		if(GameStateController.Misses > lastObservedMisses) Shake();

		lastObservedMisses = GameStateController.Misses;

		Vector2 v = Vector2.zero;;

		if(shakeage > 0f)
		{
			float t = (Duration - shakeage) / Duration;
			float f = Mathf.Sin(Mathf.PI * WobbleCount * t) * AmplitudeCurve.Evaluate(t) * Amplitude;
			v = direction * f;
			shakeage -= Time.deltaTime;
		}

		transform.position = new Vector3 { x = v.x, z = v.y };
	}
}
