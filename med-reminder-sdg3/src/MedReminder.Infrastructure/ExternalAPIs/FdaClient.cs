using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using MedReminder.Core.DTOs;

namespace MedReminder.Infrastructure.ExternalAPIs;

public class FdaClient
{
    private readonly HttpClient _httpClient;

    public FdaClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.fda.gov/");
    }

    public async Task<List<FdaDrugDto>> SearchDrugsAsync(string query)
    {
        var result = new List<FdaDrugDto>();
        try
        {
            // Search openfda.brand_name
            var response = await _httpClient.GetAsync($"drug/label.json?search=openfda.brand_name:*{query}*&limit=10");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);
                var resultsArray = json.RootElement.GetProperty("results");

                foreach (var item in resultsArray.EnumerateArray())
                {
                    if (item.TryGetProperty("openfda", out var openfda))
                    {
                        var brandName = openfda.TryGetProperty("brand_name", out var bn) ? bn[0].GetString() : string.Empty;
                        var genericName = openfda.TryGetProperty("generic_name", out var gn) ? gn[0].GetString() : string.Empty;
                        
                        var activeIngredient = string.Empty;
                        if (item.TryGetProperty("active_ingredient", out var ai))
                        {
                            activeIngredient = ai[0].GetString();
                        }

                        if (!string.IsNullOrEmpty(brandName))
                        {
                            result.Add(new FdaDrugDto
                            {
                                BrandName = brandName!,
                                GenericName = genericName ?? string.Empty,
                                ActiveIngredient = activeIngredient ?? string.Empty
                            });
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            // Log exception here in a real app
        }

        return result.DistinctBy(d => d.BrandName).ToList();
    }
}
