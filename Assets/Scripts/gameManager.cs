using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class gameManager : MonoBehaviour
{
	public TMP_Text PointsCounter;
	public GameObject StartUI;
	public GameObject LoseScreen;
	public Snake Snake;

	public AudioSource Bite;
	public AudioSource Music;
	public AudioSource Die;
	public AudioSource Move;

	private int points = 0;

	private bool startUIActive = true;
	private void Start()
	{
		UpdateCounter();
	}
	private void Update()
	{
		if(Snake.snakeState != Snake.State.start && startUIActive)
		{
			StartCoroutine("Fade",StartUI);
			startUIActive = false;
		}
		else if(Snake.snakeState == Snake.State.dead && Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine("Fade", LoseScreen);
			StartUI.SetActive(true);
			startUIActive = true;
			points = 0;
			UpdateCounter();
			Snake.Restart();
		}
		
			
		
	}
	public void AddPoint()
	{
		points++;
		UpdateCounter();
		StartCoroutine("Bump");
	}
	public void ShowGameover()
	{
		LoseScreen.SetActive(true);
	}
	public void UpdateCounter()
	{
		string newCounterText = "Score: " + points.ToString();
		PointsCounter.text = newCounterText;
	}
	private IEnumerator Fade(GameObject TMPParent)
	{
		RectTransform RectTrans = TMPParent.GetComponent<RectTransform>();
		for (int i = 1; i <= 100; i++)
		{
			RectTrans.localScale = new Vector3((100f - i) / 100f, 1f, 1f);
			yield return null;
		}
		TMPParent.SetActive(false);
		RectTrans.localScale = new Vector3(1f, 1f, 1f);
	}
	private IEnumerator Bump()
	{
		for (int i = 0; i < 60; i++)
		{
			PointsCounter.fontSize += 0.17f;
			yield return null;
		}
		for (int i = 0; i < 60; i++)
		{
			PointsCounter.fontSize -= 0.17f;
			yield return null;
		}
	}
}
