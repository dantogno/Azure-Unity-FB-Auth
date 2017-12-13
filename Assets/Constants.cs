public class Constants
{
    public static readonly string Accept = "Accept";
    public static readonly string Content_Type = "Content-Type";
    public static readonly string ApplicationJson = "application/json";
    public static readonly string ZumoString = "ZUMO-API-VERSION";
    public static readonly string ZumoVersion = "2.0.0";
    public static readonly string ZumoAuth = "X-ZUMO-AUTH";
    public static readonly string ErrorOccurred = "Error occurred";
}

// TODO: Consider removing, as it seems to be in UnityWebRequest
public enum HttpMethod
{
    Post,
    Get,
    Patch,
    Delete,
    Put,
    Merge
}