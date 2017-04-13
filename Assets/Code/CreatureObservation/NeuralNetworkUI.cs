using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


struct NeuralLink
{
    public Line line;
    public int startNodeId;
    public int endNodeId;
    public float weight;
}

public class NeuralNetworkUI : MonoBehaviour
{
    Creature ToDetail;
    public HorizontalLayoutGroup InputGroup;
    public HorizontalLayoutGroup HiddenGroup;
    public HorizontalLayoutGroup OutputGroup;

    // TODO: there should be different node objects for different types
    public GameObject NodeObject;
    public GameObject LineObject;
    public GameObject LinkParentObject;


    Dictionary<int, NeuralNode> nodeMap;
    List<NeuralLink> linkList;
    int numInputNodes = 0;
    int numHiddenNodes = 0;
    int numOutputNodes = 0;
    Text DNAText;

    public void SetCreature(Creature inCreature)
    {
        // first clean stuff up
        ToDetail = inCreature;
        if (nodeMap == null)
        {
            nodeMap = new Dictionary<int, NeuralNode>();
        }
        if (linkList == null)
        {
            linkList = new List<NeuralLink>();
        }
        nodeMap.Clear();
        linkList.Clear();
        foreach (Transform child in InputGroup.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in HiddenGroup.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in OutputGroup.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in LinkParentObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        numInputNodes = 0;
        numOutputNodes = 0;
        numHiddenNodes = 0;

        DNAText = this.transform.parent.FindChild("DNA").GetComponent<Text>();
        // instantiate nodes
        int i = 0;
        foreach (var i_v in ToDetail.LastControlStep.Inputs)
        {
            GameObject newNode = Instantiate(NodeObject) as GameObject;
            newNode.transform.SetParent(InputGroup.transform);
            newNode.GetComponent<NeuralNode>().id = 1000 + i;
            nodeMap.Add(1000 + i, newNode.GetComponent<NeuralNode>());
            i++;
        }
        numInputNodes = i;
        i = 0;
        foreach (var h_v in ToDetail.LastControlStep.Hidden)
        {
            GameObject newNode = Instantiate(NodeObject) as GameObject;
            newNode.transform.SetParent(HiddenGroup.transform);
            newNode.GetComponent<NeuralNode>().id = 2000 + i;
            nodeMap.Add(2000 + i, newNode.GetComponent<NeuralNode>());
            i++;
        }
        numHiddenNodes = i;
        i = 0;
        foreach (var o_v in ToDetail.LastControlStep.Outputs)
        {
            GameObject newNode = Instantiate(NodeObject) as GameObject;
            newNode.transform.SetParent(OutputGroup.transform);
            newNode.GetComponent<NeuralNode>().id = 3000 + i;
            nodeMap.Add(3000 + i, newNode.GetComponent<NeuralNode>());
            i++;
        }
        numOutputNodes = i;
        DrawLinks();

    }

    void DrawLinks()
    {
        // draw links
        int i = 0;
        for (int j = 0; j < numInputNodes; j++)
        {
            for (int k = 0; k < numHiddenNodes; k++)
            {
                DrawLink(nodeMap[1000 + j], nodeMap[2000 + k], 1000 + j, 2000 + k, ToDetail.Model.Control.InToHidden.Coefficients[i]);
                i++;
            }
        }
        i = 0; 
        for (int j = 0; j < numHiddenNodes; j++)
        {
            for (int k = 0; k < numOutputNodes; k++)
            {
                DrawLink(nodeMap[2000 + j], nodeMap[3000 + k], 2000 + j, 3000 + k, ToDetail.Model.Control.HiddenToOutput.Coefficients[i]);
                i++;
            }
        }
    }

    void SetNodeValues()
    {
        int i = 0;
        foreach (var i_v in ToDetail.LastControlStep.Inputs)
        {
            if (nodeMap.ContainsKey(1000 + i))
            {
                nodeMap[1000 + i].SetValue(i_v);
            }
            i++;
        }

        i = 0;
        foreach (var h_v in ToDetail.LastControlStep.Hidden)
        {
            if (nodeMap.ContainsKey(2000 + i))
            {
                nodeMap[2000 + i].SetValue(h_v);
            }
            i++;
        }

        i = 0;
        foreach (var o_v in ToDetail.LastControlStep.Outputs)
        {
            if (nodeMap.ContainsKey(3000 + i))
            {
                nodeMap[3000 + i].SetValue(o_v);
            }
            i++;
        }
    }

    /// <summary>
    /// Return link color based on its weight
    /// </summary>
    Color WeightToColor(float inWeight)
    {
        float weight = Mathf.Clamp(inWeight, -1.0f, 1.0f);
        weight = (weight + 1.0f) / 2;
        weight = Mathf.Clamp(weight, 0.0f, 1.0f);

        // lerp between red and green
        Vector3 colorSpaceCoord = Vector3.Lerp(new Vector3(1.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), weight);

        return new Color(colorSpaceCoord.x, colorSpaceCoord.y, colorSpaceCoord.z, 1.0f);
    }

    void DrawLink(NeuralNode fromNode, NeuralNode toNode, int fromId, int toId, float weight)
    {
        GameObject line = Instantiate(LineObject);
        line.transform.SetParent(LinkParentObject.transform);
        line.GetComponent<Line>().Initialise(fromNode.transform.position, toNode.transform.position, 1.0f, Color.red);

        NeuralLink nlink = new NeuralLink();
        nlink.line = line.GetComponent<Line>();
        nlink.startNodeId = fromId;
        nlink.endNodeId = toId;
        nlink.weight = weight;
        nlink.line.color = WeightToColor(weight);
        linkList.Add(nlink);
    }

    void UpdateLinks()
    {
        foreach (NeuralLink n in linkList)
        {
            if((Vector2)nodeMap[n.startNodeId].transform.position != n.line.start)
            {
                n.line.start = (Vector2)nodeMap[n.startNodeId].transform.position;
            }
            if ((Vector2)nodeMap[n.endNodeId].transform.position != n.line.end)
            {
                n.line.end = (Vector2)nodeMap[n.endNodeId].transform.position;
            }
        }
    }

    void PrintText()
    {
        DNAText.text = "Species: " + ToDetail.SpeciesName;
        DNAText.text += "\n Specimen: " + ToDetail.Model.ID;
        DNAText.text += "\n DNA Sequence: ";
        foreach (var el in ToDetail.Model.Morphology.Body)
        {
            DNAText.text += string.Format("{0}", el.Trait.TID);
        }

        DNAText.text += "\n";
        DNAText.text += " Fitness: ";
        DNAText.text += ToDetail.Model.Fitness;

        DNAText.text += "\n";
        DNAText.text += " BestFitness: ";
        DNAText.text += ToDetail.Controller.BestFitness;
        
        DNAText.text += "\n";
        DNAText.text += " DebugInfo: ";
        DNAText.text += ToDetail.DebugText;
        DNAText.text += "\n";
        DNAText.text += " Mover Executed: ";
        DNAText.text += ToDetail.Mover.ExecutedSteps;
    }

    void LateUpdate ()
    {
        SetNodeValues(); 
        UpdateLinks();
        PrintText();
    }
}
