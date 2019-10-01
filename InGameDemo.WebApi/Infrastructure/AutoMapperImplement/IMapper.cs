namespace InGameDemo.WebApi.Infrastructure.AutoMapperImplement
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
