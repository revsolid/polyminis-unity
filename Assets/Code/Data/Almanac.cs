using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Utils;

//TODO: Temp Implementation
class STATIC_JSON 
{
public static string A_SPLICES =  "{ \"version\": \"tutorial_1\", \"traits_version\": \"tutorial_1\", \"available_splices\": [ { \"Name\": \"Polar\", \"InternalName\": \"polar\", \"Traits\": [ \"coldresist\", \"tempsensor\" ], \"Description\": \"Better adapted to cold weather\", \"Instinct\": \"Nomadic\", \"Size\": \"SMALL\" }, { \"Name\": \"Tropical\", \"Traits\": [ \"tempsensor\", \"hotresist\" ], \"InternalName\": \"tropical\", \"Description\": \"Better adapted to hot weather\", \"Instinct\": \"Nomadic\", \"Size\": \"SMALL\" }, { \"Name\": \"Hot'n'Cold\", \"Traits\": [ \"hotresist\", \"coldresist\" ], \"InternalName\": \"hot_and_cold\", \"Description\": \"Adapts to any type of weather\", \"Instinct\": \"Nomadic\", \"Size\": \"SMALL\" }, { \"Name\": \"G-Eater\", \"InternalName\": \"g_eater\", \"Traits\": [ \"gsensor\", \"gtou\" ], \"Description\": \"Can eat G!\", \"Instinct\": \"Hoarding\", \"Size\": \"SMALL\" }, { \"Name\": \"Cryophile\", \"InternalName\": \"cryophile\", \"Traits\": [ \"coldresist\", \"hottempregulator\" ], \"Description\": \"Love me some cold weather\", \"Instinct\": \"Nomadic\", \"Size\": \"MEDIUM\" }, { \"Name\": \"Thermophile\", \"InternalName\": \"thermophile\", \"Traits\": [ \"hotresist\", \"coldtempreg\" ], \"Description\": \"Love me some hot weather\", \"Instinct\": \"Nomadic\", \"Size\": \"MEDIUM\" } ] }";

public static string A_TRAITS = "{ \"version\": \"tutorial_1\", \"available_traits\": [ { \"TID\": 6, \"Name\": \"Horizontal Movement\", \"InternalName\": \"hormov\", \"Tier\": \"BasicTier\", \"Type\": \"Actuator\" }, { \"TID\": 7, \"Name\": \"Vertical Movement\", \"InternalName\": \"vermov\", \"Tier\": \"BasicTier\", \"Type\": \"Actuator\" }, { \"TID\": 8, \"Name\": \"Extra Speed\", \"InternalName\": \"speed\", \"Tier\": \"BasicTier\", \"Type\": \"SimpleTrait\" }, { \"TID\": 1, \"Name\": \"Cold Weather Resistance Trait\", \"InternalName\": \"coldresist\", \"Tier\": \"TierI\", \"Type\": \"SimpleTrait\" }, { \"TID\": 2, \"Name\": \"Hot Weather Resistance Trait\", \"InternalName\": \"hotresist\", \"Tier\": \"TierI\", \"Type\": \"SimpleTrait\" }, { \"TID\": 3, \"Name\": \"Temperature Sensor\", \"InternalName\": \"tempsensor\", \"Tier\": \"TierI\", \"Type\": \"Sensor\" }, { \"TID\": 4, \"Name\": \"G-Substance Sensor\", \"InternalName\": \"gsensor\", \"Tier\": \"TierI\", \"Type\": \"Sensor\" }, { \"TID\": 5, \"Name\": \"G-Substance Consumer\", \"InternalName\": \"gtou\", \"Tier\": \"TierI\", \"Type\": \"Actuator\" }, { \"TID\": 16, \"Name\": \"Temperature Regulator (to Hot)\", \"InternalName\": \"hottempreg\", \"Tier\": \"TierII\", \"Type\": \"Actuator\" }, { \"TID\": 17, \"Name\": \"Temperature Regulator (to Cold)\", \"InternalName\": \"coldtempreg\", \"Tier\": \"TierII\", \"Type\": \"Actuator\" } ] }";
}
[Serializable]
class Traits
{
	public string version;
	public TraitModel[] available_traits;
}

[Serializable]

class Splices
{
	public string version;
	public string traits_version;
	public SpliceModel[] available_splices; 
}

[Serializable]
class StaticData
{
	public SpliceModel[] SpliceData; 
	public TraitModel[] TraitData;
}

// The almanac should hold all the static information available to the player
public class Almanac : Singleton<Almanac>
{
	// internalname => SpliceModel
	public IDictionary<string, SpliceModel> AvailableSplices;
	// organelleId => OrganelleModel
	public IDictionary<int, TraitModel> TraitData;

    // Protected Constructor for Singleton
	protected Almanac() {}

	void ARRRwake()
	{
		// TODO: Get it from the server instead of STATIC
/*		Traits tts = JsonUtility.FromJson<Traits>(STATIC_JSON.TRAITS);
		Debug.Log(tts.version);
		Debug.Log(tts.available_traits.Length);
		Splices spcs = JsonUtility.FromJson<Splices>(STATIC_JSON.SPLICES);
		Debug.Log(spcs.available_splices);

		TraitData = new Dictionary<int, TraitModel>();
		for (var i = 0; i < tts.available_traits.Length; i++)
		{
			TraitModel model = tts.available_traits[i];
			Debug.Log(model.TID);
			TraitData[model.TID] = model;
		}
		
		AvailableSplices = new Dictionary<string, SpliceModel>();
		for (var i = 0; i < spcs.available_splices.Length; i++)
		{
			SpliceModel sm = spcs.available_splices[i];
			AvailableSplices[sm.InternalName] = sm;
		}*/
	}
	
	public void SetFromJson(string msg)
	{
		StaticData sd = JsonUtility.FromJson<StaticData>(msg);

		TraitData = new Dictionary<int, TraitModel>();
		for (var i = 0; i < sd.TraitData.Length; i++)
		{
			TraitModel model = sd.TraitData[i];
			Debug.Log(model.TID);
			TraitData[model.TID] = model;
		}
		
		AvailableSplices = new Dictionary<string, SpliceModel>();
		for (var i = 0; i < sd.SpliceData.Length; i++)
		{
			SpliceModel sm = sd.SpliceData[i];
			AvailableSplices[sm.InternalName] = sm;
		}	
		
		Debug.Log("Set from Json!");
	}
}