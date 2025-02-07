namespace HelloWorld;

[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
	public override UIWindow? Window
    {
		get;
		set;
	}

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
		Window = new UIWindow(UIScreen.MainScreen.Bounds);

		UIViewController vc = new();

		vc.View!.AddSubview (new UILabel(Window!.Frame)
        {
			BackgroundColor = UIColor.SystemBackground,
			TextAlignment = UITextAlignment.Center,
			Text = "Hello, iOS!",
			AutoresizingMask = UIViewAutoresizing.All,
		});

		Window.RootViewController = vc;

		Window.MakeKeyAndVisible();

		return true;
	}
}
