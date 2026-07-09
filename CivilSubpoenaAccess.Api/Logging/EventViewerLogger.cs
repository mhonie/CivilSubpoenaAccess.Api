using System.Diagnostics;
using System.Reflection;

using CivilSubpoenaAccess.Api.Serialization;

#pragma warning disable CA1416

namespace CivilSubpoenaAccess.Api.Logging;


/// <summary>
/// If the <c>Serilog</c> logger isn't available yet, log output to the Windows Event Viewer under <c>CivilSubpoenaAccess.Api.</c>
/// </summary>
public abstract class EventViewerLogger : IndentingFormatter
{
    private const string defaultEventSourceName = "Application";

    private static readonly string eventSourceName = SetEventSource();

    private static string SetEventSource()
    {
        try
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            string? targetEventSource = executingAssembly.GetName().Name;

            if (string.IsNullOrEmpty(targetEventSource))
            {
                EventLog.WriteEntry
                (
                    defaultEventSourceName,

                    $"Assembly name was null. Defaulting to {defaultEventSourceName}...",

                    EventLogEntryType.Information
                );

                return defaultEventSourceName;
            }

            if (!EventLog.SourceExists(targetEventSource))
            {
                EventLog.WriteEntry
                (
                    defaultEventSourceName,

                    $"Event source {targetEventSource} does not exist. Defaulting to {defaultEventSourceName}...",

                    EventLogEntryType.Information
                );

                return defaultEventSourceName;
            }

            EventLog.WriteEntry
            (
                targetEventSource,

                $"Using event source: {targetEventSource}...",

                EventLogEntryType.Information
            );

            return targetEventSource;
        }
        catch
        {
            EventLog.WriteEntry
            (
                defaultEventSourceName,

                $"Error determining event source. Defaulting to {defaultEventSourceName}...",

                EventLogEntryType.Information
            );

            return defaultEventSourceName;
        }
    }

    protected static void LogEvent(string logMessage, EventLogEntryType entryType)
    {
        try
        {
            EventLog.WriteEntry(eventSourceName, logMessage, entryType);
        }
        catch
        {
            EventLog.WriteEntry(defaultEventSourceName, logMessage, entryType);
        }
    }
}