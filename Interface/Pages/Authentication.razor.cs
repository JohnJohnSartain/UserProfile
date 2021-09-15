using Blazored.LocalStorage;
using Interface.Token;
using Microsoft.AspNetCore.Components;

namespace Interface.Pages;

public partial class Authentication
{
    [Inject] ILocalStorageService LocalStorage { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (!JwtToken.IsEmptyOrInvalid(await GetToken())) NavigationManager.NavigateTo("/");
    }

    private async Task<string> GetToken() => await GetFromLocalStorage(nameof(ObjectName.Token));

    private async Task<string> GetFromLocalStorage(string key) => await LocalStorage.GetItemAsync<string>(key);
}