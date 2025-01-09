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
	[SerializeField] private Image fillImage;

	public void UpdateHealthBar(float currentValue, float maxValue)
	{
		slider.value = currentValue / maxValue;
		if (currentValue <= (maxValue / 4))
		{
			fillImage.color = Color.red;
		}
		else if (currentValue <= (maxValue / 2))
		{
			fillImage.color = Color.yellow;
		}
		else
		{
			fillImage.color = Color.green;
		}
	}
	void Update()
    {
		transform.rotation = Quaternion.identity;
		transform.position = target.position + offset;
	}
}
