using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Utils;

namespace View
{
	public class CardLayout : Singleton<CardLayout>
	{
		[Header("Position")]
		
		[SerializeField] private Transform arcTransform;
		[SerializeField] private float baseArcRadius;
		[SerializeField] private AnimationCurve arcLengthByCardNum;

		[SerializeField] private float angleCorrection;
		
		[Header("Animation")]
		[SerializeField] private float repositionDuration = 0.5f;
		[SerializeField] private float returnToHandDuration = 1f;

		[Header("Debug")]
		[SerializeField] private bool debug;

		private Canvas _ui;
		private float _arcRadius;
		private float _arcLength;

		public float ReturnToHandDuration => returnToHandDuration;

		protected override void Awake()
		{
			base.Awake();
			_ui = FindObjectOfType<Canvas>();
		}

		public void Reposition(IList<Card> cards)
		{
			List<(Vector3 pos, Quaternion rotation)> layout = Calculate(cards.Count);
			cards.ForEach((c, i) => c.Controller.Reposition(layout[i].pos, layout[i].rotation, repositionDuration));
		}

		[Button]
		public List<(Vector3 pos, Quaternion rotation)> Calculate(int cardsNum)
		{
			var layout = new List<(Vector3 pos, Quaternion rotation)>();
			_arcRadius = baseArcRadius * _ui.scaleFactor;
			_arcLength = arcLengthByCardNum.Evaluate(cardsNum);

			for (var i = 0; i < cardsNum; i++)
			{
				(Vector3 pos, float rotation) = LerpArc(Mathf.InverseLerp(0, cardsNum - 1, i));
				layout.Add((pos, Quaternion.Euler(0, 0, rotation + angleCorrection)));
			}

			return layout;
		}

		private (Vector3 pos, float rotation) LerpArc(float t)
		{
			float arcLen = _arcLength * Mathf.PI * 2;
			float shift = -_arcLength * Mathf.PI;

			Matrix4x4 finalTransform = Matrix4x4.TRS(new Vector2(arcTransform.position.x, arcTransform.position.y), Quaternion.identity, Vector3.one * _arcRadius);
			Matrix4x4 invFinalTransform = Matrix4x4.Transpose(Matrix4x4.Inverse(finalTransform));
			float theta = arcLen * t + shift;
			float sinTheta = Mathf.Sin(theta);
			float cosTheta = Mathf.Cos(theta);
			
			Vector3 finalDir = new Vector3(sinTheta, cosTheta, 0.0f);
			Vector3 finalPos = new Vector3(sinTheta, cosTheta, 0.0f) * 1f;
			finalPos = finalTransform * new Vector4(finalPos.x, finalPos.y, finalPos.z, 1.0f);
			finalDir = (invFinalTransform * new Vector4(finalDir.x, finalDir.y, finalDir.z, 0.0f)).normalized;

			return (finalPos, Mathf.Atan2(finalDir.y, finalDir.x) * Mathf.Rad2Deg);
		}

		private void OnDrawGizmos()
		{
			if (!debug)
				return;
			
			if (Application.isEditor)
				_arcRadius = baseArcRadius * FindObjectOfType<Canvas>().scaleFactor;
			
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(arcTransform.position, _arcRadius);
			Gizmos.color = Color.red;

			(Vector3 pos, float rotation) firstPoint = LerpArc(0f);
			(Vector3 pos, float rotation) lastPoint = LerpArc(1f);

			Gizmos.color = Color.green;
			Gizmos.DrawSphere(firstPoint.pos, 20f);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(lastPoint.pos, 20f);
		}
	}
}