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

    private SpliceDnaHelixRenderer.Clicked RendererClickedHandler;

    // Use this for initialization
    void Start ()
    {
        RendererClickedHandler = (renderer) => OnSpliceRendererClicked(renderer);
        SpliceDnaHelixRenderer.OnClickEvent += RendererClickedHandler;
    }

    void OnDestroy()
    {
        SpliceDnaHelixRenderer.OnClickEvent -= RendererClickedHandler;
    }

    // Update is called once per frame
    void Update ()
    {
    
    }

    public void Initialize()
    {
        Reset();
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

    void UpdateLayoutGroupView(SpeciesDesignerModel model, LayoutGroup group, TraitSize size)
    {
        for (int i = 0; i < group.transform.childCount; i++)
        {
            GameObject button = group.transform.GetChild(i).gameObject;
            SpliceModel alreadyIn = button.GetComponent<SpliceDnaHelixRenderer>().Model;
            // check against selected list. if it's not in there anymore then kick it.
            bool isStillIn = false;
            foreach (KeyValuePair<string, SpliceModel> sm in model.SelectedSplices)
            {
                if (sm.Value.InternalName == alreadyIn.InternalName)
                {
                    isStillIn = true;
                }
            }
            if (!isStillIn)
            {
                Destroy(button);
            }
        }

        // then check the selected list to see if any new ones need to be instantiated
        foreach (KeyValuePair<string, SpliceModel> sm in model.SelectedSplices)
        {
            bool found = false;
            for (int i = 0; i < group.transform.childCount; i++)
            {
                GameObject button = group.transform.GetChild(i).gameObject;
                SpliceModel alreadyIn = button.GetComponent<SpliceDnaHelixRenderer>().Model;

                if (alreadyIn.InternalName == sm.Value.InternalName)
                {
                    found = true;
                }
            }

            if (!found && sm.Value.TraitSize == size)
            {
                AddSplice(sm.Value);
            }
        }
    }


    public void UpdateView(SpeciesDesignerModel model)
    {
        UpdateLayoutGroupView(model, SmallSplices, TraitSize.SMALL);
        UpdateLayoutGroupView(model, MedSplices, TraitSize.MEDIUM);
        UpdateLayoutGroupView(model, LargeSplices, TraitSize.LARGE);
    }

    public void AddSplice(SpliceModel splice)
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
