namespace Rocketcress.UIAutomation;

/// <summary>
/// Represents an application that is controlled by Rocketcress.
/// </summary>
public class Application
{
    private readonly UIAutomationTestContext _context;
    private readonly Process _process;

    private Application(UIAutomationTestContext context, Process process, ApplicationStartType startType)
    {
        _context = context;
        _process = process;
        StartType = startType;
    }

    /// <summary>
    /// Gets an empty application.
    /// </summary>
    public static Application Empty => new(null, null, ApplicationStartType.None);

    /// <summary>
    /// Gets the context this application has been attached or stated on.
    /// </summary>
    public UIAutomationTestContext Context => _context ?? throw new InvalidOperationException("This application object does not run inside a context.");

    /// <summary>
    /// Gets the process of the attached or started application.
    /// </summary>
    public Process Process => _process ?? throw new InvalidOperationException("This application object is not attached to any process.");

    /// <summary>
    /// Gets a value indicating how the application has been started.
    /// </summary>
    public ApplicationStartType StartType { get; }

    /// <summary>
    /// Launches an application from a specified file location.
    /// </summary>
    /// <param name="context">The context on which to launch the application.</param>
    /// <param name="filePath">The file path to the application to launch.</param>
    /// <returns>An <see cref="Application"/> instance representing the launched application.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
    public static Application Launch(UIAutomationTestContext context, string filePath)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        var process = Process.Start(filePath);
        process.WaitForInputIdle();
        var app = new Application(context, process, ApplicationStartType.Launched);
        AddApplicationToContext(context, app);
        return app;
    }

    /// <summary>
    /// Attaches to an already started application that was started from a specified file location.
    /// </summary>
    /// <param name="context">The context to which to attach the application.</param>
    /// <param name="filePath">The file path to the application to attach.</param>
    /// <returns>An <see cref="Application"/> instance representing the attached application.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
    /// <exception cref="InvalidOperationException">No running process with fileName was found.</exception>
    public static Application Attach(UIAutomationTestContext context, string filePath)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        var process = Process.GetProcesses().FirstOrDefault(x => string.Equals(x.StartInfo.FileName, filePath, StringComparison.OrdinalIgnoreCase));
        if (process == null)
            throw new InvalidOperationException($"No running process with fileName \"{filePath}\" was found.");
        process.WaitForInputIdle();
        var app = new Application(context, process, ApplicationStartType.Attached);
        AddApplicationToContext(context, app);
        return app;
    }

    /// <summary>
    /// Attaches to an already started application using a specified process id.
    /// </summary>
    /// <param name="context">The context to which to attach the application.</param>
    /// <param name="processId">The process id of the application to attach.</param>
    /// <returns>An <see cref="Application"/> instance representing the attached application.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
    /// <exception cref="InvalidOperationException">No running process with id was found.</exception>
    public static Application Attach(UIAutomationTestContext context, int processId)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        var process = Process.GetProcessById(processId);
        if (process == null)
            throw new InvalidOperationException($"No running process with id {processId} was found.");
        process.WaitForInputIdle();
        var app = new Application(context, process, ApplicationStartType.Attached);
        AddApplicationToContext(context, app);
        return app;
    }

    /// <summary>
    /// Attaches to an already started application using a <see cref="System.Diagnostics.Process"/> instance.
    /// </summary>
    /// <param name="context">The context to which to attach the application.</param>
    /// <param name="process">The <see cref="System.Diagnostics.Process"/> of the application to attach.</param>
    /// <returns>An <see cref="Application"/> instance representing the attached application.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is null. - or - <paramref name="process"/> is null.</exception>
    /// <exception cref="InvalidOperationException">The process is terminated.</exception>
    public static Application Attach(UIAutomationTestContext context, Process process)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));
        if (process is null)
            throw new ArgumentNullException(nameof(process));
        if (process.HasExited)
            throw new InvalidOperationException("The process is terminated.");
        process.WaitForInputIdle();
        var app = new Application(context, process, ApplicationStartType.Attached);
        AddApplicationToContext(context, app);
        return app;
    }

    /// <summary>
    /// Creates an empty application within the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>An <see cref="Application"/> instance representing the empty application.</returns>
    public static Application CreateEmpty(UIAutomationTestContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        var app = new Application(context, null, ApplicationStartType.Attached);
        AddApplicationToContext(context, app);
        return app;
    }

    private static void AddApplicationToContext(UIAutomationTestContext context, Application application)
    {
        context.Applications.Add(application);
        context.ActiveApplication = application;
    }
}

/// <summary>
/// Types with which application can be started.
/// </summary>
public enum ApplicationStartType
{
    /// <summary>
    /// The <see cref="Application"/> object does not represent any application.
    /// </summary>
    None,

    /// <summary>
    /// The application has been launched.
    /// </summary>
    Launched,

    /// <summary>
    /// The application has been attached.
    /// </summary>
    Attached,
}
