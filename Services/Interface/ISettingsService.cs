namespace DHP_Manager.Services.Interface
{
    public interface ISettingsService
    {
        Task<decimal> GetProfitMarginAsync();
        Task UpdateProfitMarginAsync(decimal newMargin);
        Task<decimal> GetTVAMarginAsync(); // New method
        Task UpdateTVAMarginAsync(decimal newMargin); // New method
        Task<decimal> GetDroitDuTimbrePriceAsync(); // New method
        Task UpdateDroitDuTimbrePriceAsync(decimal newPrice); // New method
    }
}
