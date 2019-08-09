using LunarByte.MVVM;

public class TestViewModel : ViewModel, ITestViewModel
{
	private int valueField;

	public int Value
	{
		get { return valueField; }
		set
		{
			valueField = value;
			OnPropertyChanged();
		}
	}
}

public interface ITestViewModel
{
	int Value { get; }
}
