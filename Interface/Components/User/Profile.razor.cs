using Blazored.LocalStorage;
using Interface.Models;
using Interface.Token;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Interface.Components.User;

public partial class Profile
{
    [Inject] ILocalStorageService LocalStorage { get; set; }

    [Inject] HttpClient HttpClient { get; set; }

    [Inject] ISnackbar Snackbar { get; set; }

    private UserModel userModel = new();
    private bool? success;
    private ValidationError[] errorList { get; set; }

    [Inject] IDialogService DialogService { get; set; }

    private void ShowChangeUserPasswordDialogBox()
    {
        var parameters = new DialogParameters
        {
            { "UserId", userModel.Id }
        };

        DialogService.Show<ChangePasswordDialog>("Change Password", parameters);
    }

    protected override async Task OnParametersSetAsync() => userModel = await GetUserModelAsync();

    private async Task<UserModel> GetUserModelAsync()
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
        return (await HttpClient.GetFromJsonAsync<AutoWrapperResponseModel<UserModel>>("User/Self/Profile")).Result;
    }

    private async void OnValidSubmit(EditContext context)
    {
        var result = await UpdateUser();

        success = !result.IsError;

        if (success.Value) DisplaySnackbar("Success", Severity.Success);
        else DisplaySnackbar("Error", Severity.Error);

        if (result.ValidationErrors != null && result.ValidationErrors.Any())
            errorList = result.ValidationErrors.ToArray();

        if (errorList != null)
            foreach (var error in errorList)
                DisplaySnackbar(error.Name + ": " + error.Reason, Severity.Error);

        StateHasChanged();
    }

    private async Task<AutoWrapperResponseModel<string>> UpdateUser()
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
        HttpResponseMessage response = await HttpClient.PatchAsync("User", new StringContent(JsonConvert.SerializeObject(userModel), Encoding.UTF8, "application/json"));
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AutoWrapperResponseModel<string>>(responseContent);
    }

    private async Task<string> GetToken() => await GetFromLocalStorage(nameof(ObjectName.Token));
    private async Task<string> GetFromLocalStorage(string key) => await LocalStorage.GetItemAsync<string>(key);

    private void DisplaySnackbar(string message, Severity severity)
    {
        Snackbar.Configuration.SnackbarVariant = Variant.Filled;
        Snackbar.Add(message, severity);
    }
}