namespace Conduit.Shared.Constants
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class ConduitConstants
    {
        // Application Constants
        public const string ApiVersion = "0.1.0-alpha";

        // JSON Serialization Settings
        public static readonly JsonSerializerSettings ConduitJsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };
    }
}