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
           DnaSeq.text += "\n";
           DnaSeq.text += " DNA Sequence: ";
           foreach (var el in ToDetail.Model.Morphology.Body)
           {
               DnaSeq.text += string.Format("{0}", el.Trait.TID);
           }
           
           NN.text = "\n Nerual Network \n";
           if (ToDetail.LastControlStep != null)
           {
               NN.text += "\n Input Layer: \n";
               foreach (var i_v in ToDetail.LastControlStep.Inputs)
               {
                   NN.text += string.Format("[{0}]", i_v);
               }
               NN.text += "\n Hidden Layer: \n";
               foreach (var h_v in ToDetail.LastControlStep.Hidden)
               {
                   NN.text += string.Format("[{0}]", h_v);
               }
               NN.text += "\n Output Layer: \n";
               foreach (var o_v in ToDetail.LastControlStep.Outputs)
               {
                   NN.text += string.Format("[{0}]", o_v);
               }
           }
       }
    }
}
    