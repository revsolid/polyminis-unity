using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpliceCounter : MonoBehaviour
{

    public SpliceBox[] boxes;
    public int[] spliceID; 
    int numBoxes;
    int currentBox;
    bool full = false;

    // Use this for initialization
    void Start ()
    {
        numBoxes = boxes.Length;
        currentBox = 0;
    }
    
    public void setSelected(SpliceButton pressed)
    {
        if (full) throw new SpliceButton.splicesFullException();

        boxes[currentBox].assign(pressed);
        currentBox++;
        full = (currentBox == numBoxes);
    }
    public void deselect(SpliceButton pressed)
    {
        for (int i = 0; i < numBoxes; i++)
        {
            if (pressed.Equals(boxes[i].getButton()))
            {
                for (int j = i; j < currentBox; j++)
                {
                    if (j + 1 < numBoxes) boxes[j].assign(boxes[j + 1].getButton());
                    else boxes[j].free();
                }
            }
        }

        currentBox--;
        full = false;
    }
}
