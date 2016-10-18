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
    private int[] pointAllocation = { 0, 0, 0, 0 };
    public int currentCategory {get; private set; }

    
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
        }
        else
        {
            pointAllocation[button.category] -= (button.size + 1);
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
}
