using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BezierSolution;
using UnityEngine;
using UnityEngine.UI;

public class MouthEditor : MonoBehaviour
{
	private static readonly int tongueVectors = 63;

	private static readonly int lipVectors = 22;

	private static readonly int airflowVectors = 6;

	public GameObject lowerJawBone;

	public GameObject tongue;

	public GameObject palate;

	public GameObject uvula;

	public BezierSpline spline;

	private GameObject[] tongueTarget = new GameObject[tongueVectors];

	private GameObject[] lipTarget = new GameObject[lipVectors];

	private GameObject[] airflowTarget = new GameObject[airflowVectors];

	private List<List<int>> duplicateVerts = new List<List<int>>();

	private Transform tongueTransform;

	private Transform lipTransform;

	private Transform airflowTransform;

	private SkinnedMeshRenderer meshFilter;

	private Mesh sourceMesh;

	private Mesh mesh;

	public Vector3 scaleFactor = new Vector3(1.05f, 1.05f, 1.05f);

	public int smoothFactor = 4;

	public InputField textCtrl;

	private Quaternion defaultJawPos;

	private void Start()
	{
		meshFilter = tongue.GetComponentInChildren<SkinnedMeshRenderer>();
		if (meshFilter == null)
		{
			Debug.Log("SkinnedMeshRenderer not found");
		}
		tongue.transform.localScale = Vector3.Scale(base.transform.localScale, scaleFactor);
		defaultJawPos = lowerJawBone.transform.rotation;
		for (int i = 0; i < tongueVectors; i++)
		{
			tongueTarget[i] = new GameObject("tongueTarget." + i.ToString("000"));
			tongueTarget[i].transform.parent = base.transform;
			tongueTarget[i].AddComponent(Type.GetType("gizmo"));
			tongueTransform = lowerJawBone.transform.Find("tongue." + i.ToString("000"));
			if (tongueTransform != null)
			{
				tongueTarget[i].transform.position = tongueTransform.position;
			}
		}
		for (int j = 0; j < lipVectors; j++)
		{
			lipTarget[j] = new GameObject("lipTarget." + j.ToString("000"));
			lipTarget[j].transform.parent = base.transform;
			lipTarget[j].AddComponent(Type.GetType("gizmo"));
			lipTransform = GameObject.Find("lips." + j.ToString("000")).transform;
			if (lipTransform != null)
			{
				lipTarget[j].transform.position = lipTransform.position;
			}
		}
		for (int k = 0; k < airflowVectors; k++)
		{
			airflowTarget[k] = new GameObject("airflowTarget." + k.ToString("000"));
			airflowTarget[k].transform.parent = base.transform;
			airflowTarget[k].AddComponent(Type.GetType("gizmo"));
			airflowTransform = GameObject.Find("OralAirflow." + k.ToString("000")).transform;
			if (airflowTransform != null)
			{
				airflowTarget[k].transform.position = airflowTransform.position;
			}
		}
		sourceMesh = utils.CloneMesh(meshFilter.sharedMesh);
		mesh = new Mesh();
		meshFilter.BakeMesh(mesh);
		GetDuplicateVerts(ref mesh);
		mesh = SmoothingFilter.LaplacianFilter(mesh, smoothFactor);
		SetDuplicateVerts(ref mesh);
		meshFilter.sharedMesh = mesh;
	}

	private void Update()
	{
		for (int i = 0; i < tongueVectors; i++)
		{
			tongueTransform = lowerJawBone.transform.Find("tongue." + i.ToString("000"));
			if (tongueTransform != null)
			{
				tongueTransform.position = tongueTarget[i].transform.position;
			}
		}
		if (defaultJawPos != lowerJawBone.transform.rotation)
		{
			for (int j = 0; j < lipVectors; j++)
			{
				lipTransform = GameObject.Find("lips." + j.ToString("000")).transform;
				if (lipTransform != null)
				{
					lipTarget[j].transform.position = lipTransform.position;
				}
			}
			defaultJawPos = lowerJawBone.transform.rotation;
		}
		for (int k = 0; k < lipVectors; k++)
		{
			lipTransform = GameObject.Find("lips." + k.ToString("000")).transform;
			if (lipTransform != null)
			{
				lipTransform.position = lipTarget[k].transform.position;
			}
		}
		for (int l = 0; l < airflowVectors; l++)
		{
			airflowTransform = GameObject.Find("OralAirflow." + l.ToString("000")).transform;
			if (airflowTransform != null)
			{
				airflowTransform.position = airflowTarget[l].transform.position;
			}
		}
		spline.AutoConstructSpline();
		meshFilter.sharedMesh = sourceMesh;
		meshFilter.BakeMesh(mesh);
		mesh = SmoothingFilter.LaplacianFilter(mesh, smoothFactor);
		SetDuplicateVerts(ref mesh);
		meshFilter.sharedMesh = mesh;
	}

