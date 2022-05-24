using DG.Tweening;
using SpriteGlow;
using UnityEngine;
using Utils;

namespace View
{
	public class CardGlow : Singleton<CardGlow>
	{
		[SerializeField] private float brightnessEnabled = 3;
		[SerializeField] private float brightnessDisabled = 0;
		[SerializeField] private float translationSpeed = 0.5f;
		[SerializeField] private SpriteGlowEffect glow;

		private CardController _controller;
		private Camera _camera;

		private GameObject _tableGlowObject;

		protected override void Awake()
		{
			base.Awake();
			
			_camera = Camera.main;
		}

		public void Bind(CardController controller)
		{
			if (_controller != null)
			{
				_controller.CardPicked -= Enable;
				_controller.CardInHand -= Disable;
				_controller.CardMovedToTable -= OnCardMovedToTable;
				_controller.CardMovedFromTable -= OnCardMovedFromTable;
				_controller.CardDroppedOnTable -= OnCardDroppedOnTable;
			}
			
			_controller = controller;
			_tableGlowObject = _controller.TableView.Root.GetComponentInChildren<SpriteGlowEffect>(true).gameObject;

			_controller.CardPicked += Enable;
			_controller.CardInHand += Disable;
			_controller.CardMovedToTable += OnCardMovedToTable;
			_controller.CardMovedFromTable += OnCardMovedFromTable;
			_controller.CardDroppedOnTable += OnCardDroppedOnTable;
		}

		private void Update()
		{
			if (_controller == null)
				return;
			
			transform.position = _camera.ScreenToWorldPoint(_controller.transform.position.WithZ(_camera.nearClipPlane));
		}

		private void OnCardDroppedOnTable()
		{
			_tableGlowObject.SetActive(false);
			Disable();
		}

		private void OnCardMovedToTable()
		{
			_tableGlowObject.SetActive(true);
			glow.gameObject.SetActive(false);
		}

		private void OnCardMovedFromTable()
		{
			_tableGlowObject.SetActive(false);
			glow.gameObject.SetActive(true);
		}

		private void Enable()
		{
			DOTween.ToAlpha(() => glow.Renderer.color, newColor => glow.Renderer.color = newColor, 1f, translationSpeed);
			DOTween.To(bright => glow.GlowBrightness = bright, glow.GlowBrightness, brightnessEnabled, translationSpeed);
		}

		private void Disable()
		{
			DOTween.ToAlpha(() => glow.Renderer.color, newColor => glow.Renderer.color = newColor, 0f, translationSpeed);
			DOTween.To(bright => glow.GlowBrightness = bright, glow.GlowBrightness, brightnessDisabled, translationSpeed);
		}
	}
}