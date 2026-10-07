namespace Api.Common.Errors
{
    public static class ApiErrorCodes
    {
        public const string ValidationError = "VALIDATION_ERROR";
        public const string InvalidRequestBody = "INVALID_REQUEST_BODY";
        public const string InvalidJson = "INVALID_JSON";
        public const string InvalidRequest = "INVALID_REQUEST";
        public const string InternalError = "INTERNAL_ERROR";
        public const string ResourceAlreadyExists = "RESOURCE_ALREADY_EXISTS";
        public const string ResourceInUse = "RESOURCE_IN_USE";
        public const string InvalidData = "INVALID_DATA";
        public const string PersistenceFailed = "PERSISTENCE_FAILED";
        public const string CacheUnavailable = "CACHE_UNAVAILABLE";
        public const string ExternalServiceError = "EXTERNAL_SERVICE_ERROR";
    }
}
