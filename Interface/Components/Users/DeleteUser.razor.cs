using Blazored.LocalStorage;
using Interface.Token;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace Interface.Components.Users;

public partial class DeleteUser
{
    [Inject] ILocalStorageService LocalStorage { get; set; }
    [Inject] HttpClient HttpClient { get; set; }

    [Parameter] public string UserId { get; set; }

    private async void Delete()
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

        var e = $"User/{UserId}";
        HttpResponseMessage response = await HttpClient.DeleteAsync(e);

        var responseContent = await response.Content.ReadAsStringAsync();
    }

    private async Task<string> GetToken() => await GetFromLocalStorage(nameof(ObjectName.Token));
    private async Task<string> GetFromLocalStorage(string key) => await LocalStorage.GetItemAsync<string>(key);
}