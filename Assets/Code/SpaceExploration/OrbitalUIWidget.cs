using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OrbitalUIWidget : MonoBehaviour {

    public Text text;

    // Use this for initialization
    void Start () {
        RectTransform rt = GetComponent<RectTransform>();
        float offsetX = (rt.localRotation.z > 0) ? 2f : -2f;
        Vector3 textPosition = new Vector3(transform.position.x + offsetX, transform.position.y + 2f, transform.position.z);
        text.transform.position = textPosition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
