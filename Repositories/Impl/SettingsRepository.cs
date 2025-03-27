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
                await _context.Settings.AddAsync(new Settings { Key = "ProfitMargin", Value = defaultMargin.ToString() });
                await _context.SaveChangesAsync();
                return defaultMargin;
            }
            return decimal.Parse(setting.Value);
        }

        public async Task UpdateProfitMarginAsync(decimal newMargin)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "ProfitMargin");

            if (setting == null)
            {
                setting = new Settings { Key = "ProfitMargin", Value = newMargin.ToString() };
                await _context.Settings.AddAsync(setting);
            }
            else
            {
                setting.Value = newMargin.ToString();
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
                await _context.Settings.AddAsync(new Settings { Key = "TVAMargin", Value = defaultMargin.ToString() });
                await _context.SaveChangesAsync();
                return defaultMargin;
            }
            return decimal.Parse(setting.Value);
        }

        public async Task UpdateTVAMarginAsync(decimal newMargin)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "TVAMargin");

            if (setting == null)
            {
                setting = new Settings { Key = "TVAMargin", Value = newMargin.ToString() };
                await _context.Settings.AddAsync(setting);
            }
            else
            {
                setting.Value = newMargin.ToString();
            }
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetDroitDuTimbrePriceAsync()
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "DroitDuTimbrePrice");

            if (setting == null)
            {
                var defaultPrice = 1m; // 1 TND default (common in Tunisia, adjust as needed)
                await _context.Settings.AddAsync(new Settings { Key = "DroitDuTimbrePrice", Value = defaultPrice.ToString() });
                await _context.SaveChangesAsync();
                return defaultPrice;
            }
            return decimal.Parse(setting.Value);
        }

        public async Task UpdateDroitDuTimbrePriceAsync(decimal newPrice)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "DroitDuTimbrePrice");

            if (setting == null)
            {
                setting = new Settings { Key = "DroitDuTimbrePrice", Value = newPrice.ToString() };
                await _context.Settings.AddAsync(setting);
            }
            else
            {
                setting.Value = newPrice.ToString();
            }
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCompanyTelAsync(string newTel)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "CompanyTel");

            if (setting == null)
            {
                setting = new Settings { Key = "CompanyTel", Value = newTel };
                await _context.Settings.AddAsync(setting);
            }
            else
            {
                setting.Value = newTel;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetCompanyTelAsync()
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "CompanyTel");

            if (setting == null)
            {
                throw new KeyNotFoundException("CompanyTel setting not found.");
            }
            return setting.Value;
        }

        public async Task UpdateCompanyFiscalAsync(string newFiscal)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "CompanyFiscal");

            if (setting == null)
            {
                setting = new Settings { Key = "CompanyFiscal", Value = newFiscal };
                await _context.Settings.AddAsync(setting);
            }
            else
            {
                setting.Value = newFiscal;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetCompanyFiscalAsync()
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "CompanyFiscal");

            if (setting == null)
            {
                throw new KeyNotFoundException("CompanyFiscal setting not found.");
            }
            return setting.Value;
        }
        public async Task UpdateFactureIdAsync()
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "FactureId");

            if (setting == null)
            {
                // If it doesn't exist, create it with an initial value of 1
                setting = new Settings { Key = "FactureId", Value = "1" };
                await _context.Settings.AddAsync(setting);
            }
            else
            {
                // If it exists, increment the current value
                int count = int.Parse(setting.Value);
                setting.Value = (count + 1).ToString();
                _context.Settings.Update(setting); // Explicitly mark as updated (optional, EF tracks changes)
            }

            await _context.SaveChangesAsync();
        }
        public async Task<int> GetFactureIdAsync()
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "FactureId");

            if (setting == null)
            {
                throw new KeyNotFoundException("FactureId setting not found.");
            }
            return int.Parse(setting.Value);
        }

        public async Task UpdateCompanyRibAsync(string newRib)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "CompanyRIB");

            if (setting == null)
            {
                setting = new Settings { Key = "CompanyRIB", Value = newRib };
                await _context.Settings.AddAsync(setting);
            }
            else
            {
                setting.Value = newRib;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBankNameAsync(string newBankName)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == "BankName");

            if (setting == null)
            {
                setting = new Settings { Key = "BankName", Value = newBankName };
                await _context.Settings.AddAsync(setting);
            }
            else
            {
                setting.Value = newBankName;
            }

            await _context.SaveChangesAsync();
        }
        public async Task<string> GetCompanyRibAsync()
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Key == "CompanyRIB");

            if (setting == null)
            {
                throw new KeyNotFoundException("CompanyRIB setting not found.");
            }
            return setting.Value;
        }
        public async Task<string> GetBankNameAsync()
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Key == "BankName");

            if (setting == null)
            {
                throw new KeyNotFoundException("BankName setting not found.");
            }
            return setting.Value;
        }
    }
}