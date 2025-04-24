using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DinamicTowerSetting : MonoBehaviour
{
    public SetTowerBaseInput dynamicPanel;

    public int towerId;
    public int targetType;
    public GameObject defaultBase;

    public bool spawnTower = false;
    public bool levelUp2 = false;
    public bool levelUp3 = false;

    bool isFading = false;

    AudioManager audioManager;

	private void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}
	public void Clicked()
    {
        dynamicPanel.clickedButton = transform.parent.gameObject;
		audioManager.PlaySFX(2, 0.2f);
	}

    private void Update()
    {
        if (spawnTower && !isFading) 
        {
            isFading = true;
            StartCoroutine(FadeOutSprite(defaultBase, 2f));
        }
    }

    public IEnumerator FadeOutSprite(GameObject obj, float duration)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        Color originalColor = sr.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
