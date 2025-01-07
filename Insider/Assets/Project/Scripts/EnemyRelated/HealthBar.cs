using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] public Slider slider;
	[SerializeField] private Camera cam;
	[SerializeField] private Transform target;
	[SerializeField] private Vector3 offset;
	public Enemy enemy;

	public void UpdateHealthBar(float currentValue, float maxValue)
	{
		slider.value = currentValue / maxValue;
	}
	void Update()
    {
		transform.rotation = Quaternion.identity;
		transform.position = target.position + offset;
	}
}
