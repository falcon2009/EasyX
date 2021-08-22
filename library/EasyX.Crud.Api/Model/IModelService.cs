namespace EasyX.Crud.Api.Model
{
    public interface IModelService<in TKey> : IModelProvider<TKey>, IModelManager<TKey> where TKey : class
    { }
}
