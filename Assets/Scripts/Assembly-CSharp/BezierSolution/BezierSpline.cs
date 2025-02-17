using System.Collections.Generic;
using UnityEngine;

namespace BezierSolution
{
	[ExecuteInEditMode]
	public class BezierSpline : MonoBehaviour
	{
		public struct PointIndexTuple
		{
			public readonly int index1;

			public readonly int index2;

			public readonly float t;

			public PointIndexTuple(int index1, int index2, float t)
			{
				this.index1 = index1;
				this.index2 = index2;
				this.t = t;
			}
		}

		public delegate BezierPoint.ExtraData ExtraDataLerpFunction(BezierPoint.ExtraData data1, BezierPoint.ExtraData data2, float normalizedT);

		private static readonly ExtraDataLerpFunction defaultExtraDataLerpFunction = BezierPoint.ExtraData.LerpUnclamped;

		private static Material gizmoMaterial;

		private List<BezierPoint> endPoints = new List<BezierPoint>();

		public bool loop;

		public bool drawGizmos;

		public Color gizmoColor = Color.white;

		private float gizmoStep = 0.05f;

		[SerializeField]
		private int m_gizmoSmoothness = 4;

		public int gizmoSmoothness
		{
			get
			{
				return m_gizmoSmoothness;
			}
			set
			{
				m_gizmoSmoothness = value;
				gizmoStep = 1f / (float)(endPoints.Count * Mathf.Clamp(m_gizmoSmoothness, 1, 30));
			}
		}

		public int Count => endPoints.Count;

		public float Length => GetLengthApproximately(0f, 1f);

		public BezierPoint this[int index]
		{
			get
			{
				if (index < Count)
				{
					return endPoints[index];
				}
				Debug.LogError("Bezier index " + index + " is out of range: " + Count);
				return null;
			}
		}

		private void Awake()
		{
			Refresh();
		}

		public void Initialize(int endPointsCount)
		{
			if (endPointsCount < 2)
			{
				Debug.LogError("Can't initialize spline with " + endPointsCount + " point(s). At least 2 points are needed");
				return;
			}
			Refresh();
			for (int num = endPoints.Count - 1; num >= 0; num--)
			{
				Object.DestroyImmediate(endPoints[num].gameObject);
			}
			endPoints.Clear();
			for (int i = 0; i < endPointsCount; i++)
			{
				InsertNewPointAt(i);
			}
			Refresh();
		}

		public void Refresh()
		{
			endPoints.Clear();
			GetComponentsInChildren(endPoints);
			gizmoSmoothness = gizmoSmoothness;
		}

		public BezierPoint InsertNewPointAt(int index)
		{
			if (index < 0 || index > endPoints.Count)
			{
				Debug.LogError("Index " + index + " is out of range: [0," + endPoints.Count + "]");
				return null;
			}
			int count = endPoints.Count;
			BezierPoint bezierPoint = new GameObject("Point").AddComponent<BezierPoint>();
			Transform parent = ((endPoints.Count == 0) ? base.transform : ((index == 0) ? endPoints[0].transform.parent : endPoints[index - 1].transform.parent));
			int siblingIndex = ((index != 0) ? (endPoints[index - 1].transform.GetSiblingIndex() + 1) : 0);
			bezierPoint.transform.SetParent(parent, worldPositionStays: false);
			bezierPoint.transform.SetSiblingIndex(siblingIndex);
			if (endPoints.Count == count)
			{
				endPoints.Insert(index, bezierPoint);
			}
			return bezierPoint;
		}

		public BezierPoint DuplicatePointAt(int index)
		{
			if (index < 0 || index >= endPoints.Count)
			{
				Debug.LogError("Index " + index + " is out of range: [0," + (endPoints.Count - 1) + "]");
				return null;
			}
			BezierPoint bezierPoint = InsertNewPointAt(index + 1);
			endPoints[index].CopyTo(bezierPoint);
			return bezierPoint;
		}

		public void RemovePointAt(int index)
		{
			if (endPoints.Count <= 2)
			{
				Debug.LogError("Can't remove point: spline must consist of at least two points!");
			}
			else if (index < 0 || index >= endPoints.Count)
			{
				Debug.LogError("Index " + index + " is out of range: [0," + endPoints.Count + ")");
			}
			else
			{
				BezierPoint bezierPoint = endPoints[index];
				endPoints.RemoveAt(index);
				Object.DestroyImmediate(bezierPoint.gameObject);
			}
		}

