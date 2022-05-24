using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	public class CardView : MonoBehaviour, ICardView
	{
		[Header("Refs")]
		[SerializeField] private GameObject root;
		[SerializeField] private CardController controller;
		
		[Header("View")]
		[SerializeField] private Image background;
		[SerializeField] private Image art;
		
		[SerializeField] private TextMeshProUGUI title;
		[SerializeField] private TextMeshProUGUI description;
		[SerializeField] private TextMeshProUGUI manaValue;
		[SerializeField] private TextMeshProUGUI attackValue;
		[SerializeField] private TextMeshProUGUI hpValue;

		public Image Art => art;
		public GameObject GameObject => gameObject;
		public GameObject Root => root;
		public CardController Controller => controller;

		public void Init(Card model)
		{
			model.Title.ValueChanged += newValue => title.text = newValue;
			model.Description.ValueChanged += newValue => description.text = newValue;
			model.ManaValue.ValueChanged += newValue => manaValue.text = newValue.ToString();
			model.AttackValue.ValueChanged += newValue => attackValue.text = newValue.ToString();
			model.HpValue.ValueChanged += newValue => hpValue.text = newValue.ToString();
		}
	}
}