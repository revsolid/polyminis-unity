using UnityEngine;
using UnityEngine.UI;


public class DetailedViewUI : MonoBehaviour
{
    public Text DnaSeq;
    public Text NN;
    public Creature ToDetail;
    
    void LateUpdate()
    {
       if (ToDetail != null) 
       {
           DnaSeq.text = ToDetail.DebugText.text;
       }
    }
}
    