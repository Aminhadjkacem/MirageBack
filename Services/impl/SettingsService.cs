using DHP_Manager.Repositories.Interface;
using DHP_Manager.Services.Interface;

namespace DHP_Manager.Services.impl
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<decimal> GetProfitMarginAsync()
        {
            return await _settingsRepository.GetProfitMarginAsync();
        }

        public async Task UpdateProfitMarginAsync(decimal newMargin)
        {
            if (newMargin < 0)
                throw new ArgumentException("Profit margin cannot be negative.", nameof(newMargin));
            await _settingsRepository.UpdateProfitMarginAsync(newMargin);
        }

        public async Task<decimal> GetTVAMarginAsync()
        {
            return await _settingsRepository.GetTVAMarginAsync();
        }

        public async Task UpdateTVAMarginAsync(decimal newMargin)
        {
            if (newMargin < 0)
                throw new ArgumentException("TVA margin cannot be negative.", nameof(newMargin));
            await _settingsRepository.UpdateTVAMarginAsync(newMargin);
        }

        public async Task<decimal> GetDroitDuTimbrePriceAsync()
        {
            return await _settingsRepository.GetDroitDuTimbrePriceAsync();
        }

        public async Task UpdateDroitDuTimbrePriceAsync(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentException("Droit du Timbre price cannot be negative.", nameof(newPrice));
            await _settingsRepository.UpdateDroitDuTimbrePriceAsync(newPrice);
        }
        public async Task UpdateCompanyFiscalAsync(string newFiscal)
        {
            if (newFiscal == null)
                throw new ArgumentException("Fiscal cannot be null", nameof(newFiscal));
            await _settingsRepository.UpdateCompanyFiscalAsync(newFiscal);
        }
        public async Task UpdateCompanyTelAsync(string newTel)
        {
            if (newTel == null)
                throw new ArgumentException("Tel cannot be null", nameof(newTel));
            await _settingsRepository.UpdateCompanyTelAsync(newTel);
        }
        public async Task<string> GetCompanyTelAsync()
        {
            return await _settingsRepository.GetCompanyTelAsync();
        }
        public async Task<string> GetCompanyFiscalAsync()
        {
            return await _settingsRepository.GetCompanyFiscalAsync();
        }
        public async Task UpdateFactureId()
        {
               await _settingsRepository.UpdateFactureIdAsync();
        }
        public async Task<int> GetFactureIdAsync()
        {
            return await _settingsRepository.GetFactureIdAsync();
        }

        public async Task UpdateCompanyRibAsync(string companyRib)
        {
            if (string.IsNullOrWhiteSpace(companyRib))
                throw new ArgumentException("Company RIB cannot be empty.", nameof(companyRib));

            await _settingsRepository.UpdateCompanyRibAsync(companyRib);
        }

        public async Task UpdateBankNameAsync(string bankName)
        {
            if (string.IsNullOrWhiteSpace(bankName))
                throw new ArgumentException("Bank name cannot be empty.", nameof(bankName));

            await _settingsRepository.UpdateBankNameAsync(bankName);
        }

        public async Task<string> GetBankNameAsync()
        {
            return await _settingsRepository.GetBankNameAsync();
        }

        public async Task<string> GetCompanyRibAsync()
        {
            return await _settingsRepository.GetCompanyRibAsync();
        }
    }
}
