using UnityEngine;
using UnityEngine.UI;

public class UIPlanetRenderer : MonoBehaviour, IPlanetRenderer
{
	protected Planet Model;
	public Image PlanetImage;

	public virtual void RenderUpdate(Planet model)
	{
		if(Model == null)
		{
			Model = model;
			PlanetImage.color = GetColor();
		}
	}

	protected Color GetColor()
	{
		float average = Model.Temperature.Average ();

		// TODO: not hardcode the colors here
		Color HotColor = new Color (0.82f, 0.475f, 0.29f);
		Color TempColor = new Color (0.914f, 0.294f, 0.353f);
		Color ColdColor = new Color (0.53f, 0.6f, 0.9f);

		if (average > 0.5) {
			// TODO: fix the logic to recalculate average to a value between 0 and 1
			return Color.Lerp (TempColor, HotColor, average);
		}
		return Color.Lerp(ColdColor, TempColor, average);
	}
}
