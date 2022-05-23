using System;

namespace Utils
{
	public class BindableField<T>
	{
		public event Action<T> ValueChanged;
		private T _value;

		public T Value
		{
			get => _value;
			set
			{
				if (Equals(_value, value))
					return;
				
				_value = value;
				ValueChanged?.Invoke(value);
			}
		}
		
		public BindableField() : this(default)
		{
		}

		public BindableField(T initialValue)
		{
			Value = initialValue;
		}
		
		public override string ToString()
		{
			return _value.ToString();
		}
		
		public static implicit operator T(BindableField<T> field)
		{
			return field._value;
		}
	}
}