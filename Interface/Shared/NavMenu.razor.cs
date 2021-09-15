using Blazored.LocalStorage;
using Interface.Token;
using Microsoft.AspNetCore.Components;

namespace Interface.Shared
{
    public partial class NavMenu
    {
        [Inject] ILocalStorageService LocalStorage { get; set; }
        [Inject] TokenAuthenticationStateProvider AuthStateProvider { get; set; }

        private async Task Logout() => await AuthStateProvider.Logout();
    }
}