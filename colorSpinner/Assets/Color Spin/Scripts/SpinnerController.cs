using UnityEngine;
using System.Collections;

public class SpinnerController : MonoBehaviour
{
	private new Transform transform;

	private float inputAngleOnTouch;
	private float outputAngleOnTouch;

	void Start()
	{
		transform = gameObject.GetComponent<Transform>();
	}
	
	float inputAngleOnPriorFrame;

	void Update()
	{
		Vector3 spinnerPosition = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 delta = spinnerPosition - Input.mousePosition;

		float inputAngle = Mathf.Atan2(delta.y, delta.x);

		if(!Input.GetMouseButtonDown(0) && Input.GetMouseButton(0))
		{
			float angleChange = (inputAngleOnPriorFrame - inputAngle) * Mathf.Rad2Deg;
			transform.localRotation = Quaternion.Euler(0f, transform.localRotation.eulerAngles.y + angleChange, 0f);
		}

		inputAngleOnPriorFrame = inputAngle;
	}
}
