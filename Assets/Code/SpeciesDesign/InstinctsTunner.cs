using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class InstinctsTunner : MonoBehaviour
{
    public Slider Hoarding , MaxHoarding;
    public Slider Herding , MaxHerding;
    public Slider Predatory, MaxPreadatory;
    public Slider Nomadic, MaxNomadic;
    
        
    public int MinLevel = 2;
    public int MaxLevel = 8;
    
    private Dictionary<Instinct, int> Levels;
    private Dictionary<Instinct, Slider> MaxSliderMap;
    private Dictionary<Instinct, Slider> ValueSliderMap;

    // Use this for initialization
    public void Initialize()
    {
        MaxSliderMap = new Dictionary<Instinct, Slider>();
        ValueSliderMap = new Dictionary<Instinct, Slider>();
        MaxSliderMap[Instinct.HOARDING] = Hoarding;
        MaxSliderMap[Instinct.HERDING] = Herding;
        MaxSliderMap[Instinct.PREDATORY] = Predatory;
        MaxSliderMap[Instinct.NOMADIC] = Nomadic;

        ValueSliderMap[Instinct.HOARDING] = Hoarding;
        ValueSliderMap[Instinct.HERDING] = Herding;
        ValueSliderMap[Instinct.PREDATORY] = Predatory;
        ValueSliderMap[Instinct.NOMADIC] = Nomadic;

        Levels = new Dictionary<Instinct, int>();
        foreach(Instinct i in Enum.GetValues(typeof(Instinct)))
        {
            Levels[i] = MinLevel;
            MaxSliderMap[i].maxValue = MaxLevel;
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

    

    public void AddSplice(Instinct instinct, int size)
    {
        Levels[instinct] += size;
    }
    
    public void RemoveSplice(Instinct instinct, int size)
    {
        Levels[instinct] -= size;
    }
    
    public void OnUp(Instinct i)
    {}

    // called every frame for each level
    void UpdateLevel(Instinct i)
    {
        MaxSliderMap[i].value = Levels[i];
    }

    public void ChangeTuning(Instinct instinct, bool up)
    {
        if (up && ValueSliderMap[instinct].value < MaxSliderMap[instinct].value)
        {
            ValueSliderMap[instinct].value++;
        }
    }

}
