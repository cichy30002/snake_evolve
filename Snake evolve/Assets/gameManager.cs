using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
	public TMP_Text PointsCounter;

	private int points = 0;
	private void Start()
	{
		PointsCounter.text = points.ToString();
	}
	public void AddPoint()
	{
		points++;
		UpdateCounter();
	}
	public void UpdateCounter()
	{
		PointsCounter.text = points.ToString();
	}
}
