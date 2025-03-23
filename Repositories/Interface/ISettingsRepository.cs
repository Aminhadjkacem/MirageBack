namespace DHP_Manager.Repositories.Interface
{
    public interface ISettingsRepository
    {
        Task<decimal> GetProfitMarginAsync();
        Task UpdateProfitMarginAsync(decimal newMargin);
        Task<decimal> GetTVAMarginAsync(); // New method for TVA margin
        Task UpdateTVAMarginAsync(decimal newMargin); // New method for TVA margin
        Task<decimal> GetDroitDuTimbrePriceAsync(); // New method for Droit du Timbre
        Task UpdateDroitDuTimbrePriceAsync(decimal newPrice); // New method for Droit du Timbre
        Task UpdateCompanyTelAsync(string newTel);
        Task<string> GetCompanyTelAsync();
        Task UpdateCompanyFiscalAsync(string newFiscal);
        Task<string> GetCompanyFiscalAsync();
        Task UpdateFactureIdAsync();
        Task<int> GetFactureIdAsync();
    }
}
