public interface ICommentRepository
{
    Task AddCommentAsync(Comment comment);
    Task<List<Comment>> GetCommentsAsync();
    Task LikeCommentAsync(int commentId, int userId);
}
