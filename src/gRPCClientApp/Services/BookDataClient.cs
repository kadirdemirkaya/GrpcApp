using ArticleService;
using Grpc.Net.Client;

namespace gRPCClientApp.Services
{
    public class BookDataClient : IBookDataClient
    {
        private readonly IConfiguration _configuration;

        public BookDataClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public GrpcArticleModel ReturnAllArticles(string id)
        {
            System.Console.WriteLine($"Calling GRPC Service {_configuration["GrpcPlatform"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
            var client = new GrpcArticle.GrpcArticleClient(channel);
            var request = new GetAllRequestArticle() { Id = id };

            try
            {
                var reply = client.GetArticles(request);
                return reply.Article.FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("ERROR MESSAGE FOR GRPC: " + ex.Message);
                return null;
            }
        }
    }
}