using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HoverScriptAll : MonoBehaviour {

    Vector3 OriginalPosition;
    // Use this for initialization
	void Start () {
        OriginalPosition = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnMouseEnter()
    {
       this.transform.Translate(Vector3.left * 100.0f);
    }
    public void OnMouseExit()
    {

        this.transform.Translate(Vector3.right * 100.0f);
    }
}
