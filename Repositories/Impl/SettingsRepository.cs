using DHP_Manager.Data; // For ApplicationDbContext
using DHP_Manager.Models.Entities; // For Settings entity
using DHP_Manager.Repositories.Interface; // For ISettingsRepository
using Microsoft.EntityFrameworkCore; // For FirstOrDefaultAsync and EF Core functionality
using System.Threading.Tasks; // For Task
using System.Linq; // For LINQ methods (optional, but good practice to include explicitly)

namespace DHP_Manager.Repositories.Impl
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public SettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetProfitMarginAsync()
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "ProfitMargin");

            if (setting == null)
            {
                var defaultMargin = 0.3m; // 30% default
                await _context.Settings.AddAsync(new Settings { Key = "ProfitMargin", Value = defaultMargin });
                await _context.SaveChangesAsync();
                return defaultMargin;
            }
            return setting.Value;
        }

        public async Task UpdateProfitMarginAsync(decimal newMargin)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "ProfitMargin");

            if (setting == null)
            {
                setting = new Settings { Key = "ProfitMargin", Value = newMargin };
                await _context.Settings.AddAsync(setting);
            }
            else
            {
                setting.Value = newMargin;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTVAMarginAsync()
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "TVAMargin");

            if (setting == null)
            {
                var defaultMargin = 0.19m; // 19% default (common TVA rate, adjust as needed)
                await _context.Settings.AddAsync(new Settings { Key = "TVAMargin", Value = defaultMargin });
                await _context.SaveChangesAsync();
                return defaultMargin;
            }
            return setting.Value;
        }

        public async Task UpdateTVAMarginAsync(decimal newMargin)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "TVAMargin");

            if (setting == null)
            {
                setting = new Settings { Key = "TVAMargin", Value = newMargin };
                await _context.Settings.AddAsync(setting);
            }
            else
            {
                setting.Value = newMargin;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetDroitDuTimbrePriceAsync()
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "DroitDuTimbrePrice");

            if (setting == null)
            {
                var defaultPrice = 0.6m; // 0.6 TND default (common in Tunisia, adjust as needed)
                await _context.Settings.AddAsync(new Settings { Key = "DroitDuTimbrePrice", Value = defaultPrice });
                await _context.SaveChangesAsync();
                return defaultPrice;
            }
            return setting.Value;
        }

        public async Task UpdateDroitDuTimbrePriceAsync(decimal newPrice)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "DroitDuTimbrePrice");

            if (setting == null)
            {
                setting = new Settings { Key = "DroitDuTimbrePrice", Value = newPrice };
                await _context.Settings.AddAsync(setting);
            }
            else
            {
                setting.Value = newPrice;
            }
            await _context.SaveChangesAsync();
        }
    }
}