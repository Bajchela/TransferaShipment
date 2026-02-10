namespace Shipments.WorkerService.Helper;

internal static class CorrelationIdHelper
{
    public static string? TryGetCorrelationId(object msg, string correlationKey)
    {
        // Probaj da pročita msg.CorrelationId (ako postoji)
        var prop = msg.GetType().GetProperty("CorrelationId");
        if (prop != null)
        {
            var value = prop.GetValue(msg)?.ToString();
            if (!string.IsNullOrWhiteSpace(value))
                return value;
        }

        // Probaj da pročita msg.Properties["CorrelationId"] (ako postoji)
        var propsProp = msg.GetType().GetProperty("Properties");
        if (propsProp != null)
        {
            var propsObj = propsProp.GetValue(msg);

            // Dictionary<string, object>
            if (propsObj is IDictionary<string, object> dictObj &&
                dictObj.TryGetValue(correlationKey, out var v1) &&
                v1 != null)
            {
                var value = v1.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }

            // Dictionary<string, string>
            if (propsObj is IDictionary<string, string> dictStr &&
                dictStr.TryGetValue(correlationKey, out var v2) &&
                !string.IsNullOrWhiteSpace(v2))
            {
                return v2;
            }
        }
        return null;
    }
}