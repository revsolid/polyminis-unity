using UnityEngine;
using System.Collections;

public class StarrySky : MonoBehaviour
{
	private Vector2 m_Offset = new Vector2 ();
    public Vector2 m_Velocity = new Vector2 ();

	// random texture for galaxy sky
	public GameObject[] m_Sky;
	private Texture2D m_RandomTex;
	private float m_Time = 0;
	[Range(0, 0.2f)] public float m_GalaxyDt = 0;
	
	void Start ()
	{
		System.Random rnd = new System.Random ();
		m_RandomTex = new Texture2D (512, 512);
		m_RandomTex.filterMode = FilterMode.Point;
		Color[] pixels = m_RandomTex.GetPixels ();
		for (int i = 0; i < pixels.Length; i++)
		{
			float v = (float)rnd.NextDouble ();
			pixels[i] = new Color (v, v, v);
		}
		m_RandomTex.SetPixels (pixels);
		m_RandomTex.Apply ();
	}
	void Update ()
	{
		m_Offset += m_Velocity * 0.001f;
		m_Time += m_GalaxyDt;
		for (int i = 0; i < m_Sky.Length; i++)
		{
			Renderer rd = m_Sky[i].GetComponent<Renderer> ();
			rd.material.SetTexture ("_RandomTex", m_RandomTex);
			rd.material.SetFloat ("_Distort", m_Time);
			rd.material.SetVector ("_Offset", m_Offset);
		}
	}
}