		public void SwapPointsAt(int index1, int index2)
		{
			if (index1 == index2)
			{
				return;
			}
			if (index1 < 0 || index1 >= endPoints.Count || index2 < 0 || index2 >= endPoints.Count)
			{
				Debug.LogError("Indices must be in range [0," + (endPoints.Count - 1) + "]");
				return;
			}
			BezierPoint bezierPoint = endPoints[index1];
			BezierPoint bezierPoint2 = endPoints[index2];
			int siblingIndex = bezierPoint.transform.GetSiblingIndex();
			int siblingIndex2 = bezierPoint2.transform.GetSiblingIndex();
			Transform parent = bezierPoint.transform.parent;
			Transform parent2 = bezierPoint2.transform.parent;
			endPoints[index1] = bezierPoint2;
			endPoints[index2] = bezierPoint;
			if (parent != parent2)
			{
				bezierPoint.transform.SetParent(parent2, worldPositionStays: true);
				bezierPoint2.transform.SetParent(parent, worldPositionStays: true);
			}
			bezierPoint.transform.SetSiblingIndex(siblingIndex2);
			bezierPoint2.transform.SetSiblingIndex(siblingIndex);
		}

		public void MovePoint(int previousIndex, int newIndex)
		{
			Internal_MovePoint(previousIndex, newIndex, null);
		}

		public void Internal_MovePoint(int previousIndex, int newIndex, string undo)
		{
			if (previousIndex == newIndex)
			{
				return;
			}
			if (previousIndex < 0 || previousIndex >= endPoints.Count || newIndex < 0 || newIndex >= endPoints.Count)
			{
				Debug.LogError("Indices must be in range [0," + (endPoints.Count - 1) + "]");
				return;
			}
			BezierPoint bezierPoint = endPoints[previousIndex];
			BezierPoint bezierPoint2 = endPoints[newIndex];
			if (previousIndex < newIndex)
			{
				for (int i = previousIndex; i < newIndex; i++)
				{
					endPoints[i] = endPoints[i + 1];
				}
			}
			else
			{
				for (int num = previousIndex; num > newIndex; num--)
				{
					endPoints[num] = endPoints[num - 1];
				}
			}
			endPoints[newIndex] = bezierPoint;
			Transform parent = bezierPoint2.transform.parent;
			if (bezierPoint.transform.parent != parent)
			{
				bezierPoint.transform.SetParent(parent, worldPositionStays: true);
				int siblingIndex = bezierPoint2.transform.GetSiblingIndex();
				if (previousIndex < newIndex)
				{
					if (bezierPoint.transform.GetSiblingIndex() < siblingIndex)
					{
						bezierPoint.transform.SetSiblingIndex(siblingIndex);
					}
					else
					{
						bezierPoint.transform.SetSiblingIndex(siblingIndex + 1);
					}
				}
				else if (bezierPoint.transform.GetSiblingIndex() < siblingIndex)
				{
					bezierPoint.transform.SetSiblingIndex(siblingIndex - 1);
				}
				else
				{
					bezierPoint.transform.SetSiblingIndex(siblingIndex);
				}
			}
			else
			{
				bezierPoint.transform.SetSiblingIndex(bezierPoint2.transform.GetSiblingIndex());
			}
		}

		public int IndexOf(BezierPoint point)
		{
			return endPoints.IndexOf(point);
		}

		public Vector3 GetPoint(float normalizedT)
		{
			if (!loop)
			{
				if (normalizedT <= 0f)
				{
					return endPoints[0].position;
				}
				if (normalizedT >= 1f)
				{
					return endPoints[endPoints.Count - 1].position;
				}
			}
			else if (normalizedT < 0f)
			{
				normalizedT += 1f;
			}
			else if (normalizedT >= 1f)
			{
				normalizedT -= 1f;
			}
			float num = normalizedT * (float)(loop ? endPoints.Count : (endPoints.Count - 1));
			int num2 = (int)num;
			int num3 = num2 + 1;
			if (num3 == endPoints.Count)
			{
				num3 = 0;
			}
			BezierPoint bezierPoint = endPoints[num2];
			BezierPoint bezierPoint2 = endPoints[num3];
			float num4 = num - (float)num2;
			float num5 = 1f - num4;
			return num5 * num5 * num5 * bezierPoint.position + 3f * num5 * num5 * num4 * bezierPoint.followingControlPointPosition + 3f * num5 * num4 * num4 * bezierPoint2.precedingControlPointPosition + num4 * num4 * num4 * bezierPoint2.position;
		}

