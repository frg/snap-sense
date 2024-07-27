using System.Globalization;
using Emgu.CV;
using Microsoft.Extensions.Logging;

namespace SnapSense;

public class FacePersistencePersistenceHandler
{
    private readonly ILogger<FacePersistencePersistenceHandler> _logger;

    public FacePersistencePersistenceHandler(ILogger<FacePersistencePersistenceHandler> logger)
    {
        _logger = logger;
    }

    public void Save(Mat matFrame, string pathFormat)
    {
        var formattedPath = ReplaceTimestampPlaceholders(pathFormat, DateTime.Now);

        var directory = Path.GetDirectoryName(formattedPath);
        if (directory is not null && Directory.Exists(directory) is false)
        {
            Directory.CreateDirectory(directory);
        }

        matFrame.Save(formattedPath);
        _logger.LogInformation("Photo saved at {Path}.", formattedPath);
    }

    private static string ReplaceTimestampPlaceholders(string pathFormat, DateTime timestamp)
    {
        const string placeholder = "{Timestamp:";
        var startIndex = 0;

        while ((startIndex = pathFormat.IndexOf(placeholder, startIndex, StringComparison.Ordinal)) != -1)
        {
            var endIndex = pathFormat.IndexOf('}', startIndex);
            if (endIndex == -1)
            {
                break;
            }

            var dateFormat = pathFormat.Substring(startIndex + placeholder.Length, endIndex - startIndex - placeholder.Length);
            var formattedDate = timestamp.ToString(dateFormat, CultureInfo.InvariantCulture);
            pathFormat = string.Concat(pathFormat.AsSpan(0, startIndex), formattedDate, pathFormat.AsSpan(endIndex + 1));

            startIndex += formattedDate.Length; // Move past the newly inserted date string
        }

        return pathFormat;
    }
}
