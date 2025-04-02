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
        Task UpdateCompanyRibAsync(string companyRib);
        Task UpdateBankNameAsync(string bankName);
        Task<string> GetCompanyRibAsync();
        Task<string> GetBankNameAsync();
    }
}
