using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Rendering;
using Utils;
using View;
using Random = UnityEngine.Random;

namespace Core
{
	[RequireComponent(typeof(CardLayout))]
	public class Hand : Singleton<Hand>
	{
		private readonly ObservableList<Card> _cards = new();
		private CardLayout _cardLayout;

		public Card this[int idx] => _cards[idx % _cards.Count];

		protected override void Awake()
		{
			base.Awake();
			_cardLayout = GetComponent<CardLayout>();
		}

		private void Start()
		{
			GetCards();
		}

		public void Remove(Card card)
		{
			_cards.Remove(card);
		}

		[Button]
		private void GetCards()
		{
			_cards.ClearFixed();
			_cards.AddRange(CardFactory.Instance.CreateAll());
			
			_cards.ItemAdded += CardAdded;
			_cards.ItemRemoved += CardRemoved;
			
			CalculateLayout();
		}

		[Button]
		private void CreateCard()
		{
			Card card = CardFactory.Instance.Create();
			_cards.Add(card);
		}
		
		[Button]
		private void RemoveRandomCard()
		{
			_cards.RemoveAt(Random.Range(0, _cards.Count));
		}

		private void CardAdded(ObservableList<Card> sender, ListChangedEventArgs<Card> listChangedEventArgs)
		{
			CalculateLayout();
		}
		
		private void CardRemoved(ObservableList<Card> sender, ListChangedEventArgs<Card> listChangedEventArgs)
		{
			listChangedEventArgs.item.Dispose();
			CalculateLayout();
		}

		private void Update()
		{
			CalculateLayout();
		}

		private void CalculateLayout()
		{
			_cardLayout.Reposition(_cards.Where(card => card.UIView.GameObject.activeSelf).ToArray());
		}
	}
}