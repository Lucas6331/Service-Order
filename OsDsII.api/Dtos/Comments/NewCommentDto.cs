namespace OsDsII.api.Dtos.Comments
{
    public record NewCommentDto(long Id, string Description, DateTimeOffset SendDate, int ServiceOrderId);
}