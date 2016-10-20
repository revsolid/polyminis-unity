using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpliceUIManager : MonoBehaviour {

    public GameObject tabPanel, splicePanel; //Set these in editor
    private HorizontalLayoutGroup tabLayout;
    private Button[] tabButtons;
    private GameObject[] tabs;
    public Color[] tabColors = {Color.yellow, Color.green, Color.blue,Color.red};
    public SpliceCounter[] counters = new SpliceCounter[3];
    public GridLayoutGroup[] spliceTabs = new GridLayoutGroup[4];
    public Slider[] displayBars = new Slider[4];
    public Slider[] instinctBars = new Slider[4];
    private int[] pointAllocation = { 0, 0, 0, 0 };
    public int currentCategory {get; private set; }
    public Button[] UpButtons = new Button[4];
    public Button[] DownButtons = new Button[4];
    public  InputField SpeciesName;

    
	void Start ()
    {

        tabLayout = tabPanel.GetComponent<HorizontalLayoutGroup>();
        tabButtons = new Button[4];
        int i = 0;
        foreach (Button b in tabLayout.GetComponentsInChildren<Button>())
        {
            tabButtons[i] = b;
            i++;
        }

        currentCategory = 0;
        //TODO: This should come from server / loaded species data
        SpeciesName.text = "Test Species";
	}

    void Update()
    {
	
	}

    public void tabClicked(int id)
    {
        for (int i = 0; i < 4; i++)
        {
            spliceTabs[i].gameObject.SetActive(i == id);
        }
        
        currentCategory = id;
    }

    public void updateBars(SpliceButton button, bool turnedOn)
    {
        if (turnedOn)
        {
            pointAllocation[button.category] += (button.size + 1);
            button.GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            pointAllocation[button.category] -= (button.size + 1);
            button.GetComponentInChildren<Text>().color = Color.black;

            while ( Mathf.Max(displayBars[button.category].value - (button.size + 1), 0) < instinctBars[button.category].value)
            {
                decrementInstinct(button.category);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            displayBars[i].value = pointAllocation[i];
        }
    }
    
    public void OnCloseClicked()
    {
        gameObject.SetActive(false);
        // SAVE et al.
    }

    public void incrementInstinct(int id)
    {
        int[] opposites = {3,2,1,0};
        if (instinctBars[id].value < displayBars[id].value && displayBars[opposites[id]].value > instinctBars[opposites[id]].value )
        {
            Debug.Log("Incrementing");
            instinctBars[id].value++;
            displayBars[opposites[id]].value--;
        }
    }

    public void decrementInstinct(int id)
    {
        int[] opposites = { 3, 2, 1, 0 };
        if (instinctBars[id].value > 0)
        {
            instinctBars[id].value--;
            displayBars[opposites[id]].value++;
        }
    }
}
