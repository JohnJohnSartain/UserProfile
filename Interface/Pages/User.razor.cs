using Interface.Token;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Interface.Pages;

public partial class User
{
    [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; }
    [Inject] TokenAuthenticationStateProvider AuthStateProvider { get; set; }
    [Inject] NavigationManager navigationManager { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!((await AuthenticationState).User.Identity.IsAuthenticated))
        {
            await AuthStateProvider.SetTokenAsync();
            navigationManager.NavigateTo("/login");
        }
    }
}