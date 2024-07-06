using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HelloWorld;

public class MainWindow
{
    public NSWindow Window { get; private set; }

    public NSViewController ViewController { get; private set; }

    public MainWindow()
    {
        NSWindowStyle style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

        CGSize screenSize = NSScreen.MainScreen.Frame.Size;

        CGSize windowSize = new CGSize(width: 1920, height: 1080);
        CGRect windowRect = new CGRect(screenSize.Width / 2.0 - windowSize.Width / 2.0, screenSize.Height / 2.0 - windowSize.Height / 2.0, windowSize.Width, windowSize.Height);

        this.Window = new NSWindow(windowRect, style, NSBackingStore.Buffered, deferCreation: false);

        this.Window.Title = nameof(HelloWorld);

        IMainWindowViewController mainWindowViewController = ((AppDelegate)NSApplication.SharedApplication.Delegate)
            .ServiceProvider
            .GetService<IMainWindowViewController>();

        this.Window.ContentView = ((NSViewController)mainWindowViewController).View;

        this.Window.WillClose += OnWillClose;
    }

    ~MainWindow()
    {
        Window?.Dispose();
        Window = null;

        ViewController?.Dispose();
        ViewController = null;
    }

    private void OnWillClose(object sender, EventArgs e)
    {
        NSApplication.SharedApplication.Terminate(this.Window);
    }
}

public interface IMainWindowViewController
{ }

public class MainWindowViewController : NSViewController, IMainWindowViewController
{
    private NSButton JoyButton { get; set; }

    ~MainWindowViewController()
    {
        this.JoyButton.Activated -= OnJoyButtonClick;
        this.JoyButton?.Dispose();
        this.JoyButton = null;
    }

    public override void LoadView()
    {
        this.View = new NSView();

        this.View.TranslatesAutoresizingMaskIntoConstraints = false;

        this.JoyButton = new();
        this.View.AddSubview(this.JoyButton);
        this.JoyButton.TranslatesAutoresizingMaskIntoConstraints = false;
        this.JoyButton.Title = "Joy";
        this.JoyButton.SetButtonType(NSButtonType.MomentaryLightButton);
        this.JoyButton.BezelStyle = NSBezelStyle.Rounded;
        this.JoyButton.CenterXAnchor.ConstraintEqualTo(this.View.CenterXAnchor).Active = true;
        this.JoyButton.CenterYAnchor.ConstraintEqualTo(this.View.CenterYAnchor).Active = true;
        this.JoyButton.Activated += OnJoyButtonClick;
        this.JoyButton.SizeToFit();
    }

    public void OnJoyButtonClick(object sender, EventArgs e)
    {
        using NSAlert alert = new()
        {
            MessageText = "Hello World"
        };

        alert.BeginSheet(this.View.Window);
    }
}
