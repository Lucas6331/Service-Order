using OsDsII.api.Models;

namespace OsDsII.api.Repository.Comments
{
    public interface ICommentsRepository
    {
        public Task AddCommentAsync(Comment comment);

    }
}
