using Blazored.LocalStorage;
using Interface.Models;
using Interface.Token;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Interface.Components.Users;

public partial class UsersTable
{
    private IEnumerable<UserModel> UserModels = new List<UserModel>();

    [Inject] ILocalStorageService LocalStorage { get; set; }
    [Inject] HttpClient HttpClient { get; set; }

    protected override async Task OnInitializedAsync() => await PopulateTableWithLatest();

    private async Task PopulateTableWithLatest() => UserModels = await GetUserModelsAsync();

    private async Task<IEnumerable<UserModel>> GetUserModelsAsync()
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
        return (await HttpClient.GetFromJsonAsync<AutoWrapperResponseModel<IEnumerable<UserModel>>>("User")).Result;
    }

    private async Task<string> GetToken() => await GetFromLocalStorage(nameof(ObjectName.Token));
    private async Task<string> GetFromLocalStorage(string key) => await LocalStorage.GetItemAsync<string>(key);

    private static string GetLastLogin(DateTime[] authenticationHistory) =>
        authenticationHistory == null || authenticationHistory.Length == 0
            ? "Never"
            : authenticationHistory[^1].ToString();
}