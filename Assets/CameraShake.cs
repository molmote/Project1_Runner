using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	[SerializeField] Camera targetcamera;

	float time;
	float intensity;
	float maxTime;
	Vector3 originalPosition;
	EntityController target;

	public void Shake(float _time, float _intensity, EntityController _target)
	{
		maxTime = time = _time;
		intensity = _intensity;

		target = _target;

		originalPosition = targetcamera.transform.position;
	}

	public void Update()
	{
		if (time > 0)
		{
			time -= Time.deltaTime;

			targetcamera.transform.position =
				new Vector3(
					originalPosition.x + intensity,
					originalPosition.y + intensity,
					originalPosition.z);

			intensity = -(intensity * time/maxTime);

			if (target != null)
				target.FlipColor(intensity>0);
		}
		else if (target != null)
		{
			target.FlipColor(false);
			target = null;
		}
	}
}
