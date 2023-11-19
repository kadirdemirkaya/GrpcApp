using ArticleService;
using Grpc.Core;

namespace gRPCServerApp.Services
{
    public class GrpcArticleService : GrpcArticle.GrpcArticleBase
    {
        public override Task<ArticleResponse> GetArticles(GetAllArticleRequest request, ServerCallContext context)
        {
            System.Console.WriteLine("Come The Request In the GetArticles");
            var response = new ArticleResponse();
            GrpcArticleModel aModel = new();
            if (request.Id == "1")
            {
                aModel = new GrpcArticleModel() { ArticleAge = "32", ArticleName = "Oliver Pitt" };
                response.Article.Add(aModel);
            }
            if (request.Id == "2")
            {
                aModel = new GrpcArticleModel() { ArticleAge = "31", ArticleName = "Ali Cabbar" };
                response.Article.Add(aModel);
            }
            if (request.Id == "3")
            {
                aModel = new GrpcArticleModel() { ArticleAge = "30", ArticleName = "Kyle Chen" };
                response.Article.Add(aModel);
            }
            return Task.FromResult(response);
        }
    }
}
