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
    
    private Dictionary<Instinct, int> Levels = new Dictionary<Instinct, int>();
    private Dictionary<Instinct, int> MaxLevels = new Dictionary<Instinct, int>();
    private Dictionary<Instinct, Slider> MaxSliderMap = new Dictionary<Instinct, Slider>();
    private Dictionary<Instinct, Slider> ValueSliderMap = new Dictionary<Instinct, Slider>();
    private Dictionary<Instinct, Instinct> Opposites = new Dictionary<Instinct, Instinct>();

    // Use this for initialization
    public void Initialize()
    {
        MaxSliderMap[Instinct.HOARDING] = MaxHoarding;
        MaxSliderMap[Instinct.HERDING] = MaxHerding;
        MaxSliderMap[Instinct.PREDATORY] = MaxPredatory;
        MaxSliderMap[Instinct.NOMADIC] = MaxNomadic;

        ValueSliderMap[Instinct.HOARDING] = Hoarding;
        ValueSliderMap[Instinct.HERDING] = Herding;
        ValueSliderMap[Instinct.PREDATORY] = Predatory;
        ValueSliderMap[Instinct.NOMADIC] = Nomadic;

        Opposites[Instinct.HOARDING] = Instinct.NOMADIC;
        Opposites[Instinct.PREDATORY] = Instinct.HERDING;
        Opposites[Instinct.NOMADIC] = Instinct.HOARDING;
        Opposites[Instinct.HERDING] = Instinct.PREDATORY;

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

    public InstinctTuningModel ToModel()
    {
        InstinctTuningModel ret = new InstinctTuningModel();
        ret.HoardingLvl = Levels [Instinct.HOARDING];
        ret.HerdingLvl = Levels [Instinct.HERDING];
        ret.NomadicLvl = Levels [Instinct.NOMADIC];
        ret.PredatoryLvl = Levels [Instinct.PREDATORY];
        ret.HoardingMaxLvl = MaxLevels [Instinct.HOARDING];
        ret.HerdingMaxLvl = MaxLevels [Instinct.HERDING];
        ret.NomadicMaxLvl = MaxLevels [Instinct.NOMADIC];
        ret.PredatoryMaxLvl = MaxLevels [Instinct.PREDATORY];
        return ret;
    }

    public void LoadModel(InstinctTuningModel model)
    {
        Levels [Instinct.HOARDING] = model.HoardingLvl;
        Levels [Instinct.HERDING] = model.HerdingLvl;
        Levels [Instinct.NOMADIC] = model.NomadicLvl;
        Levels [Instinct.PREDATORY] = model.PredatoryLvl;
        MaxLevels [Instinct.HOARDING] = model.HoardingMaxLvl;
        MaxLevels [Instinct.HERDING] = model.HerdingMaxLvl;
        MaxLevels [Instinct.NOMADIC] = model.NomadicMaxLvl;
        MaxLevels [Instinct.PREDATORY] = model.PredatoryMaxLvl;
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
