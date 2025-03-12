﻿using BlogApp.Models;
using BlogApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILikesRepository _likesRepository;

        public PostController(IPostRepository postRepository, IWebHostEnvironment webHostEnvironment, ILikesRepository likeRepository)
        {
            _postRepository = postRepository;
            _webHostEnvironment = webHostEnvironment;
            _likesRepository = likeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            return View(posts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null) return NotFound();
            return View(post);
        }
        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            return View(new Post());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Post post, IFormFile? PhotoFile)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
                return View(post);

            if (PhotoFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(PhotoFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await PhotoFile.CopyToAsync(fileStream);
                }

                post.PhotoPath = uniqueFileName;
            }

           
            post.UserId = userId.Value;
            post.CreatedAt = DateTime.UtcNow;
            await _postRepository.AddPostAsync(post);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null) return NotFound();
            return View(post);
        }
       


        [HttpPost]
        public async Task<IActionResult> Edit(int id, Post updatedPost, IFormFile PhotoFile)
        {
            if (id != updatedPost.Id) return NotFound();

            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null) return NotFound();

            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;
            post.Status = updatedPost.Status;

            if (PhotoFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(PhotoFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await PhotoFile.CopyToAsync(fileStream);
                }

                post.PhotoPath = uniqueFileName;
            }

            await _postRepository.UpdatePostAsync(post);
            return RedirectToAction("Index");
        }
       /* [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }

        // Role based access
        [Authorize(Roles = "Admin")]
        public IActionResult ManageUsers()
        {
            return View();
        }
       */
        public async Task<IActionResult> Delete(int id)
        {
            await _postRepository.DeletePostAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, PostStatus status)
        {
            await _postRepository.UpdatePostStatusAsync(id, status);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Like(int postId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //Check if this user has already liked this post
            // Hint: Need to use HasUserLikedAsync function from like repository
       
             if(await _likesRepository.HasUserLikedAsync(postId, userId))
            {
                // No Action
            }
            //If user has not liked this post hen call AddLikeAsync to add a like in database.
            else
            {
                await _likesRepository.AddLikeAsync(postId, userId);
            }

            return RedirectToAction("Details", new { id = postId });
        }

        [HttpPost]
        public async Task<IActionResult> ReportPost(Report report)
        {
            report.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _postRepository.ReportPostAsync(report);
            return RedirectToAction("Details", new { id = report.PostId });
        }










    }
}
