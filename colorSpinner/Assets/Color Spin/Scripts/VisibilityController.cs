using UnityEngine;
using System.Collections;

public class VisibilityController : MonoBehaviour
{
	public float smoothTime = 0.33f;

	private new Transform transform;
	private float currentSize = 1;
	private float sizeDelta = 0f;
	private float idealSize {
		get { return GameStateController.IsSessionInProgress ? 1f : 0f; }
	}
	private Vector3 initialScale = Vector3.one;

	// Use this for initialization
	void Start () {
		transform = gameObject.GetComponent<Transform>();
		currentSize = idealSize;
		initialScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		currentSize = Mathf.SmoothDamp(currentSize, idealSize, ref sizeDelta, smoothTime);
		transform.localScale = initialScale * currentSize;
	}
}
