namespace ContainersApiTask.Models.Enumerations
{
    public enum RequestStatus
    {
        CREATED,
        DELETED,
        UPDATED,
        VIEWED,
        NOT_FOUND = -1,
        FORBIDDEN = -2,
        BAD_REQUEST = -3
    }
}
