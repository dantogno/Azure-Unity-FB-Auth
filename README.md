# Azure-Unity-FB-Auth
This example project demonstrates how to incorporate Facebook authentication into a Unity project that stores high score and game telemetry data in Azure Easy Tables.

The approach uses the [Facebook Unity SDK](https://developers.facebook.com/docs/unity/) to allow users to log in to Facebook. It then uses [UnityWebRequest](https://docs.unity3d.com/Manual/UnityWebRequest.html) to send HTTP requests to an [Azure function app](https://azure.microsoft.com/en-us/services/functions/) that handles authentication and data insertion and retrieval.

This project is not intended as a complete SDK, but rather an example of connectivity between a Unity app, an Azure Function App, and an Azure App Service with Facebook authentication.

This example is based on the [Azure Services for Unity](https://github.com/dgkanatsios/AzureServicesForUnity) project, with modifications to use Azure functions in place of Easy APIs, and to incorporate a Facebook login flow. 
