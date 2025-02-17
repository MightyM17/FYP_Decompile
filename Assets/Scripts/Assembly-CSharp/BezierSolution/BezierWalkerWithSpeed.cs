using System;
using UnityEngine;
using UnityEngine.Events;

namespace BezierSolution
{
	public class BezierWalkerWithSpeed : BezierWalker
	{
		public BezierSpline spline;

		public TravelMode travelMode;

		public float speed = 5f;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_normalizedT;

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

		public override bool MovingForward => speed > 0f == isGoingForward;

		private void Update()
		{
			Execute(Time.deltaTime);
		}

		public override void Execute(float deltaTime)
		{
			float num = (isGoingForward ? speed : (0f - speed));
			Vector3 position = spline.MoveAlongSpline(ref m_normalizedT, num * deltaTime);
			base.transform.position = position;
			bool movingForward = MovingForward;
			if (lookAt == LookAtMode.Forward)
			{
				Quaternion b = ((!movingForward) ? Quaternion.LookRotation(-spline.GetTangent(m_normalizedT)) : Quaternion.LookRotation(spline.GetTangent(m_normalizedT)));
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, rotationLerpModifier * deltaTime);
			}
			else if (lookAt == LookAtMode.SplineExtraData)
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, spline.GetExtraData(m_normalizedT, BezierWalker.extraDataLerpAsQuaternionFunction), rotationLerpModifier * deltaTime);
			}
			if (movingForward)
			{
				if (m_normalizedT >= 1f)
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
						isGoingForward = !isGoingForward;
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
			}
			else if (m_normalizedT <= 0f)
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
					isGoingForward = !isGoingForward;
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
