using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Blogger.v3;
using Google.Apis.Blogger.v3.Data;
using Google.Apis.Services;

namespace BloggerViewer.Utils
{
    public class PostWrapper
    {
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public IEnumerable<string> Images { get; set; }
    }

    public class BloggerWrapper
    {
        private static BloggerService Service;

        public static async Task AuthenticateAsync()
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                Application.GetResourceStream(new Uri("pack://application:,,,/client_id.json")).Stream,
                new[] { BloggerService.Scope.BloggerReadonly },
                "user",
                CancellationToken.None);

            Service = new BloggerService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "BloggerQuickViewer",
            });
        }

        public static async Task<Blog> GetBlogAsync() => await Service.Blogs.GetByUrl(@"https://1z2x1z-apink.blogspot.ca/").ExecuteAsync();

        public static async Task<IEnumerable<PostWrapper>> GetPostsAsync()
        {
            var blog = await GetBlogAsync();
            var listrequest = Service.Posts.List(blog.Id);
            listrequest.MaxResults = 500;

            var lst = new List<PostWrapper>();
            while (true)
            {
                var newposts = await listrequest.ExecuteAsync();
                lst.AddRange(newposts.Items.Select(p => new PostWrapper
                {
                    Title = p.Title,
                    Published = p.Published.Value,
                    Images = GetImageLinksFromContent(p.Content)
                }));

                if (newposts.Items.Count == 0 || string.IsNullOrWhiteSpace(newposts.NextPageToken))
                    break;

                listrequest.PageToken = newposts.NextPageToken;
            }

            return lst;
        }

        private static IEnumerable<string> GetImageLinksFromContent(string content)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);
            var nodes = doc.DocumentNode.SelectNodes(".//img");
            return nodes == null ? new string[0] : nodes.Select(n => n.Attributes["src"].Value).ToArray();
        }
    }
}
