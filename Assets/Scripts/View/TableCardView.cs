using Core;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	public class TableCardView : MonoBehaviour, ICardView
	{
		[SerializeField] private CardView view;
		
		public Image Art => view.Art;
		
		public GameObject GameObject => view.GameObject;
		public GameObject Root => view.Root;
		public CardController Controller => null;

		private void Awake()
		{
			gameObject.SetActive(false);
		}

		public void Init(Card model)
		{
			view.Init(model);
		}
	}
}