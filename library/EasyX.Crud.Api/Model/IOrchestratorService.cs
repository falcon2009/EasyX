namespace EasyX.Crud.Api.Model
{
    public interface IOrchestratorService<in TKey> : IModelService<TKey> where TKey : class
    {
    }
}
