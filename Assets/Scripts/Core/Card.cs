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

		public CardView View { get; }

		public Card(CardView view)
		{
			View = view;
			View.Init(this);

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
			Object.Destroy(View.gameObject);
			GC.SuppressFinalize(this);
		}
	}
}