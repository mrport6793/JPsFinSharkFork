using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Newtonsoft.Json;

namespace api.Service;

public class FMPService(HttpClient httpClient, IConfiguration config) : IFMPService
{
    public async Task<Stock?> FindStockBySymbolAsync(string symbol)
    {
        try
        {
            var result = await httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={config["FMPKey"]}");
            if (!result.IsSuccessStatusCode) return null;
            var content = await result.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
            var stock = tasks?[0];
            return stock?.ToStockFromFMP();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}