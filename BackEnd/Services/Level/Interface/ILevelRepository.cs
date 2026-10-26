namespace BackEnd.Services.Level.Interface
{
    public interface ILevelRepository
    {
        public Task<BackEnd.Entities.Level.Level?> GetLevelByXpAsync(int Xp);
    }
}
