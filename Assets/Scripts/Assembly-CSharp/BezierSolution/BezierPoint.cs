using System;
using System.Diagnostics;
using UnityEngine;

namespace BezierSolution
{
	public class BezierPoint : MonoBehaviour
	{
		[Serializable]
		public struct ExtraData
		{
			public float c1;

			public float c2;

			public float c3;

			public float c4;

			public ExtraData(float c1 = 0f, float c2 = 0f, float c3 = 0f, float c4 = 0f)
			{
				this.c1 = c1;
				this.c2 = c2;
				this.c3 = c3;
				this.c4 = c4;
			}

			public static ExtraData Lerp(ExtraData a, ExtraData b, float t)
			{
				t = Mathf.Clamp01(t);
				return new ExtraData(a.c1 + (b.c1 - a.c1) * t, a.c2 + (b.c2 - a.c2) * t, a.c3 + (b.c3 - a.c3) * t, a.c4 + (b.c4 - a.c4) * t);
			}

			public static ExtraData LerpUnclamped(ExtraData a, ExtraData b, float t)
			{
				return new ExtraData(a.c1 + (b.c1 - a.c1) * t, a.c2 + (b.c2 - a.c2) * t, a.c3 + (b.c3 - a.c3) * t, a.c4 + (b.c4 - a.c4) * t);
			}

			public static implicit operator ExtraData(Vector2 v)
			{
				return new ExtraData(v.x, v.y);
			}

			public static implicit operator ExtraData(Vector3 v)
			{
				return new ExtraData(v.x, v.y, v.z);
			}

			public static implicit operator ExtraData(Vector4 v)
			{
				return new ExtraData(v.x, v.y, v.z, v.w);
			}

			public static implicit operator ExtraData(Quaternion q)
			{
				return new ExtraData(q.x, q.y, q.z, q.w);
			}

			public static implicit operator ExtraData(Rect r)
			{
				return new ExtraData(r.xMin, r.yMin, r.width, r.height);
			}

			public static implicit operator ExtraData(Vector2Int v)
			{
				return new ExtraData(v.x, v.y);
			}

			public static implicit operator ExtraData(Vector3Int v)
			{
				return new ExtraData(v.x, v.y, v.z);
			}

			public static implicit operator ExtraData(RectInt r)
			{
				return new ExtraData(r.xMin, r.yMin, r.width, r.height);
			}

			public static implicit operator Vector2(ExtraData v)
			{
				return new Vector2(v.c1, v.c2);
			}

			public static implicit operator Vector3(ExtraData v)
			{
				return new Vector3(v.c1, v.c2, v.c3);
			}

			public static implicit operator Vector4(ExtraData v)
			{
				return new Vector4(v.c1, v.c2, v.c3, v.c4);
			}

			public static implicit operator Quaternion(ExtraData v)
			{
				return new Quaternion(v.c1, v.c2, v.c3, v.c4);
			}

			public static implicit operator Rect(ExtraData v)
			{
				return new Rect(v.c1, v.c2, v.c3, v.c4);
			}

			public static implicit operator Vector2Int(ExtraData v)
			{
				return new Vector2Int(Mathf.RoundToInt(v.c1), Mathf.RoundToInt(v.c2));
			}

			public static implicit operator Vector3Int(ExtraData v)
			{
				return new Vector3Int(Mathf.RoundToInt(v.c1), Mathf.RoundToInt(v.c2), Mathf.RoundToInt(v.c3));
			}

			public static implicit operator RectInt(ExtraData v)
			{
				return new RectInt(Mathf.RoundToInt(v.c1), Mathf.RoundToInt(v.c2), Mathf.RoundToInt(v.c3), Mathf.RoundToInt(v.c4));
			}

			public static bool operator ==(ExtraData d1, ExtraData d2)
			{
				if (d1.c1 == d2.c1 && d1.c2 == d2.c2 && d1.c3 == d2.c3)
				{
					return d1.c4 == d2.c4;
				}
				return false;
			}

