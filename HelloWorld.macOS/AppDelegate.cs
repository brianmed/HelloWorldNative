using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// https://dev.to/kicsipixel/nswindow-without-storyboard-2g2d

// dotnet publish -c Release -f net8.0-macos -p:UseCurrentRuntimeIdentifier=false -p:PublishTrimmed=true -p:TrimMode=full
// dotnet publish -c Release -f net8.0-macos -p:UseCurrentRuntimeIdentifier=false -p:PublishTrimmed=true -p:TrimMode=partial
// dotnet publish -c Release -f net8.0-macos -p:UseCurrentRuntimeIdentifier=false -p:PublishAot=true

namespace HelloWorld;

[Register ("AppDelegate")]
public class AppDelegate : NSApplicationDelegate
{
    private MainWindow MainWindow;

    public IServiceProvider ServiceProvider;

	public override void DidFinishLaunching(NSNotification notification)
	{
        this.CreateMenu();

        this.ServiceProvider = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            })
            .AddTransient<IMainWindowViewController, MainWindowViewController>()
            .BuildServiceProvider();

        ILogger logger = this.ServiceProvider
            .GetService<ILoggerFactory>()
            .CreateLogger<Program>();

        logger.LogInformation("Starting application");

        // Bring to front
        NSApplication.SharedApplication.ActivateIgnoringOtherApps(flag: true);

        this.MainWindow = new MainWindow();

        this.MainWindow.Window.MakeKeyAndOrderFront(sender: null);
	}

	public override void WillTerminate(NSNotification notification)
	{ }

    public void CreateMenu()
    {
        // Get application name
        NSDictionary bundleInfoDict = NSBundle.MainBundle.InfoDictionary;
        string appName = bundleInfoDict["CFBundleName"].ToString();

        // Add menu
        NSMenu mainMenu = new();

        NSMenu appMenu = new();
        appMenu.AddItem(title: $"Quit {appName}", action: new ObjCRuntime.Selector("terminate:"), charCode: "q");

        NSMenuItem appMenuItem = new();
        appMenuItem.Submenu = appMenu;

        NSMenuItem fileMenuItem = new(title: "File");
        NSMenu fileMenu = new();
        fileMenu.AddItem(title: $"Hide {appName}", action: new ObjCRuntime.Selector("hide:"), charCode: "");
        fileMenuItem.Submenu = fileMenu;

        mainMenu.AddItem(appMenuItem);
        mainMenu.AddItem(fileMenuItem);

        NSApplication.SharedApplication.MainMenu = mainMenu;
    }
}
