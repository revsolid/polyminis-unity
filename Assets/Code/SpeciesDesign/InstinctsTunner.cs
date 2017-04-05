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

    // data representation of instinct levels. Value of the solid bars.
    private Dictionary<Instinct, int> Levels = new Dictionary<Instinct, int>();
    // the actual max level.
    private Dictionary<Instinct, int> MaxLevels = new Dictionary<Instinct, int>();
    // the "base" max level determined by selected splices (without use messing with arrow buttons).
    private Dictionary<Instinct, int> MaxLevelsBase = new Dictionary<Instinct, int>();
    // the "tunned" max level determined by user clicking arrow buttons, but nothing to do with splices
    private Dictionary<Instinct, int> MaxLevelsTunned = new Dictionary<Instinct, int>();

    private Dictionary<Instinct, Slider> MaxSliderMap = new Dictionary<Instinct, Slider>();
    private Dictionary<Instinct, Slider> ValueSliderMap = new Dictionary<Instinct, Slider>();
    private Dictionary<Instinct, Instinct> Opposites = new Dictionary<Instinct, Instinct>();


    void ResetValues()
    {
        Initialize();
        foreach (Instinct i in Enum.GetValues(typeof(Instinct)))
        {
            Debug.Log(i);
            Levels[i] = 0;
            MaxLevels[i] = MinLevel;
            MaxLevelsBase[i] = MinLevel;
            MaxLevelsTunned[i] = 0;
            MaxSliderMap[i].maxValue = MaxLevel;
            ValueSliderMap[i].maxValue = MaxLevel;
        }
    }
    // Use this for initialization
    public void Initialize()
    {
        MaxSliderMap[Instinct.HOARDING] = MaxHoarding;
        MaxSliderMap[Instinct.HERDING] = MaxHerding;
        MaxSliderMap[Instinct.PREDATORY] = MaxPredatory;
        MaxSliderMap[Instinct.NOMADIC] = MaxNomadic;

        Levels[Instinct.HOARDING] = 0;
        Levels[Instinct.HERDING] = 0;
        Levels[Instinct.PREDATORY] = 0;
        Levels[Instinct.NOMADIC] = 0;

        MaxLevels[Instinct.HOARDING] = 0;
        MaxLevels[Instinct.HERDING] = 0;
        MaxLevels[Instinct.PREDATORY] = 0;
        MaxLevels[Instinct.NOMADIC] = 0;

        MaxLevelsBase[Instinct.HOARDING] = 0;
        MaxLevelsBase[Instinct.HERDING] = 0;
        MaxLevelsBase[Instinct.PREDATORY] = 0;
        MaxLevelsBase[Instinct.NOMADIC] = 0;

        ValueSliderMap[Instinct.HOARDING] = Hoarding;
        ValueSliderMap[Instinct.HERDING] = Herding;
        ValueSliderMap[Instinct.PREDATORY] = Predatory;
        ValueSliderMap[Instinct.NOMADIC] = Nomadic;

        Opposites[Instinct.HOARDING] = Instinct.NOMADIC;
        Opposites[Instinct.PREDATORY] = Instinct.HERDING;
        Opposites[Instinct.NOMADIC] = Instinct.HOARDING;
        Opposites[Instinct.HERDING] = Instinct.PREDATORY;

        //ResetValues();
    }

    // make MaxLevelBase reflect selected splices
    void UpdateMaxLevelsBase(SpeciesDesignerModel model)
    {
        foreach (Instinct i in Enum.GetValues(typeof(Instinct)))
        {
            MaxLevelsBase[i] = MinLevel;
        }

        foreach (SpliceModel sm in model.SelectedSplices)
        {
            MaxLevelsBase[sm.EInstinct] += 1;
        }
    }

    void UpdateMaxLevels()
    {
        foreach (Instinct i in Enum.GetValues(typeof(Instinct)))
        {
            Debug.Log(i);
            MaxLevels[i] = MaxLevelsBase[i] + MaxLevelsTunned[i];
        }
    }

    void UpdateSliders()
    {
        UpdateLevel(Instinct.HOARDING);
        UpdateLevel(Instinct.NOMADIC);
        UpdateLevel(Instinct.PREDATORY);
        UpdateLevel(Instinct.HERDING);
    }

    public void UpdateView(SpeciesDesignerModel model)
    {
        UpdateMaxLevelsBase(model);
        UpdateMaxLevels();
        UpdateSliders();
    }

    void UpdateView()
    {
        UpdateMaxLevels();
        UpdateSliders();
    }
        
    
    public void OnUp(Instinct i)
    {}

    // called every frame for each level
    // update sliders to reflect lists.
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

    void ArrowKeyClicked(Instinct i, bool up)
    {
        Debug.Log("Arrow key clicked: " + i + " " + (up ? "up" : "down"));
        if (!CanTune(i, up)) return;
        
        if(up)
        {
            Levels[i]++;
            MaxLevelsTunned[Opposites[i]]--;
        }
        else
        {
            Levels[i]--;
            MaxLevelsTunned[Opposites[i]]++;
        }

        UpdateView();
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

    // this should be called only after MaxLevelBase has been loaded (possibly through UpdateView())
    public void LoadModel(InstinctTuningModel model)
    {
        if(model != null)
        {
            Levels[Instinct.HOARDING] = model.HoardingLvl;
            Levels[Instinct.HERDING] = model.HerdingLvl;
            Levels[Instinct.NOMADIC] = model.NomadicLvl;
            Levels[Instinct.PREDATORY] = model.PredatoryLvl;

            MaxLevelsTunned[Instinct.HOARDING] = model.HoardingMaxLvl - MaxLevelsBase[Instinct.HOARDING];
            MaxLevelsTunned[Instinct.HERDING] = model.HerdingMaxLvl - MaxLevelsBase[Instinct.HERDING];
            MaxLevelsTunned[Instinct.NOMADIC] = model.NomadicMaxLvl - MaxLevelsBase[Instinct.NOMADIC];
            MaxLevelsTunned[Instinct.PREDATORY] = model.PredatoryMaxLvl - MaxLevelsBase[Instinct.PREDATORY];
        }
        else
        {
            ResetValues();
        }
        UpdateView();
    }

    // wraps around ChangeTune, each case correspond to an arrow button.
    public void Tune(int i)
    {
        switch (i)
        {
            case 0:
                ArrowKeyClicked(Instinct.HERDING, true);
                break;
            case 1:
                ArrowKeyClicked(Instinct.HERDING, false);
                break;
            case 2:
                ArrowKeyClicked(Instinct.HOARDING, true);
                break;
            case 3:
                ArrowKeyClicked(Instinct.HOARDING, false);
                break;
            case 4:
                ArrowKeyClicked(Instinct.PREDATORY, true);
                break;
            case 5:
                ArrowKeyClicked(Instinct.PREDATORY, false);
                break;
            case 6:
                ArrowKeyClicked(Instinct.NOMADIC, true);
                break;
            case 7:
                ArrowKeyClicked(Instinct.NOMADIC, false);
                break;
        }
    }
}
