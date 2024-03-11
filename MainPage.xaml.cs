namespace maui0;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		BindingContext = new ViewModel(); 
		Application.Current!.RequestedThemeChanged += RequestedThemeChanged; 
	}

	private void RequestedThemeChanged(object sender, AppThemeChangedEventArgs atcea) {
		UpdateThemeOnMainThread(); 
	}

	public void UpdateThemeOnMainThread() {
		var theme = Application.Current!.RequestedTheme; 
		if (theme == AppTheme.Dark) {
			Logo.Source = "cslogod.png"; 		
		} else if (theme == AppTheme.Light) {
			Logo.Source = "cslogol.png"; 
		} else {
			Console.WriteLine("Unknown theme");
		}
	}

    protected override async void OnAppearing() {
		base.OnAppearing();
		await Task.Delay(1000);
		Dispatcher.Dispatch(() => {
			((ViewModel)BindingContext).LoadContent(); 
			Console.WriteLine("After load. "); 
		}); 
		Dispatcher.Dispatch(() => {
			UpdateThemeOnMainThread(); 
			Console.WriteLine("After load2. ");  
		}); 
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
		Application.Current!.RequestedThemeChanged -= RequestedThemeChanged; 
    }

    private void OnQueryInputChanged(object sender, TextChangedEventArgs tcea) {
		Console.WriteLine($"Query input changed: {tcea.NewTextValue}"); 
		// ((ViewModel)BindingContext).Query = tcea.NewTextValue; 
	}

	private void OnEntryCompleted(object sender, EventArgs ea) {
		Console.WriteLine($"Entry completed: {((Entry)sender).Text}"); 
		// ((ViewModel)BindingContext).Query = ((Entry)sender).Text;  
	}

}

