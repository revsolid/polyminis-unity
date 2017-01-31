using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class InstinctsTunner : MonoBehaviour
{
    public Slider Hoarding;
    public Slider Herding;
    public Slider Predatory;
    public Slider Nomadic;
    
    public int MinLevel = 2;
    public int MaxLevel = 8;
    
    private Dictionary<Instinct, int> Levels;
    private Dictionary<Instinct, Slider> SliderMap;
    
    // Use this for initialization
    public void Initialize()
    {
        SliderMap = new Dictionary<Instinct, Slider>();
        SliderMap[Instinct.HOARDING] = Hoarding;
        SliderMap[Instinct.HERDING] = Herding;
        SliderMap[Instinct.PREDATORY] = Predatory;
        SliderMap[Instinct.NOMADIC] = Nomadic;

        Levels = new Dictionary<Instinct, int>();
        foreach(Instinct i in Enum.GetValues(typeof(Instinct)))
        {
            Levels[i] = MinLevel;
            SliderMap[i].maxValue = MaxLevel;
        }
    }

    void Reset()
    {
        foreach (Instinct i in Enum.GetValues(typeof(Instinct)))
        {
            Levels[i] = MinLevel;
        }
    }

    public void UpdateView(SpeciesDesignerModel model)
    {
        Reset();

        foreach (SpliceModel sm in model.SelectedSplices)
        {
            Levels[sm.EInstinct] += 1;
        }

        UpdateLevel(Instinct.HOARDING);
        UpdateLevel(Instinct.NOMADIC);
        UpdateLevel(Instinct.PREDATORY);
        UpdateLevel(Instinct.HERDING);
    }

    

    public void AddSplice(Instinct instinct)
    {
        Levels[instinct] += 1;
    }
    
    public void RemoveSplice(Instinct instinct)
    {
        Levels[instinct] -= 1;
    }
    
    public void OnUp(Instinct i)
    {}

    // called every frame for each level
    void UpdateLevel(Instinct i)
    {
        SliderMap[i].value = Levels[i];
    }

}
