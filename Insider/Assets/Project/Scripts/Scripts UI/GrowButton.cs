using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrowButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Vector3 normalScale = new Vector3(1, 1, 1);
	public Vector3 newScale = new Vector3(1.2f, 1.2f, 1.2f);
	public float transitionDuration = 0.2f;


	private Vector3 targetScale;
	private Vector3 currentVelocity = Vector3.zero;


	void Start()
	{
		targetScale = normalScale;
		transform.localScale = normalScale;
	}

	void Update()
	{
		transform.localScale = Vector3.SmoothDamp(transform.localScale, targetScale, ref currentVelocity, transitionDuration);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		targetScale = newScale;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		targetScale = normalScale;
	}
}
