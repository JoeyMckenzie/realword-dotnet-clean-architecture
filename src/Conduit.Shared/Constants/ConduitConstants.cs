namespace Conduit.Shared.Constants
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class ConduitConstants
    {
        // JSON Serialization Settings
        public static readonly JsonSerializerSettings ConduitJsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}