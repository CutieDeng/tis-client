namespace maui0;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		BindingContext = new ViewModel(); 
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

    protected override async void OnAppearing() {
		base.OnAppearing();
		await Task.Delay(1000);
		Dispatcher.Dispatch(() => {
			((ViewModel)BindingContext).LoadContent(); 
			Console.WriteLine("After load. "); 
		}); 
	}

}

