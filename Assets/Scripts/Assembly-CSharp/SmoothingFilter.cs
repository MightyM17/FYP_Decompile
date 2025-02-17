using System;
using System.Collections.Generic;
using UnityEngine;

public class SmoothingFilter
{
	public static Mesh LaplacianFilter(Mesh mesh, int times = 1)
	{
		mesh.vertices = LaplacianFilter(mesh.vertices, mesh.triangles, times);
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	public static Vector3[] LaplacianFilter(Vector3[] vertices, int[] triangles, int times)
	{
		Dictionary<int, VertexConnection> network = VertexConnection.BuildNetwork(triangles);
		for (int i = 0; i < times; i++)
		{
			vertices = LaplacianFilter(network, vertices, triangles);
		}
		return vertices;
	}

	private static Vector3[] LaplacianFilter(Dictionary<int, VertexConnection> network, Vector3[] origin, int[] triangles)
	{
		Vector3[] array = new Vector3[origin.Length];
		int i = 0;
		for (int num = origin.Length; i < num; i++)
		{
			HashSet<int> connection = network[i].Connection;
			Vector3 zero = Vector3.zero;
			foreach (int item in connection)
			{
				zero += origin[item];
			}
			array[i] = zero / connection.Count;
		}
		return array;
	}

	public static Mesh HCFilter(Mesh mesh, int times = 5, float alpha = 0.5f, float beta = 0.75f)
	{
		mesh.vertices = HCFilter(mesh.vertices, mesh.triangles, times, alpha, beta);
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	private static Vector3[] HCFilter(Vector3[] vertices, int[] triangles, int times, float alpha, float beta)
	{
		alpha = Mathf.Clamp01(alpha);
		beta = Mathf.Clamp01(beta);
		Dictionary<int, VertexConnection> network = VertexConnection.BuildNetwork(triangles);
		Vector3[] array = new Vector3[vertices.Length];
		Array.Copy(vertices, array, vertices.Length);
		for (int i = 0; i < times; i++)
		{
			vertices = HCFilter(network, array, vertices, triangles, alpha, beta);
		}
		return vertices;
	}

	public static Vector3[] HCFilter(Dictionary<int, VertexConnection> network, Vector3[] o, Vector3[] q, int[] triangles, float alpha, float beta)
	{
		Vector3[] array = LaplacianFilter(network, q, triangles);
		Vector3[] array2 = new Vector3[o.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = array[i] - (alpha * o[i] + (1f - alpha) * q[i]);
		}
		for (int j = 0; j < array.Length; j++)
		{
			HashSet<int> connection = network[j].Connection;
			Vector3 zero = Vector3.zero;
			foreach (int item in connection)
			{
				zero += array2[item];
			}
			array[j] -= beta * array2[j] + (1f - beta) / (float)connection.Count * zero;
		}
		return array;
	}
}
