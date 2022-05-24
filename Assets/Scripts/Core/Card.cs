using System;
using Utils;
using View;
using Object = UnityEngine.Object;

namespace Core
{
	public class Card : IDisposable
	{
		public BindableField<string> Title { get; } = new();
		public BindableField<string> Description { get; } = new();
		public BindableField<int> ManaValue { get; } = new();
		public BindableField<int> AttackValue { get; } = new();
		public BindableField<int> HpValue { get; } = new();
		
		public ICardView UIView { get; }
		public ICardView TableView { get; }

		public CardController Controller { get; }

		public Card(ICardView uiView, ICardView tableView, CardController controller)
		{
			UIView = uiView;
			TableView = tableView;
			Controller = controller;
			
			UIView.Init(this);
			TableView.Init(this);
			Controller.Init(this, uiView, TableView);
			
			HpValue.ValueChanged += hp =>
			{
				if (hp <= 0)
					Hand.Instance.Remove(this);
			};
		}

		public void ChangeRandomField(int change)
		{
			switch (UnityEngine.Random.Range(0, 3))
			{
				case 0:
					ManaValue.Value = change;
					break;
				case 1:
					AttackValue.Value = change;
					break;
				case 2:
					HpValue.Value = change;
					break;
			}
		}

		public void Dispose()
		{
			Object.Destroy(UIView.Root);
			Object.Destroy(TableView.Root);
			GC.SuppressFinalize(this);
		}
	}
}