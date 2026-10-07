namespace Infrastructure.Common
{
    public static class PersistenceErrorCodes
    {
        public const string UniqueViolation = "UNIQUE_VIOLATION";
        public const string ForeignKeyViolation = "FOREIGN_KEY_VIOLATION";
        public const string InvalidData = "INVALID_DATA";
        public const string PersistenceFailed = "PERSISTENCE_FAILED";
    }
}
