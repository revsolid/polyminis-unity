using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpeciesDesignUI : MonoBehaviour
{
	public VerticalLayoutGroup SmallSplices;
	public VerticalLayoutGroup MedSplices;
	public VerticalLayoutGroup LargeSplices;
	public SpliceButton SpliceButtonRendererPrototype;

	IList<SpliceModel> CurrentSelection;

	// Use this for initialization
	void Start ()
	{

		SpliceButton.OnClickEvent += (button) => OnSpliceButtonClicked(button);
		foreach(KeyValuePair<string, SpliceModel> entry in Almanac.Instance.AvailableSplices)
		{
    		SpliceButton sbutton = GameObject.Instantiate(SpliceButtonRendererPrototype);
			sbutton.Model = entry.Value;

			switch (entry.Value.TraitSize)
			{
				case TraitSize.SMALL: 
    				sbutton.transform.SetParent(SmallSplices.transform);
					break;
				case TraitSize.MEDIUM: 
    				sbutton.transform.SetParent(MedSplices.transform);
					break;
				case TraitSize.LARGE: 
    				sbutton.transform.SetParent(LargeSplices.transform);
					break;
			}
    		sbutton.transform.localPosition = Vector3.zero;
    		sbutton.transform.localScale = Vector3.one;
    		sbutton.transform.SetAsFirstSibling();
			CurrentSelection = new List<SpliceModel>();
		}
	}
	// Update is called once per frame
	void Update ()
	{
	
	}

	bool ValidateSelection(SpliceModel model)
	{
		// 4 Small, 2 Med, 1 Large
		var small = CurrentSelection.Where( x => x.TraitSize == TraitSize.SMALL).Count();
		var med = CurrentSelection.Where( x => x.TraitSize == TraitSize.MEDIUM).Count();
		var large = CurrentSelection.Where( x => x.TraitSize == TraitSize.LARGE).Count(); 

		if (model.TraitSize == TraitSize.SMALL)
			small++;
		if (model.TraitSize == TraitSize.MEDIUM)
			med++;
		if (model.TraitSize == TraitSize.LARGE)
			large++;

		return small <= 3 && med <= 2 && large <=1;

	}

	void OnSpliceButtonClicked(SpliceButton button)
	{
		Debug.Log(button.Model.Name);

		if (ValidateSelection(button.Model))
		{
			CurrentSelection.Add(button.Model);
			Destroy(button.gameObject);
		}
	}
}
