using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OrbitalUIWidget : MonoBehaviour {

    public Text text;

    void OnWillRenderObject()
    {
        if (text != null)
        {
            RectTransform rt = GetComponent<RectTransform>();
            float offsetX = (rt.localRotation.z > 0) ? 2f : -2f;
            Vector3 textPosition = new Vector3(transform.position.x + offsetX, transform.position.y + 2f, transform.position.z);
            text.transform.position = textPosition;
        }
    }
}
