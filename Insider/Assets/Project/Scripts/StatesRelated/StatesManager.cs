using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StatesManager : MonoBehaviour
{
	public UnityEngine.UI.Slider slider;

	void Start()
	{
		float centerValue = (slider.minValue + slider.maxValue) / 2f;
		slider.value = centerValue;
		StartCoroutine(AutoIncrementPosition());
	}

	private void Update()
	{
		if(slider.value >= 2.98f) 
		{
			//Saludable
		}
		else if(slider.value <= 6.03f)
		{
			//Enfebrit
		}
	}


	public void IncrementPosition()
	{
		if (slider.value >= slider.minValue && slider.value <= slider.maxValue)
			slider.value += 1f;
	}

	public void DecrementPosition()
	{
		if (slider.value >= slider.minValue && slider.value <= slider.maxValue)
			slider.value -= 0.005f;
	}

	private IEnumerator AutoIncrementPosition()
	{
		while (true)
		{
			DecrementPosition();
			yield return new WaitForSeconds(0.2f);
		}
	}
}
