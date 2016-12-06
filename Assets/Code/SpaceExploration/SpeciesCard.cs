using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeciesCard : MonoBehaviour
{
    public GameObject SpeciesMenu, ExtractionMenu;
    GameObject OrbitalUI;
    int AmmountToExtract = 0;
    private SpeciesModel Species;
    public Text IndicatiorText;
    public BiomassManager PlayerMgr;


	// Use this for initialization
	void Start ()
    {
        ExtractionMenu.SetActive(false);
        SpeciesMenu.SetActive(false);
        OrbitalUI = GameObject.FindGameObjectWithTag("OrbitalUI");
	}

    public void OnClicked()
    {
        //OrbitalUI.GetComponent<OrbitalUI>().OnEditCreatureClicked("Cool dudes");
        SpeciesMenu.SetActive(!SpeciesMenu.activeInHierarchy);
        if (ExtractionMenu.activeInHierarchy) ExtractionMenu.SetActive(false);

    }

    public void OpenExtractMenu()
    {
        ExtractionMenu.SetActive(!ExtractionMenu.activeInHierarchy);
    }

    public void OpenEditor()
    {
        OrbitalUI.GetComponent<OrbitalUI>().OnEditCreatureClicked("Cool dudes");
    }

    public void IncrementAmmount()
    {
        AmmountToExtract++;
        IndicatiorText.text = ""+AmmountToExtract;
   }

    public void DecrementAmmount()
    {
        AmmountToExtract--;
        IndicatiorText.text = "" + AmmountToExtract;
    }

    public void SetAmmount(int i)
    {
        AmmountToExtract = i;
    }

    public void Extract()
    {
        //TODO: Implelent Extraction from planet simulation
        BiomassManager.AddBiomass(AmmountToExtract);
        AmmountToExtract = 0;
        IndicatiorText.text = "" + AmmountToExtract;
    }
}