			public static bool operator !=(ExtraData d1, ExtraData d2)
			{
				if (d1.c1 == d2.c1 && d1.c2 == d2.c2 && d1.c3 == d2.c3)
				{
					return d1.c4 != d2.c4;
				}
				return true;
			}

			public override bool Equals(object obj)
			{
				if (obj is ExtraData)
				{
					return this == (ExtraData)obj;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (int)((((391f + c1) * 23f + c2) * 23f + c3) * 23f + c4);
			}

			public override string ToString()
			{
				return ((Vector4)this/*cast due to .constrained prefix*/).ToString();
			}
		}

		public enum HandleMode
		{
			Free = 0,
			Aligned = 1,
			Mirrored = 2
		}

		[SerializeField]
		[HideInInspector]
		private Vector3 m_position;

		[SerializeField]
		[HideInInspector]
		private Vector3 m_precedingControlPointLocalPosition = Vector3.left;

		[SerializeField]
		[HideInInspector]
		private Vector3 m_precedingControlPointPosition;

		[SerializeField]
		[HideInInspector]
		private Vector3 m_followingControlPointLocalPosition = Vector3.right;

		[SerializeField]
		[HideInInspector]
		private Vector3 m_followingControlPointPosition;

		[SerializeField]
		[HideInInspector]
		private HandleMode m_handleMode = HandleMode.Mirrored;

		[HideInInspector]
		public ExtraData extraData;

		public Vector3 localPosition
		{
			get
			{
				return base.transform.localPosition;
			}
			set
			{
				base.transform.localPosition = value;
			}
		}

		public Vector3 position
		{
			get
			{
				if (base.transform.hasChanged)
				{
					Revalidate();
				}
				return m_position;
			}
			set
			{
				base.transform.position = value;
			}
		}

		public Quaternion localRotation
		{
			get
			{
				return base.transform.localRotation;
			}
			set
			{
				base.transform.localRotation = value;
			}
		}

		public Quaternion rotation
		{
			get
			{
				return base.transform.rotation;
			}
			set
			{
				base.transform.rotation = value;
			}
		}

		public Vector3 localEulerAngles
		{
			get
			{
				return base.transform.localEulerAngles;
			}
			set
			{
				base.transform.localEulerAngles = value;
			}
		}

		public Vector3 eulerAngles
		{
			get
			{
				return base.transform.eulerAngles;
			}
			set
			{
				base.transform.eulerAngles = value;
			}
		}

		public Vector3 localScale
		{
			get
			{
				return base.transform.localScale;
			}
			set
			{
				base.transform.localScale = value;
			}
		}

		public Vector3 precedingControlPointLocalPosition
		{
			get
			{
				return m_precedingControlPointLocalPosition;
			}
			set
			{
				m_precedingControlPointLocalPosition = value;
				m_precedingControlPointPosition = base.transform.TransformPoint(value);
				if (m_handleMode == HandleMode.Aligned)
				{
					m_followingControlPointLocalPosition = -m_precedingControlPointLocalPosition.normalized * m_followingControlPointLocalPosition.magnitude;
					m_followingControlPointPosition = base.transform.TransformPoint(m_followingControlPointLocalPosition);
				}
				else if (m_handleMode == HandleMode.Mirrored)
				{
					m_followingControlPointLocalPosition = -m_precedingControlPointLocalPosition;
					m_followingControlPointPosition = base.transform.TransformPoint(m_followingControlPointLocalPosition);
				}
			}
		}

		public Vector3 precedingControlPointPosition
		{
			get
			{
				if (base.transform.hasChanged)
				{
					Revalidate();
				}
				return m_precedingControlPointPosition;
			}
			set
			{
				m_precedingControlPointPosition = value;
				m_precedingControlPointLocalPosition = base.transform.InverseTransformPoint(value);
				if (base.transform.hasChanged)
				{
					m_position = base.transform.position;
					base.transform.hasChanged = false;
				}
				if (m_handleMode == HandleMode.Aligned)
				{
					m_followingControlPointPosition = m_position - (m_precedingControlPointPosition - m_position).normalized * (m_followingControlPointPosition - m_position).magnitude;
					m_followingControlPointLocalPosition = base.transform.InverseTransformPoint(m_followingControlPointPosition);
				}
				else if (m_handleMode == HandleMode.Mirrored)
				{
					m_followingControlPointPosition = 2f * m_position - m_precedingControlPointPosition;
					m_followingControlPointLocalPosition = base.transform.InverseTransformPoint(m_followingControlPointPosition);
				}
			}
		}

