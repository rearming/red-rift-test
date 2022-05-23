using Core;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	
	public class CardView : MonoBehaviour
	{
		[Header("View")]
		[SerializeField] private Image background;
		[SerializeField] private Image art;
		
		[SerializeField] private TextMeshProUGUI title;
		[SerializeField] private TextMeshProUGUI description;
		[SerializeField] private TextMeshProUGUI manaValue;
		[SerializeField] private TextMeshProUGUI attackValue;
		[SerializeField] private TextMeshProUGUI hpValue;

		[Header("Animation")]
		[SerializeField] private float tweenDuration = 0.5f;
		
		private TweenerCore<Vector3, Vector3, VectorOptions> _posTweener;
		private TweenerCore<Quaternion, Quaternion, NoOptions> _rotTweener;

		public Image Art => art;

		public void Init(Card model)
		{
			model.Title.ValueChanged += newValue => title.text = newValue;
			model.Description.ValueChanged += newValue => description.text = newValue;
			model.ManaValue.ValueChanged += newValue => manaValue.text = newValue.ToString();
			model.AttackValue.ValueChanged += newValue => attackValue.text = newValue.ToString();
			model.HpValue.ValueChanged += newValue => hpValue.text = newValue.ToString();

			StartCoroutine(CardFactory.Instance.GetArt(this));
		}

		public void Refresh(Vector3 newPos, Quaternion newRot)
		{
			_posTweener = transform.DOMove(newPos, tweenDuration);
			_rotTweener = transform.DORotateQuaternion(newRot, tweenDuration);
		}

		public void MoveToTable()
		{
			_posTweener.Goto(1);
			_rotTweener.Goto(1);
		}
	}
}