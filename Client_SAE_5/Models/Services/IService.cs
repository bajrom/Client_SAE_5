namespace Client_SAE_5.Models.Services
{
    public interface IService<T>
    {
        Task<List<T>> GetAllTAsync(string nomControleur);
        Task<T> GetTAsync(string nomControleur, int id);
        Task<T> PostTAsync(string nomControleur, T serie);
        Task<T> PutTAsync(string nomControleur, T serie);
        Task DeleteTAsync(string nomControleur, int id);
        Task DeleteDoubleTAsync(string nomControleur, int id1, int id2);
    }
}