		public Vector3 followingControlPointLocalPosition
		{
			get
			{
				return m_followingControlPointLocalPosition;
			}
			set
			{
				m_followingControlPointLocalPosition = value;
				m_followingControlPointPosition = base.transform.TransformPoint(value);
				if (m_handleMode == HandleMode.Aligned)
				{
					m_precedingControlPointLocalPosition = -m_followingControlPointLocalPosition.normalized * m_precedingControlPointLocalPosition.magnitude;
					m_precedingControlPointPosition = base.transform.TransformPoint(m_precedingControlPointLocalPosition);
				}
				else if (m_handleMode == HandleMode.Mirrored)
				{
					m_precedingControlPointLocalPosition = -m_followingControlPointLocalPosition;
					m_precedingControlPointPosition = base.transform.TransformPoint(m_precedingControlPointLocalPosition);
				}
			}
		}

		public Vector3 followingControlPointPosition
		{
			get
			{
				if (base.transform.hasChanged)
				{
					Revalidate();
				}
				return m_followingControlPointPosition;
			}
			set
			{
				m_followingControlPointPosition = value;
				m_followingControlPointLocalPosition = base.transform.InverseTransformPoint(value);
				if (base.transform.hasChanged)
				{
					m_position = base.transform.position;
					base.transform.hasChanged = false;
				}
				if (m_handleMode == HandleMode.Aligned)
				{
					m_precedingControlPointPosition = m_position - (m_followingControlPointPosition - m_position).normalized * (m_precedingControlPointPosition - m_position).magnitude;
					m_precedingControlPointLocalPosition = base.transform.InverseTransformPoint(m_precedingControlPointPosition);
				}
				else if (m_handleMode == HandleMode.Mirrored)
				{
					m_precedingControlPointPosition = 2f * m_position - m_followingControlPointPosition;
					m_precedingControlPointLocalPosition = base.transform.InverseTransformPoint(m_precedingControlPointPosition);
				}
			}
		}

		public HandleMode handleMode
		{
			get
			{
				return m_handleMode;
			}
			set
			{
				m_handleMode = value;
				if (value == HandleMode.Aligned || value == HandleMode.Mirrored)
				{
					precedingControlPointLocalPosition = m_precedingControlPointLocalPosition;
				}
			}
		}

		private void Awake()
		{
			base.transform.hasChanged = true;
		}

		public void CopyTo(BezierPoint other)
		{
			other.transform.localPosition = base.transform.localPosition;
			other.transform.localRotation = base.transform.localRotation;
			other.transform.localScale = base.transform.localScale;
			other.m_handleMode = m_handleMode;
			other.m_precedingControlPointLocalPosition = m_precedingControlPointLocalPosition;
			other.m_followingControlPointLocalPosition = m_followingControlPointLocalPosition;
		}

		private void Revalidate()
		{
			m_position = base.transform.position;
			m_precedingControlPointPosition = base.transform.TransformPoint(m_precedingControlPointLocalPosition);
			m_followingControlPointPosition = base.transform.TransformPoint(m_followingControlPointLocalPosition);
			base.transform.hasChanged = false;
		}

		[Conditional("UNITY_EDITOR")]
		private void SetSplineDirty()
		{
		}

		public void Reset()
		{
			localPosition = Vector3.zero;
			localRotation = Quaternion.identity;
			localScale = Vector3.one;
			precedingControlPointLocalPosition = Vector3.left;
			followingControlPointLocalPosition = Vector3.right;
			base.transform.hasChanged = true;
		}
	}
}
