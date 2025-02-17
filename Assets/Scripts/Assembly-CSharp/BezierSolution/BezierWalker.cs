using UnityEngine;

namespace BezierSolution
{
	public abstract class BezierWalker : MonoBehaviour
	{
		protected static readonly BezierSpline.ExtraDataLerpFunction extraDataLerpAsQuaternionFunction = InterpolateExtraDataAsQuaternion;

		public abstract BezierSpline Spline { get; }

		public abstract bool MovingForward { get; }

		public abstract float NormalizedT { get; set; }

		public abstract void Execute(float deltaTime);

		private static BezierPoint.ExtraData InterpolateExtraDataAsQuaternion(BezierPoint.ExtraData data1, BezierPoint.ExtraData data2, float normalizedT)
		{
			return Quaternion.LerpUnclamped(data1, data2, normalizedT);
		}
	}
}
