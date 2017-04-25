using UnityEngine;
using System.Collections.Generic;

public class SkyDome : MonoBehaviour
{
	public int m_Divisions = 10;
	public float m_PlanetRadius = 280f;
	public float m_AtmosphereRadius = 295f;
	public float m_HTilefactor = 2f;
	public float m_VTilefactor = 2f;
	
	private List<Vector3> m_Vertices = new List<Vector3>();
	private List<Vector3> m_Normals = new List<Vector3>();
	private List<Vector2> m_Texcoords = new List<Vector2>();
	private List<int> m_Triangles = new List<int>();
	private Mesh m_Mesh = null;
	
    private void Start ()
	{
		MeshRenderer rd = this.gameObject.GetComponent<MeshRenderer>();
		rd.material.SetVector ("_Resolution", new Vector4(100, 100, 0, 0));
	}
	private void Update ()
    {
		GenerateSkyPlane();
		
		Vector3 newpos = new Vector3 (Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
		transform.position = newpos;
    }
	private void GenerateSkyPlane ()
	{
		m_Vertices.Clear();
		m_Normals.Clear();
		m_Texcoords.Clear();
		m_Triangles.Clear();
	
		int divs = Mathf.Clamp (m_Divisions, 1, 256);
		int numIndices = divs * divs * 2 * 3;
		int numDivisions = (int)Mathf.Sqrt (numIndices / 6f);
		
		float plane_size = 2f * Mathf.Sqrt((m_AtmosphereRadius * m_AtmosphereRadius) - (m_PlanetRadius * m_PlanetRadius));
		float delta = plane_size / (float)numDivisions;
		float tex_delta = 2.0f / (float)numDivisions;
		float x_dist   = 0.0f;
		float z_dist   = 0.0f;
		float x_height = 0.0f;
		float z_height = 0.0f;
		float height = 0.0f;

		for (int i = 0; i <= numDivisions; i++)
		{
			for (int j = 0; j <= numDivisions; j++)
			{
				x_dist = (-0.5f * plane_size) + ((float)j*delta);
				z_dist = (-0.5f * plane_size) + ((float)i*delta);

				x_height = (x_dist*x_dist) / m_AtmosphereRadius;
				z_height = (z_dist*z_dist) / m_AtmosphereRadius;
				height = x_height + z_height;

				m_Vertices.Add (new Vector3 (x_dist, -height, z_dist));
				m_Normals.Add (new Vector3 (0, -1, 0));
				m_Texcoords.Add (new Vector2 (
					m_HTilefactor * ((float)j * tex_delta * 0.5f),
					m_VTilefactor * (1.0f - (float)i * tex_delta * 0.5f)
				));
			}
		}

		for (int i = 0; i < numDivisions; i++)
		{
			for (int j = 0; j < numDivisions; j++)
			{
				int startvert = (i * (numDivisions + 1) + j);

				// tri 1
				AddTriangle(
					startvert,
					startvert + 1,
					startvert + numDivisions + 1);
				
				// tri 2
				AddTriangle(
					startvert + 1,
					startvert + numDivisions + 2,
					startvert + numDivisions + 1);
			}
		}
		
		if (m_Mesh == null)
		{
			m_Mesh = new Mesh();
			m_Mesh.name = "Sky Dome";
		}
		else
		{
			m_Mesh.Clear();
		}
		m_Mesh.vertices = m_Vertices.ToArray();
		m_Mesh.normals = m_Normals.ToArray();
		m_Mesh.uv = m_Texcoords.ToArray();
		m_Mesh.triangles = m_Triangles.ToArray();
		m_Mesh.RecalculateNormals();
		
		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter != null)
			filter.mesh = m_Mesh;
	}
	private void AddTriangle (int ind0, int ind1, int ind2)
	{
		m_Triangles.Add(ind0);
		m_Triangles.Add(ind1);
		m_Triangles.Add(ind2);
	}
}
