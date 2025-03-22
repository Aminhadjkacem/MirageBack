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
    }
}
