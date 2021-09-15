using Interface.Token;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Interface.Components.Users;

public partial class NewUserDialogOpenButton
{
    [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; }
    [Inject] TokenAuthenticationStateProvider AuthStateProvider { get; set; }
    [Inject] NavigationManager navigationManager { get; set; }
    [Inject] IDialogService DialogService { get; set; }

    private void OpenCreateNewUserDialog() => DialogService.Show<Interface.Components.Users.NewUserDialog>("Simple Dialog");

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!((await AuthenticationState).User.Identity.IsAuthenticated))
        {
            await AuthStateProvider.SetTokenAsync();
            navigationManager.NavigateTo("/login");
        }
    }
}