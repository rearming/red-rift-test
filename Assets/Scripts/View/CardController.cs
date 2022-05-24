using System;
using Core;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View
{
	public class CardController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		[Header("Settings")]
		[SerializeField] private float moveSpeed = 30f;
		[SerializeField] private float rotationSpeed = 30f;

		[Header("Debug")]
		[SerializeField] private bool debug;

		private Card _model;
		private ICardView _uiView;
		private ICardView _tableView;

		private Transform _parent;
		private bool _downed;

		private TweenerCore<Vector3, Vector3, VectorOptions> _posTweener;
		private TweenerCore<Quaternion, Quaternion, NoOptions> _rotTweener;
		
		private (Vector3 pos, Quaternion rot) _handPosition;

		public Transform ControlledTransform { get; private set; }
		public ICardView TableView => _tableView;
		
		public event Action CardPicked;
		public event Action CardInHand;
		public event Action CardMovedToTable;
		public event Action CardMovedFromTable;
		public event Action CardDroppedOnTable;

		public void Init(Card model, ICardView uiView, ICardView tableView)
		{
			_model = model;
			_uiView = uiView;
			_tableView = tableView;
		}

		public void Reposition(Vector3 newPos, Quaternion newRot, float duration)
		{
			Move(newPos, newRot, duration);
			_handPosition = (newPos, newRot);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_downed = true;
			CardGlow.Instance.Bind(this);
			CardPicked?.Invoke();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			_downed = false;

			if (_tableView.Root.activeSelf)
			{
				CardDroppedOnTable?.Invoke();
				return;
			}
			ReturnToHand();
		}

		private void Update()
		{
			if (!_downed)
				return;
			Move();
		}

		private void Move()
		{
			if (Table.Instance.GetCardTablePosition(out Vector3? pos, out _))
			{
				_tableView.Root.SetActive(true);
				_uiView.GameObject.SetActive(false);
				
				ControlledTransform = _tableView.Root.transform;
				ControlledTransform.position = Vector3.Lerp(ControlledTransform.position, pos.Value, Time.deltaTime * rotationSpeed);
				CardMovedToTable?.Invoke();
			}
			else
			{
				_tableView.Root.SetActive(false);
				_uiView.GameObject.SetActive(true);
				
				if (pos.HasValue)
				{
					StopTweens();
					if (ControlledTransform == _tableView.Root.transform)
						transform.position = pos.Value;
					ControlledTransform = transform;
					ControlledTransform.position = Vector3.Lerp(ControlledTransform.position, pos.Value, Time.deltaTime * moveSpeed);
					ControlledTransform.rotation = Quaternion.Slerp(ControlledTransform.rotation, Quaternion.identity, Time.deltaTime * rotationSpeed);
					CardMovedFromTable?.Invoke();
				}
			}
			
			_projectedPos = pos ?? Vector3.zero;
		}

		private void Move(Vector3 newPos, Quaternion newRot, float duration)
		{
			StopTweens();
			_posTweener = transform.DOMove(newPos, duration);
			_rotTweener = transform.DORotateQuaternion(newRot, duration);
		}

		private void ReturnToHand()
		{
			CardInHand?.Invoke();
			Move(_handPosition.pos, _handPosition.rot, CardLayout.Instance.ReturnToHandDuration);
		}

		private void StopTweens()
		{
			_posTweener.Kill();
			_rotTweener.Kill();
		}

		private Vector3 _projectedPos;
		
		private void OnDrawGizmos()
		{
			if (!Application.isPlaying && debug)
				return;
			
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(_projectedPos, 0.2f);
		}
	}
}