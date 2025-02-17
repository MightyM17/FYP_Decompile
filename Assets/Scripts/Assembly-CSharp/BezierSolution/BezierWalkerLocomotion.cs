using System;
using System.Collections.Generic;
using UnityEngine;

namespace BezierSolution
{
	public class BezierWalkerLocomotion : BezierWalker
	{
		public BezierWalker walker;

		[SerializeField]
		private List<Transform> tailObjects;

		[SerializeField]
		private List<float> tailObjectDistances;

		public float movementLerpModifier = 10f;

		public float rotationLerpModifier = 10f;

		[NonSerialized]
		[Obsolete("Use lookAt instead", true)]
		public bool lookForward = true;

		public LookAtMode lookAt = LookAtMode.Forward;

		public List<Transform> Tail => tailObjects;

		public List<float> TailDistances => tailObjectDistances;

		public override BezierSpline Spline => walker.Spline;

		public override bool MovingForward => walker.MovingForward;

		public override float NormalizedT
		{
			get
			{
				return walker.NormalizedT;
			}
			set
			{
				walker.NormalizedT = value;
			}
		}

		private void Start()
		{
			if (!walker)
			{
				Debug.LogError("Need to attach BezierWalkerLocomotion to a BezierWalker!");
				UnityEngine.Object.Destroy(this);
			}
			if (tailObjects.Count != tailObjectDistances.Count)
			{
				Debug.LogError("One distance per tail object is needed!");
				UnityEngine.Object.Destroy(this);
			}
		}

		private void LateUpdate()
		{
			Execute(Time.deltaTime);
		}

		public override void Execute(float deltaTime)
		{
			float normalizedT = NormalizedT;
			BezierSpline spline = Spline;
			bool movingForward = MovingForward;
			for (int i = 0; i < tailObjects.Count; i++)
			{
				Transform transform = tailObjects[i];
				if (movingForward)
				{
					transform.position = Vector3.Lerp(transform.position, spline.MoveAlongSpline(ref normalizedT, 0f - tailObjectDistances[i]), movementLerpModifier * deltaTime);
					if (lookAt == LookAtMode.Forward)
					{
						transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(spline.GetTangent(normalizedT)), rotationLerpModifier * deltaTime);
					}
					else if (lookAt == LookAtMode.SplineExtraData)
					{
						transform.rotation = Quaternion.Lerp(transform.rotation, spline.GetExtraData(normalizedT, BezierWalker.extraDataLerpAsQuaternionFunction), rotationLerpModifier * deltaTime);
					}
				}
				else
				{
					transform.position = Vector3.Lerp(transform.position, spline.MoveAlongSpline(ref normalizedT, tailObjectDistances[i]), movementLerpModifier * deltaTime);
					if (lookAt == LookAtMode.Forward)
					{
						transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-spline.GetTangent(normalizedT)), rotationLerpModifier * deltaTime);
					}
					else if (lookAt == LookAtMode.SplineExtraData)
					{
						transform.rotation = Quaternion.Lerp(transform.rotation, spline.GetExtraData(normalizedT, BezierWalker.extraDataLerpAsQuaternionFunction), rotationLerpModifier * deltaTime);
					}
				}
			}
		}

		public void AddToTail(Transform transform, float distanceToPreviousObject)
		{
			if (transform == null)
			{
				Debug.LogError("Object is null!");
				return;
			}
			tailObjects.Add(transform);
			tailObjectDistances.Add(distanceToPreviousObject);
		}

		public void InsertIntoTail(int index, Transform transform, float distanceToPreviousObject)
		{
			if (transform == null)
			{
				Debug.LogError("Object is null!");
				return;
			}
			tailObjects.Insert(index, transform);
			tailObjectDistances.Insert(index, distanceToPreviousObject);
		}

		public void RemoveFromTail(Transform transform)
		{
			if (transform == null)
			{
				Debug.LogError("Object is null!");
				return;
			}
			for (int i = 0; i < tailObjects.Count; i++)
			{
				if (tailObjects[i] == transform)
				{
					tailObjects.RemoveAt(i);
					tailObjectDistances.RemoveAt(i);
					break;
				}
			}
		}
	}
}
