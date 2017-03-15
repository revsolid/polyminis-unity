using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuralNetworkUI : MonoBehaviour
{
    Creature ToDetail;
    public HorizontalLayoutGroup InputGroup;
    public HorizontalLayoutGroup HiddenGroup;
    public HorizontalLayoutGroup OutputGroup;

    Dictionary<int, NeuralNode> nodeMap;

    // TODO: there should be different node objects for different types
    public GameObject NodeObject;

    public void SetCreature(Creature inCreature)
    {
        // first clean stuff up
        ToDetail = inCreature;
        if (nodeMap == null)
        {
            nodeMap = new Dictionary<int, NeuralNode>();
        }
        nodeMap.Clear();
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

        i = 0;
        foreach (var h_v in ToDetail.LastControlStep.Hidden)
        {
            GameObject newNode = Instantiate(NodeObject) as GameObject;
            newNode.transform.SetParent(HiddenGroup.transform);
            newNode.GetComponent<NeuralNode>().id = 2000 + i;
            nodeMap.Add(2000 + i, newNode.GetComponent<NeuralNode>());
            i++;
        }

        i = 0;
        foreach (var o_v in ToDetail.LastControlStep.Outputs)
        {
            GameObject newNode = Instantiate(NodeObject) as GameObject;
            newNode.transform.SetParent(OutputGroup.transform);
            newNode.GetComponent<NeuralNode>().id = 3000 + i;
            nodeMap.Add(3000 + i, newNode.GetComponent<NeuralNode>());
            i++;
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


    void LateUpdate ()
    {
        SetNodeValues();    
    }
}
