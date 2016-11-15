using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
        renderer.transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
        renderer.transform.SetAsLastSibling();
    }
    
    void OnSpliceRendererClicked(SpliceDnaHelixRenderer renderer)
    {
		SpliceModel model = renderer.Model;
		OnSpliceRemovedEvent(model);
        Destroy(renderer.gameObject);
    }
}
