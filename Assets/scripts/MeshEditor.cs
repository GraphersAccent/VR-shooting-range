using Parabox.CSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class MeshEditor : MonoBehaviour
{
	[SerializeField] private GameObject PrefabHoll;
    [SerializeField] private GameObject _other, _composite;
    [SerializeField] private GameObject _thisObject;
    void Start()
    {
        _thisObject = transform.gameObject;
    }

	public GameObject SetPrefab
    {
		set { PrefabHoll = value; }
    }

    public GameObject RecalculateMesh(Vector3 WorldPos, Quaternion Rotation)
    {
		Rotation = Quaternion.Euler(Rotation.eulerAngles + PrefabHoll.transform.rotation.eulerAngles);

		_other = Instantiate(PrefabHoll, WorldPos, Rotation);

        CSG_Model result;

        result = Boolean.Subtract(_thisObject, _other);

		_composite = new GameObject();
		_composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
		_composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
		_composite.AddComponent<BoxCollider>();
		_composite.AddComponent<MeshEditor>().SetPrefab = PrefabHoll;
        GenerateBarycentric(ref _composite);

		Destroy(_other);
		Destroy(_thisObject);
		return _composite;
	}

	void GenerateBarycentric(ref GameObject go)
	{
		Mesh m = go.GetComponent<MeshFilter>().sharedMesh;

		if (m == null) return;

		int[] tris = m.triangles;
		int triangleCount = tris.Length;

		Vector3[] mesh_vertices = m.vertices;
		Vector3[] mesh_normals = m.normals;
		Vector2[] mesh_uv = m.uv;

		Vector3[] vertices = new Vector3[triangleCount];
		Vector3[] normals = new Vector3[triangleCount];
		Vector2[] uv = new Vector2[triangleCount];
		Color[] colors = new Color[triangleCount];

		for (int i = 0; i < triangleCount; i++)
		{
			vertices[i] = mesh_vertices[tris[i]];
			normals[i] = mesh_normals[tris[i]];
			uv[i] = mesh_uv[tris[i]];

			colors[i] = i % 3 == 0 ? new Color(1, 0, 0, 0) : (i % 3) == 1 ? new Color(0, 1, 0, 0) : new Color(0, 0, 1, 0);

			tris[i] = i;
		}

		Mesh wireframeMesh = new Mesh();

		wireframeMesh.Clear();
		wireframeMesh.vertices = vertices;
		wireframeMesh.triangles = tris;
		wireframeMesh.normals = normals;
		wireframeMesh.colors = colors;
		wireframeMesh.uv = uv;

		go.GetComponent<MeshFilter>().sharedMesh = wireframeMesh;
	}

}