		public Vector3 GetTangent(float normalizedT)
		{
			if (!loop)
			{
				if (normalizedT <= 0f)
				{
					return 3f * (endPoints[0].followingControlPointPosition - endPoints[0].position);
				}
				if (normalizedT >= 1f)
				{
					int index = endPoints.Count - 1;
					return 3f * (endPoints[index].position - endPoints[index].precedingControlPointPosition);
				}
			}
			else if (normalizedT < 0f)
			{
				normalizedT += 1f;
			}
			else if (normalizedT >= 1f)
			{
				normalizedT -= 1f;
			}
			float num = normalizedT * (float)(loop ? endPoints.Count : (endPoints.Count - 1));
			int num2 = (int)num;
			int num3 = num2 + 1;
			if (num3 == endPoints.Count)
			{
				num3 = 0;
			}
			BezierPoint bezierPoint = endPoints[num2];
			BezierPoint bezierPoint2 = endPoints[num3];
			float num4 = num - (float)num2;
			float num5 = 1f - num4;
			return 3f * num5 * num5 * (bezierPoint.followingControlPointPosition - bezierPoint.position) + 6f * num5 * num4 * (bezierPoint2.precedingControlPointPosition - bezierPoint.followingControlPointPosition) + 3f * num4 * num4 * (bezierPoint2.position - bezierPoint2.precedingControlPointPosition);
		}

		public BezierPoint.ExtraData GetExtraData(float normalizedT)
		{
			return GetExtraData(normalizedT, defaultExtraDataLerpFunction);
		}

		public BezierPoint.ExtraData GetExtraData(float normalizedT, ExtraDataLerpFunction lerpFunction)
		{
			if (!loop)
			{
				if (normalizedT <= 0f)
				{
					return endPoints[0].extraData;
				}
				if (normalizedT >= 1f)
				{
					return endPoints[endPoints.Count - 1].extraData;
				}
			}
			else if (normalizedT < 0f)
			{
				normalizedT += 1f;
			}
			else if (normalizedT >= 1f)
			{
				normalizedT -= 1f;
			}
			float num = normalizedT * (float)(loop ? endPoints.Count : (endPoints.Count - 1));
			int num2 = (int)num;
			int num3 = num2 + 1;
			if (num3 == endPoints.Count)
			{
				num3 = 0;
			}
			return lerpFunction(endPoints[num2].extraData, endPoints[num3].extraData, num - (float)num2);
		}

		public float GetLengthApproximately(float startNormalizedT, float endNormalizedT, float accuracy = 50f)
		{
			if (endNormalizedT < startNormalizedT)
			{
				float num = startNormalizedT;
				startNormalizedT = endNormalizedT;
				endNormalizedT = num;
			}
			if (startNormalizedT < 0f)
			{
				startNormalizedT = 0f;
			}
			if (endNormalizedT > 1f)
			{
				endNormalizedT = 1f;
			}
			float num2 = AccuracyToStepSize(accuracy) * (endNormalizedT - startNormalizedT);
			float num3 = 0f;
			Vector3 vector = GetPoint(startNormalizedT);
			for (float num4 = startNormalizedT + num2; num4 < endNormalizedT; num4 += num2)
			{
				Vector3 point = GetPoint(num4);
				num3 += Vector3.Distance(point, vector);
				vector = point;
			}
			return num3 + Vector3.Distance(vector, GetPoint(endNormalizedT));
		}

