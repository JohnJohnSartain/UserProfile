using Interface.Token;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Interface.Pages;

public partial class Index
{
    [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; }
    [Inject] TokenAuthenticationStateProvider AuthStateProvider { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!((await AuthenticationState).User.Identity.IsAuthenticated))
        {
            await AuthStateProvider.SetTokenAsync();
            NavigationManager.NavigateTo("/login");
        }
    }
}