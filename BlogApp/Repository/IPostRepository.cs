﻿using BlogApp.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task AddPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int id);
        Task UpdatePostStatusAsync(int id, PostStatus status);
        Task ReportPostAsync(Report report);
        Task ApprovePostAsync(int postId);
        Task RejectPostAsync(int postId);

        //  Task DeletePostAsync(int post);

        Task<int> GetTotalPostsCountAsync();

        Task<int> GetTotalPostLikesCountAsync();

        Task<int> GetTotalPostCommentsCountAsync();

        


    }
}
