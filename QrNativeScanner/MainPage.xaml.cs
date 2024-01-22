using WorkApp.Services;

namespace QrNativeScanner;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		QrScannerService.StartScanning((res) =>
		{
			Console.WriteLine("Scanned " + res);
		}, this);

	}
}


