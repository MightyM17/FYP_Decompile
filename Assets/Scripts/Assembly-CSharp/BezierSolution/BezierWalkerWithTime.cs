using System;
using UnityEngine;
using UnityEngine.Events;

namespace BezierSolution
{
	public class BezierWalkerWithTime : BezierWalker
	{
		public BezierSpline spline;

		public TravelMode travelMode;

		public float travelTime = 5f;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_normalizedT;

		public float movementLerpModifier = 10f;

		public float rotationLerpModifier = 10f;

		[NonSerialized]
		[Obsolete("Use lookAt instead", true)]
		public bool lookForward = true;

		public LookAtMode lookAt = LookAtMode.Forward;

		private bool isGoingForward = true;

		public UnityEvent onPathCompleted = new UnityEvent();

		private bool onPathCompletedCalledAt1;

		private bool onPathCompletedCalledAt0;

		public override BezierSpline Spline => spline;

		public override float NormalizedT
		{
			get
			{
				return m_normalizedT;
			}
			set
			{
				m_normalizedT = value;
			}
		}

		public override bool MovingForward => isGoingForward;

		private void Update()
		{
			Execute(Time.deltaTime);
		}

		public override void Execute(float deltaTime)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, spline.GetPoint(m_normalizedT), movementLerpModifier * deltaTime);
			if (lookAt == LookAtMode.Forward)
			{
				Quaternion b = ((!isGoingForward) ? Quaternion.LookRotation(-spline.GetTangent(m_normalizedT)) : Quaternion.LookRotation(spline.GetTangent(m_normalizedT)));
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, rotationLerpModifier * deltaTime);
			}
			else if (lookAt == LookAtMode.SplineExtraData)
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, spline.GetExtraData(m_normalizedT, BezierWalker.extraDataLerpAsQuaternionFunction), rotationLerpModifier * deltaTime);
			}
			if (isGoingForward)
			{
				m_normalizedT += deltaTime / travelTime;
				if (m_normalizedT > 1f)
				{
					if (travelMode == TravelMode.Once)
					{
						m_normalizedT = 1f;
					}
					else if (travelMode == TravelMode.Loop)
					{
						m_normalizedT -= 1f;
					}
					else
					{
						m_normalizedT = 2f - m_normalizedT;
						isGoingForward = false;
					}
					if (!onPathCompletedCalledAt1)
					{
						onPathCompletedCalledAt1 = true;
						onPathCompleted.Invoke();
					}
				}
				else
				{
					onPathCompletedCalledAt1 = false;
				}
				return;
			}
			m_normalizedT -= deltaTime / travelTime;
			if (m_normalizedT < 0f)
			{
				if (travelMode == TravelMode.Once)
				{
					m_normalizedT = 0f;
				}
				else if (travelMode == TravelMode.Loop)
				{
					m_normalizedT += 1f;
				}
				else
				{
					m_normalizedT = 0f - m_normalizedT;
					isGoingForward = true;
				}
				if (!onPathCompletedCalledAt0)
				{
					onPathCompletedCalledAt0 = true;
					onPathCompleted.Invoke();
				}
			}
			else
			{
				onPathCompletedCalledAt0 = false;
			}
		}
	}
}