	private void GetDuplicateVerts(ref Mesh mesh)
	{
		HashSet<Vector3> hashSet = new HashSet<Vector3>();
		HashSet<Vector3> hashSet2 = new HashSet<Vector3>();
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < mesh.vertices.Length; i++)
		{
			if (!hashSet.Add(mesh.vertices[i]))
			{
				if (!hashSet2.Add(mesh.vertices[i]))
				{
					int index = list.IndexOf(mesh.vertices[i]);
					duplicateVerts[index].Add(i);
					continue;
				}
				int item = Array.IndexOf(mesh.vertices, mesh.vertices[i]);
				list.Add(mesh.vertices[i]);
				duplicateVerts.Add(new List<int> { item, i });
			}
		}
	}

	private void SetDuplicateVerts(ref Mesh mesh)
	{
		Vector3[] vertices = mesh.vertices;
		foreach (List<int> duplicateVert in duplicateVerts)
		{
			Vector3 zero = Vector3.zero;
			foreach (int item in duplicateVert)
			{
				zero += vertices[item];
			}
			zero /= (float)duplicateVert.Count;
			foreach (int item2 in duplicateVert)
			{
				vertices[item2] = zero;
			}
		}
		mesh.vertices = vertices;
	}

	public void OnSaveModel()
	{
		string path = Application.dataPath + "/langs/en_us/transforms/" + textCtrl.text;
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		string contents;
		for (int i = 0; i < tongueVectors; i++)
		{
			contents = tongueTarget[i].transform.position.x + ", " + tongueTarget[i].transform.position.y + ", " + tongueTarget[i].transform.position.z + "\n";
			File.AppendAllText(path, contents);
		}
		for (int j = 0; j < lipVectors; j++)
		{
			contents = lipTarget[j].transform.position.x + ", " + lipTarget[j].transform.position.y + ", " + lipTarget[j].transform.position.z + "\n";
			File.AppendAllText(path, contents);
		}
		contents = lowerJawBone.transform.eulerAngles.x + ", " + lowerJawBone.transform.eulerAngles.y + ", " + lowerJawBone.transform.eulerAngles.z + "\n";
		File.AppendAllText(path, contents);
		contents = palate.transform.eulerAngles.x + ", " + palate.transform.eulerAngles.y + ", " + palate.transform.eulerAngles.z + "\n";
		File.AppendAllText(path, contents);
		contents = uvula.transform.eulerAngles.x + ", " + uvula.transform.eulerAngles.y + ", " + uvula.transform.eulerAngles.z + "\n";
		File.AppendAllText(path, contents);
		for (int k = 0; k < airflowVectors; k++)
		{
			contents = airflowTarget[k].transform.position.x + ", " + airflowTarget[k].transform.position.y + ", " + airflowTarget[k].transform.position.z + "\n";
			File.AppendAllText(path, contents);
		}
	}

	public void OnLoadModel()
	{
		string path = Application.dataPath + "/langs/en_us/transforms/" + textCtrl.text;
		if (File.Exists(path))
		{
			string[] array = File.ReadAllLines(path);
			int i;
			float[] array2;
			for (i = 0; i < tongueVectors; i++)
			{
				array2 = array[i].Split(',').Select(Convert.ToSingle).ToArray();
				tongueTarget[i].transform.position = new Vector3(array2[0], array2[1], array2[2]);
			}
			for (int j = 0; j < lipVectors; j++)
			{
				array2 = array[i++].Split(',').Select(Convert.ToSingle).ToArray();
				lipTarget[j].transform.position = new Vector3(array2[0], array2[1], array2[2]);
			}
			array2 = array[i++].Split(',').Select(Convert.ToSingle).ToArray();
			lowerJawBone.transform.eulerAngles = new Vector3(array2[0], array2[1], array2[2]);
			array2 = array[i++].Split(',').Select(Convert.ToSingle).ToArray();
			palate.transform.eulerAngles = new Vector3(array2[0], array2[1], array2[2]);
			array2 = array[i++].Split(',').Select(Convert.ToSingle).ToArray();
			uvula.transform.eulerAngles = new Vector3(array2[0], array2[1], array2[2]);
			for (int k = 0; k < airflowVectors; k++)
			{
				array2 = array[i++].Split(',').Select(Convert.ToSingle).ToArray();
				airflowTarget[k].transform.position = new Vector3(array2[0], array2[1], array2[2]);
			}
			defaultJawPos = lowerJawBone.transform.rotation;
		}
	}
}
