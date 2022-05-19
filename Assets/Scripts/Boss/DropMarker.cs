using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMarker : MonoBehaviour
{
	[SerializeField] private float m_RotateSpeed = 180.0f;

	private void Update()
	{
		SetScale(0.5f * ((Mathf.Sin(Time.time * 10.0f) + 1) / 2) + 1);
		transform.Rotate(Vector3.up * m_RotateSpeed * Time.deltaTime);
	}

	private void SetScale(float _scale)
	{
		Vector3 scale = transform.localScale;
		scale.x = _scale;
		scale.z = _scale;
		transform.localScale = scale;
	}
}