		public PointIndexTuple GetNearestPointIndicesTo(float normalizedT)
		{
			if (!loop)
			{
				if (normalizedT <= 0f)
				{
					return new PointIndexTuple(0, 1, 0f);
				}
				if (normalizedT >= 1f)
				{
					return new PointIndexTuple(endPoints.Count - 1, endPoints.Count - 1, 1f);
				}
			}
			else if (normalizedT < 0f)
			{
				normalizedT += 1f;
			}
			else if (normalizedT >= 1f)
			{
				normalizedT -= 1f;
			}
			float num = normalizedT * (float)(loop ? endPoints.Count : (endPoints.Count - 1));
			int num2 = (int)num;
			int num3 = num2 + 1;
			if (num3 == endPoints.Count)
			{
				num3 = 0;
			}
			return new PointIndexTuple(num2, num3, num - (float)num2);
		}

		public Vector3 FindNearestPointTo(Vector3 worldPos, float accuracy = 100f)
		{
			float normalizedT;
			return FindNearestPointTo(worldPos, out normalizedT, accuracy);
		}

		public Vector3 FindNearestPointTo(Vector3 worldPos, out float normalizedT, float accuracy = 100f)
		{
			Vector3 result = Vector3.zero;
			normalizedT = -1f;
			float num = AccuracyToStepSize(accuracy);
			float num2 = float.PositiveInfinity;
			for (float num3 = 0f; num3 < 1f; num3 += num)
			{
				Vector3 point = GetPoint(num3);
				float sqrMagnitude = (worldPos - point).sqrMagnitude;
				if (sqrMagnitude < num2)
				{
					num2 = sqrMagnitude;
					result = point;
					normalizedT = num3;
				}
			}
			return result;
		}

		public Vector3 MoveAlongSpline(ref float normalizedT, float deltaMovement, int accuracy = 3)
		{
			float num = deltaMovement / (float)((loop ? endPoints.Count : (endPoints.Count - 1)) * accuracy);
			for (int i = 0; i < accuracy; i++)
			{
				normalizedT += num / GetTangent(normalizedT).magnitude;
			}
			return GetPoint(normalizedT);
		}

		public void ConstructLinearPath()
		{
			for (int i = 0; i < endPoints.Count; i++)
			{
				endPoints[i].handleMode = BezierPoint.HandleMode.Free;
				if (i < endPoints.Count - 1)
				{
					Vector3 vector = (endPoints[i].position + endPoints[i + 1].position) * 0.5f;
					endPoints[i].followingControlPointPosition = vector;
					endPoints[i + 1].precedingControlPointPosition = vector;
				}
				else
				{
					Vector3 vector2 = (endPoints[i].position + endPoints[0].position) * 0.5f;
					endPoints[i].followingControlPointPosition = vector2;
					endPoints[0].precedingControlPointPosition = vector2;
				}
			}
		}

		public void AutoConstructSpline()
		{
			for (int i = 0; i < endPoints.Count; i++)
			{
				endPoints[i].handleMode = BezierPoint.HandleMode.Mirrored;
			}
			int num = endPoints.Count - 1;
			if (num == 1)
			{
				endPoints[0].followingControlPointPosition = (2f * endPoints[0].position + endPoints[1].position) / 3f;
				endPoints[1].precedingControlPointPosition = 2f * endPoints[0].followingControlPointPosition - endPoints[0].position;
				return;
			}
			Vector3[] array = ((!loop) ? new Vector3[num] : new Vector3[num + 1]);
			for (int j = 1; j < num - 1; j++)
			{
				array[j] = 4f * endPoints[j].position + 2f * endPoints[j + 1].position;
			}
			array[0] = endPoints[0].position + 2f * endPoints[1].position;
			if (!loop)
			{
				array[num - 1] = (8f * endPoints[num - 1].position + endPoints[num].position) * 0.5f;
			}
			else
			{
				array[num - 1] = 4f * endPoints[num - 1].position + 2f * endPoints[num].position;
				array[num] = (8f * endPoints[num].position + endPoints[0].position) * 0.5f;
			}
			Vector3[] firstControlPoints = GetFirstControlPoints(array);
			for (int k = 0; k < num; k++)
			{
				endPoints[k].followingControlPointPosition = firstControlPoints[k];
				if (loop)
				{
					endPoints[k + 1].precedingControlPointPosition = 2f * endPoints[k + 1].position - firstControlPoints[k + 1];
				}
				else if (k < num - 1)
				{
					endPoints[k + 1].precedingControlPointPosition = 2f * endPoints[k + 1].position - firstControlPoints[k + 1];
				}
				else
				{
					endPoints[k + 1].precedingControlPointPosition = (endPoints[num].position + firstControlPoints[num - 1]) * 0.5f;
				}
			}
			if (loop)
			{
				float num2 = Vector3.Distance(endPoints[0].followingControlPointPosition, endPoints[0].position);
				Vector3 vector = Vector3.Normalize(endPoints[num].position - endPoints[1].position);
				endPoints[0].precedingControlPointPosition = endPoints[0].position + vector * num2;
				endPoints[0].followingControlPointLocalPosition = -endPoints[0].precedingControlPointLocalPosition;
			}
		}

