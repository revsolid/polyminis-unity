using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class InstinctsTunner : MonoBehaviour
{
    public Slider Hoarding , MaxHoarding;
    public Slider Herding , MaxHerding;
    public Slider Predatory, MaxPredatory;
    public Slider Nomadic, MaxNomadic;
    
        
    public int MinLevel = 2;
    public int MaxLevel = 8;
    
    private Dictionary<Instinct, int> Levels;
    private Dictionary<Instinct, int> MaxLevels;
    private Dictionary<Instinct, Slider> MaxSliderMap;
    private Dictionary<Instinct, Slider> ValueSliderMap;
    private Dictionary<Instinct, Instinct> Opposites;

    // Use this for initialization
    public void Initialize()
    {
        MaxSliderMap = new Dictionary<Instinct, Slider>();
        ValueSliderMap = new Dictionary<Instinct, Slider>();
        MaxSliderMap[Instinct.HOARDING] = MaxHoarding;
        MaxSliderMap[Instinct.HERDING] = MaxHerding;
        MaxSliderMap[Instinct.PREDATORY] = MaxPredatory;
        MaxSliderMap[Instinct.NOMADIC] = MaxNomadic;

        ValueSliderMap[Instinct.HOARDING] = Hoarding;
        ValueSliderMap[Instinct.HERDING] = Herding;
        ValueSliderMap[Instinct.PREDATORY] = Predatory;
        ValueSliderMap[Instinct.NOMADIC] = Nomadic;

        Opposites = new Dictionary<Instinct, Instinct>();

        Opposites[Instinct.HOARDING] = Instinct.NOMADIC;
        Opposites[Instinct.PREDATORY] = Instinct.HERDING;
        Opposites[Instinct.NOMADIC] = Instinct.HOARDING;
        Opposites[Instinct.HERDING] = Instinct.PREDATORY;

        Levels = new Dictionary<Instinct, int>();
        MaxLevels = new Dictionary<Instinct, int>();
        foreach (Instinct i in Enum.GetValues(typeof(Instinct)))
        {
            Levels[i] = 0;
            MaxSliderMap[i].maxValue = MaxLevel;
            ValueSliderMap[i].maxValue = MaxLevel;
        }
    }

    void Reset()
    {
        foreach (Instinct i in Enum.GetValues(typeof(Instinct)))
        {
            MaxLevels[i] = MinLevel;
        }
    }

    public void UpdateView(SpeciesDesignerModel model)
    {
        Reset();

        foreach (SpliceModel sm in model.SelectedSplices)
        {
            MaxLevels[sm.EInstinct] += 1;
        }

        UpdateLevel(Instinct.HOARDING);
        UpdateLevel(Instinct.NOMADIC);
        UpdateLevel(Instinct.PREDATORY);
        UpdateLevel(Instinct.HERDING);
    }

    

    public void AddSplice(Instinct instinct, int size)
    {
        MaxLevels[instinct] += size;
    }
    
    public void RemoveSplice(Instinct instinct, int size)
    {
        MaxLevels[instinct] -= size;
    }
    
    public void OnUp(Instinct i)
    {}

    // called every frame for each level
    void UpdateLevel(Instinct i)
    {
        MaxSliderMap[i].value = MaxLevels[i];
        ValueSliderMap[i].value = Levels[i];
    }

    private bool CanTune(Instinct i, bool up)
    {
        if (up) return (Levels[i] < MaxLevels[i] && MaxLevels[Opposites[i]] > Levels[Opposites[i]]);
        else return (Levels[i] > 0);
    }

    public void ChangeTuning(Instinct i, bool up)
    {
        Debug.Log("Tuning: " + i + " " + (up ? "up" : "down"));
        if (!CanTune(i, up)) return;
        if (up)
        {
            Levels[i]++;
            MaxLevels[Opposites[i]]--;
        }else
        {
            Levels[i]--;
            MaxLevels[Opposites[i]]++;
        }

        Debug.Log("Instinct: " + i + " Max Value: " + MaxLevels[i] + " Current Level: " + Levels[i]);
        UpdateLevel(Instinct.HOARDING);
        UpdateLevel(Instinct.NOMADIC);
        UpdateLevel(Instinct.PREDATORY);
        UpdateLevel(Instinct.HERDING);
    }

    public void Tune(int i)
    {
        switch (i)
        {
            case 0:
                ChangeTuning(Instinct.HERDING, true);
                break;
            case 1:
                ChangeTuning(Instinct.HERDING, false);
                break;
            case 2:
                ChangeTuning(Instinct.HOARDING, true);
                break;
            case 3:
                ChangeTuning(Instinct.HOARDING, false);
                break;
            case 4:
                ChangeTuning(Instinct.PREDATORY, true);
                break;
            case 5:
                ChangeTuning(Instinct.PREDATORY, false);
                break;
            case 6:
                ChangeTuning(Instinct.NOMADIC, true);
                break;
            case 7:
                ChangeTuning(Instinct.NOMADIC, false);
                break;
        }
    }
}
