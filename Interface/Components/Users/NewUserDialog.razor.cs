using Blazored.LocalStorage;
using Interface.Models;
using Interface.Token;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Interface.Components.Users;

public partial class NewUserDialog
{
    [Inject] ILocalStorageService LocalStorage { get; set; }

    [Inject] HttpClient HttpClient { get; set; }

    private UserModel userModel = new();
    private bool? success;
    private ValidationError[] errorList { get; set; }

    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog.Cancel();

    private async void OnValidSubmit(EditContext context)
    {
        var result = await CreateUser();

        success = result.IsError;

        if (result.ValidationErrors != null && result.ValidationErrors.Any())
            errorList = result.ValidationErrors.ToArray();

        StateHasChanged();

        if (success.HasValue && success.Value)
            Submit();
    }

    private async Task<AutoWrapperResponseModel<string>> CreateUser()
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("User", userModel);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AutoWrapperResponseModel<string>>(responseContent);
    }

    private async Task<string> GetToken() => await GetFromLocalStorage(nameof(ObjectName.Token));
    private async Task<string> GetFromLocalStorage(string key) => await LocalStorage.GetItemAsync<string>(key);
}