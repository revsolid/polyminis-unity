using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DnaHelix : MonoBehaviour
{
    public LayoutGroup SmallSplices;
    public LayoutGroup MedSplices;
    public LayoutGroup LargeSplices;
    public SpliceDnaHelixRenderer SmallSpliceDnaHelixRendererPrototype;
    public SpliceDnaHelixRenderer MedSpliceDnaHelixRendererPrototype;
    public SpliceDnaHelixRenderer LargeSpliceDnaHelixRendererPrototype;
	
	public delegate void SpliceRemoved(SpliceModel model);
    public static event SpliceRemoved OnSpliceRemovedEvent;

    // Use this for initialization
    void Start ()
    {
        SpliceDnaHelixRenderer.OnClickEvent += (renderer) => OnSpliceRendererClicked(renderer);
    }
    
    // Update is called once per frame
    void Update ()
    {
    
    }
    
    public void Reset()
    {
        ResetLayoutGroup(SmallSplices);
        ResetLayoutGroup(MedSplices);
        ResetLayoutGroup(LargeSplices);
    }
    void ResetLayoutGroup(LayoutGroup lg)
    {
        var children = new List<GameObject>();
        foreach (Transform child in lg.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }
    
    public void AddSelectedSplice(SpliceModel splice)
    {
		SpliceDnaHelixRenderer renderer;
        switch (splice.TraitSize)
        {
            case TraitSize.SMALL: 
                renderer = GameObject.Instantiate(SmallSpliceDnaHelixRendererPrototype);
                renderer.transform.SetParent(SmallSplices.transform);
                break;
            case TraitSize.MEDIUM: 
                renderer = GameObject.Instantiate(MedSpliceDnaHelixRendererPrototype);
                renderer.transform.SetParent(MedSplices.transform);
                break;
            case TraitSize.LARGE: 
			default: // Just here so the compiler shuts up
                renderer = GameObject.Instantiate(LargeSpliceDnaHelixRendererPrototype);
                renderer.transform.SetParent(LargeSplices.transform);
                break;
        }    
        renderer.Model = splice;
        renderer.transform.localPosition = Vector3.zero;
        renderer.transform.localScale = Vector3.one;
        renderer.transform.SetAsLastSibling();
    }
    
    void OnSpliceRendererClicked(SpliceDnaHelixRenderer renderer)
    {
		SpliceModel model = renderer.Model;
		OnSpliceRemovedEvent(model);
        Destroy(renderer.gameObject);
    }
}
