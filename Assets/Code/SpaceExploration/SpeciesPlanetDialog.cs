using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SpeciesPlanetAction
{
    Extract,
    Deploy,
    Research,
    Sample,
    Erroring
}

public class SpeciesPlanetDialog : MonoBehaviour
{
    public Slider BiomassSlider;
    public PolyminisDialog MainDialog;
    public Text BiomassValue;
    
    public SpeciesPlanetAction CurrentAction = SpeciesPlanetAction.Deploy;
    
    public PlanetModel PlanetModel;
    public SpeciesModel SpeciesModel;

    public InventoryFullDialog InvFullDialog;
    
    // Use this for initialization
    void Start ()
    {
        BiomassValue.text = BiomassSlider.value.ToString("0.00");
    }
    
    // Update is called once per frame
    void Update ()
    {
        switch (CurrentAction)    
        {
            case SpeciesPlanetAction.Deploy:
                MainDialog.DialogMessage.text = "How much Biomass do you want to Deploy this species with?";
                BiomassSlider.gameObject.SetActive(true);
                BiomassSlider.minValue = 10.0f;
                BiomassSlider.maxValue = 100.0f / ( PlanetModel.Species.Count + 1); 
                break;
            case SpeciesPlanetAction.Extract:
                MainDialog.DialogMessage.text  = "How much Biomass do you want to Extract from this Species?";
                BiomassSlider.gameObject.SetActive(true);
                BiomassSlider.minValue = 0.0f;
                BiomassSlider.maxValue = SpeciesModel.Percentage * 100; 
                break;
            case SpeciesPlanetAction.Research:
                MainDialog.DialogMessage.text = "Do you want to research this Species? (Researching takes a slot)";
                BiomassSlider.gameObject.SetActive(false);
                break;
            case SpeciesPlanetAction.Sample:
                MainDialog.DialogMessage.text = "Do you want to Sample DNA from this Species?";
                BiomassSlider.gameObject.SetActive(false);
                break;
            case SpeciesPlanetAction.Erroring:
                MainDialog.DialogMessage.text = "Something went Wrong :S";
                BiomassSlider.gameObject.SetActive(false);
                break;
        }
        if (!BiomassSlider.gameObject.active)
        {
            BiomassValue.text = "";
        }
        
    }
    
    public void OnSliderValueChange()
    {
        BiomassValue.text = BiomassSlider.value.ToString("0.0");
    }
    
    public void OnAccept()
    {
        PlanetInteractionCommand pInteractionCommand;
        switch (CurrentAction)    
        {
            case SpeciesPlanetAction.Deploy:
                pInteractionCommand = new PlanetInteractionCommand(PlanetInteractionCommandType.DEPLOY);
                pInteractionCommand.Epoch = PlanetModel.Epoch; 
                pInteractionCommand.PlanetId = PlanetModel.ID; 
                pInteractionCommand.DeployedBiomass = BiomassSlider.value;
                pInteractionCommand.Species = SpeciesModel;
                Connection.Instance.Send(JsonUtility.ToJson(pInteractionCommand));
                break;
            case SpeciesPlanetAction.Sample:
                InventoryCommand sampleSpeciesCommand = new InventoryCommand(InventoryCommandType.SAMPLE_FROM_PLANET);
                sampleSpeciesCommand.Species = SpeciesModel;
                sampleSpeciesCommand.Epoch = PlanetModel.Epoch; 
                sampleSpeciesCommand.PlanetId = PlanetModel.ID; 
                sampleSpeciesCommand.Slot = Session.Instance.NextAvailableSlot();
                if (sampleSpeciesCommand.Slot == -1)
                {
                // This is an issue    
                    Debug.Log("NO MORE SLOTS FOR YOU!!!!!");
                }
                else
                {
                    Connection.Instance.Send(JsonUtility.ToJson(sampleSpeciesCommand));
                }
                break;
            case SpeciesPlanetAction.Research:
                InventoryCommand researchSpeciesCommand = new InventoryCommand(InventoryCommandType.RESEARCH);
                researchSpeciesCommand.Species = SpeciesModel;
                researchSpeciesCommand.Epoch = PlanetModel.Epoch; 
                researchSpeciesCommand.PlanetId = PlanetModel.ID; 
                researchSpeciesCommand.Slot = Session.Instance.NextAvailableSlot();
                if (researchSpeciesCommand.Slot == -1)
                {
                    InventoryFullDialog ifDialog = Instantiate(InvFullDialog);
                    Debug.Log("NO MORE SLOTS FOR YOU!!!!!");
                }
                else
                {
                    Connection.Instance.Send(JsonUtility.ToJson(researchSpeciesCommand));
                }
                break;
            case SpeciesPlanetAction.Extract:
                pInteractionCommand = new PlanetInteractionCommand(PlanetInteractionCommandType.EXTRACT);
                pInteractionCommand.Epoch = PlanetModel.Epoch; 
                pInteractionCommand.PlanetId = PlanetModel.ID; 
                pInteractionCommand.ExtractedPercentage = BiomassSlider.value / 100.0f;
                pInteractionCommand.Species = SpeciesModel;
                Connection.Instance.Send(JsonUtility.ToJson(pInteractionCommand));
                break;
        }
        Destroy(gameObject);
    }
}
