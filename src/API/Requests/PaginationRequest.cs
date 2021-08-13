namespace Attender.Server.API.Requests
{
    public class PaginationRequest
    {
        public int PageSize { get; init; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
