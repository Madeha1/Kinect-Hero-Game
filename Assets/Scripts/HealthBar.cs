using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Slider Slidebar;
	public Gradient GradientColor;
	public Image Fill;

	public void SetMaxHealth(int health)
	{
		Slidebar.maxValue = health;
		Slidebar.value = health;

		Fill.color = GradientColor.Evaluate(1f);
	}

	public void SetHealth(int health)
	{
		Slidebar.value = health;

		Fill.color = GradientColor.Evaluate(Slidebar.normalizedValue);
	}

}