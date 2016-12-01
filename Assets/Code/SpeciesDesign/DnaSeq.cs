using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DnaSeq : MonoBehaviour
{
	public DnaSeqRenderer DnaSeqRendererPrototype;
	public LayoutGroup Layout;
	IDictionary<int, DnaSeqRenderer> Renderers = new Dictionary<int, DnaSeqRenderer>();

	// Use this for initialization
	void Start ()
	{
		foreach(KeyValuePair<int, TraitModel> entry in Almanac.Instance.TraitData)
		{
			AddTraitData(entry.Value);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	}


	void AddTraitData(TraitModel model)
	{
		DnaSeqRenderer dnaSeqRenderer = GameObject.Instantiate<DnaSeqRenderer>(DnaSeqRendererPrototype);
		dnaSeqRenderer.Model = model;
		dnaSeqRenderer.Active = false;
		dnaSeqRenderer.transform.SetParent(Layout.transform);
		dnaSeqRenderer.transform.SetAsLastSibling();

		Renderers[model.TID] = dnaSeqRenderer;
	}

	public void ActivateSelection(SpeciesModel model)
	{
		foreach(KeyValuePair<int, DnaSeqRenderer> entry in Renderers)
		{
			entry.Value.Active = false;
		}

		foreach (SpliceModel sm in model.Splices)
		{
			for(int i =0; i < sm.Traits.Length; ++i)
			{
				int j = sm.Traits[i];
				DnaSeqRenderer r = null;
				Renderers.TryGetValue(j, out r);
				if (r != null)
				{
					r.Active = true;
				}
			}
		}
	}
}
