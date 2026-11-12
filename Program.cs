using CommunityToolkit.WinUI.Notifications;

namespace WindowsNotifier;

// Minimal Win32 toast CLI. Shows a desktop toast and exits.
// Usage:  windows-notifier <title> [message]
// Exit:   0 shown, 1 toast failed, 2 bad args. On failure the reason goes to stderr
//         so the PowerShell caller can decide whether to warn or stay quiet.
internal static class Program
{
    private static int Main(string[] args)
    {
        if (args.Length == 0 || args[0] is "-h" or "--help" or "/?")
        {
            Console.Error.WriteLine("Usage: windows-notifier <title> [message]");
            return args.Length == 0 ? 2 : 0;
        }

        var title = args[0];
        var message = args.Length > 1 ? args[1] : null;

        try
        {
            var builder = new ToastContentBuilder().AddText(title);
            if (!string.IsNullOrEmpty(message))
            {
                builder.AddText(message);
            }

            builder.Show();
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"windows-notifier: toast failed: {ex.Message}");
            return 1;
        }
    }
}