		private static Vector3[] GetFirstControlPoints(Vector3[] rhs)
		{
			int num = rhs.Length;
			Vector3[] array = new Vector3[num];
			float[] array2 = new float[num];
			float num2 = 2f;
			array[0] = rhs[0] / num2;
			for (int i = 1; i < num; i++)
			{
				float num3 = (array2[i] = 1f / num2);
				num2 = ((i < num - 1) ? 4f : 3.5f) - num3;
				array[i] = (rhs[i] - array[i - 1]) / num2;
			}
			for (int j = 1; j < num; j++)
			{
				array[num - j - 1] -= array2[num - j] * array[num - j];
			}
			return array;
		}

		public void AutoConstructSpline2()
		{
			for (int i = 0; i < endPoints.Count; i++)
			{
				Vector3 vector = ((i != 0) ? endPoints[i - 1].position : ((!loop) ? endPoints[0].position : endPoints[endPoints.Count - 1].position));
				Vector3 position;
				Vector3 position2;
				if (loop)
				{
					position = endPoints[(i + 1) % endPoints.Count].position;
					position2 = endPoints[(i + 2) % endPoints.Count].position;
				}
				else if (i < endPoints.Count - 2)
				{
					position = endPoints[i + 1].position;
					position2 = endPoints[i + 2].position;
				}
				else if (i == endPoints.Count - 2)
				{
					position = endPoints[i + 1].position;
					position2 = endPoints[i + 1].position;
				}
				else
				{
					position = endPoints[i].position;
					position2 = endPoints[i].position;
				}
				endPoints[i].followingControlPointPosition = endPoints[i].position + (position - vector) / 6f;
				endPoints[i].handleMode = BezierPoint.HandleMode.Mirrored;
				if (i < endPoints.Count - 1)
				{
					endPoints[i + 1].precedingControlPointPosition = position - (position2 - endPoints[i].position) / 6f;
				}
				else if (loop)
				{
					endPoints[0].precedingControlPointPosition = position - (position2 - endPoints[i].position) / 6f;
				}
			}
		}

		private float AccuracyToStepSize(float accuracy)
		{
			if (accuracy <= 0f)
			{
				return 0.2f;
			}
			return Mathf.Clamp(1f / accuracy, 0.001f, 0.2f);
		}

		private void OnRenderObject()
		{
			if (drawGizmos && endPoints.Count >= 2)
			{
				if (!gizmoMaterial)
				{
					gizmoMaterial = new Material(Shader.Find("Hidden/Internal-Colored"))
					{
						hideFlags = HideFlags.HideAndDontSave
					};
					gizmoMaterial.SetInt("_SrcBlend", 5);
					gizmoMaterial.SetInt("_DstBlend", 10);
					gizmoMaterial.SetInt("_Cull", 0);
					gizmoMaterial.SetInt("_ZWrite", 0);
				}
				gizmoMaterial.SetPass(0);
				GL.Begin(1);
				GL.Color(gizmoColor);
				Vector3 vector = endPoints[0].position;
				for (float num = gizmoStep; num < 1f; num += gizmoStep)
				{
					GL.Vertex3(vector.x, vector.y, vector.z);
					vector = GetPoint(num);
					GL.Vertex3(vector.x, vector.y, vector.z);
				}
				GL.Vertex3(vector.x, vector.y, vector.z);
				vector = GetPoint(1f);
				GL.Vertex3(vector.x, vector.y, vector.z);
				GL.End();
			}
		}
	}
}
