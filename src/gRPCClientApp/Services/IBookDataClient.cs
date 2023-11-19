
using ArticleService;

namespace gRPCClientApp.Services
{
    public interface IBookDataClient
    {
        GrpcArticleModel ReturnAllArticles(string id);
    }
}