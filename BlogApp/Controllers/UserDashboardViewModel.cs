namespace BlogApp.Controllers
{
    internal class UserDashboardViewModel
    {
        public int TotalPosts { get; set; }
        public int TotalLikes { get; internal set; }
        public int TotalComments { get; internal set; }
     
    }
}