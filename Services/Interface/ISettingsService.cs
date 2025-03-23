namespace DHP_Manager.Services.Interface
{
    public interface ISettingsService
    {
        Task<decimal> GetProfitMarginAsync();
        Task UpdateProfitMarginAsync(decimal newMargin);
        Task<decimal> GetTVAMarginAsync();
        Task UpdateTVAMarginAsync(decimal newMargin);
        Task<decimal> GetDroitDuTimbrePriceAsync();
        Task UpdateDroitDuTimbrePriceAsync(decimal newPrice);
        Task UpdateCompanyFiscalAsync(string newFiscal);
        Task UpdateCompanyTelAsync(string newTel);
        Task<string> GetCompanyTelAsync();

        Task<string> GetCompanyFiscalAsync();
        Task UpdateFactureId();
        Task<int> GetFactureIdAsync();
    }
}
