using UnityEngine;
using System.Collections;

public class Galaxy : MonoBehaviour
{
	public Color m_Color;
	[Range(-5, 5)] public float m_RotDegrees = 0;
	
	void Start ()
	{
	}
	void Update ()
	{
		Renderer rd = GetComponent<Renderer> ();
		rd.material.SetColor ("_Color", m_Color);

		Transform t = GetComponent<Transform> ();
		t.rotation = Quaternion.AngleAxis (m_RotDegrees, transform.forward) * t.rotation;
	}
}