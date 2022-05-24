using System.Collections.Generic;
using Sirenix.OdinInspector;
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
		private readonly List<Card> _cardsInHand = new();

		private CardLayout _cardLayout;

		public IReadOnlyList<Card> CardsInHand => _cardsInHand;

		protected override void Awake()
		{
			base.Awake();
			_cardLayout = GetComponent<CardLayout>();
		}

		private void Start()
		{
			GetCards();
		}

		public void Remove(Card card) => _cards.Remove(card);

		[Button]
		private void GetCards()
		{
			if (_cards.Count == 0)
			{
				_cards.ItemAdded += CardCreated;
				_cards.ItemRemoved += CardDeleted;
			}
			
			_cards.ClearFixed();

			foreach (Card card in CardFactory.Instance.CreateAll())
			{
				_cards.Add(card);
				card.Controller.CardPicked += () => OnCardPicked(card);
				card.Controller.CardInHand += () => OnCardReturnToHand(card);
			}

			CalculateLayout();
		}

		[Button]
		private void CreateCard()
		{
			Card card = CardFactory.Instance.Create();
			_cards.Add(card);
		}
		
		[Button]
		private void DeleteRandomCard()
		{
			_cards.RemoveAt(Random.Range(0, _cards.Count));
		}
		
		private void OnCardPicked(Card card)
		{
			_cardsInHand.Remove(card);
			CalculateLayout();
		}

		private void OnCardReturnToHand(Card card)
		{
			_cardsInHand.Add(card);
			CalculateLayout();
		}

		private void CardCreated(ObservableList<Card> sender, ListChangedEventArgs<Card> listChangedEventArgs)
		{
			_cardsInHand.Add(listChangedEventArgs.item);
			CalculateLayout();
		}

		private void CardDeleted(ObservableList<Card> sender, ListChangedEventArgs<Card> listChangedEventArgs)
		{
			_cardsInHand.Remove(listChangedEventArgs.item);
			listChangedEventArgs.item.Dispose();
			CalculateLayout();
		}

		private void CalculateLayout()
		{
			_cardLayout.Reposition(_cardsInHand);
		}
	}
}