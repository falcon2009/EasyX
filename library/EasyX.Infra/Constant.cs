namespace EasyX.Infra
{
    public static class Constant
    {
        public static class Errors
        {
            public const string ApplicationNull = "application_null";
            public const string BuilderNull = "builder_null";
            public const string DataProviderNull = "data_provider_null";
            public const string DataManagerNull = "data_manager_null";
            public const string FilterItemInvalid = "filter_item_invalid";
            public const string FilterItemNull = "filter_item_null";
            public const string KeyNull = "key_null";
            public const string ModelNull = "model_null";
            public const string ModelInvalid = "model_invalid";
            public const string OptionNull = "option_null";
            public const string RequestFilterNull = "request_filter_null";
            public const string RequestInvalid = "invalid_request";
            public const string ServicesNull = "services_null";
            public const string UnsupportedModel = "unsupported_model";
            public const string QueryNull = "query_null";
        }
        public static class MediaType
        {
            public static class Application
            {
                public const string Json = "application/json";
                public const string Xml = "application/xml";
                public const string FormUrlencoded = "application/x-www-form-urlencoded";
            }
        }
        public static class StatusCode
        {
            public const int OK = 200;
            public const int BadRequest = 400;
            public const int NoFOund = 404;
            public const int Conflict = 409;
            public const int InternalServerError = 500;
        }

        public static class Headers
        {
            public const string Transaction = "X-TransactionId";
        }
    }
}
