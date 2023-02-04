using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFader : MonoBehaviour
{
	SpriteRenderer sr;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	private void FixedUpdate()
	{
		Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouseWorld.z = 0;
		float d = Vector3.Distance(mouseWorld, transform.position);
		float a = d / 30;
		a = Mathf.Clamp(a, 0.1f, 1);
		sr.color = new Color(1, 1, 1, a);
	}
}
