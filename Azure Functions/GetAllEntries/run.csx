using System.Net;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
 
public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");
    MobileServiceClient client = new MobileServiceClient("https://fb-auth-example.azurewebsites.net");
    dynamic data = await req.Content.ReadAsStringAsync();
 
    // Server expects a json arrary with the format:
    // [{"access_token":"value"},{"tableName":"value"}]
    JArray arrayJson = JArray.Parse(data);
    dynamic authToken = arrayJson[0];
    var tableName = arrayJson[1].Value<string>("tableName");
 
    // Try to log in. Return Unauthorized response upon failure.
    MobileServiceUser user = null;
    while (user == null)
    {
        try
        {
            // Change MobileServiceAuthenticationProvider.Facebook
            // to MobileServiceAuthenticationProvider.Google if using Google auth.
            user = await client.LoginAsync(MobileServiceAuthenticationProvider.Facebook, authToken);
        }
        catch (InvalidOperationException exception)
        {
            log.Info("Log in failure!");
            return req.CreateErrorResponse(HttpStatusCode.Unauthorized, exception);
        }
    }
 
    // Try to get all entries as a list. Return BadRequest response upon failure.
    try
    {
        var table = client.GetTable(tableName);
        var list = await table.ReadAsync(string.Empty);
        return req.CreateResponse(HttpStatusCode.OK, list);
    }
    catch (Exception exception)
    {
        return req.CreateErrorResponse(HttpStatusCode.BadRequest, exception);
    }
}
