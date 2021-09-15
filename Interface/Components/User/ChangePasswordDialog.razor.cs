using Blazored.LocalStorage;
using Interface.Models;
using Interface.Token;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Interface.Components.User;

public partial class ChangePasswordDialog
{
    [Inject] ILocalStorageService LocalStorage { get; set; }

    [Inject] HttpClient HttpClient { get; set; }

    [Inject] ISnackbar Snackbar { get; set; }


    private UserModel userModel = new() { Username = "username" };
    private bool? success;
    private ValidationError[] errorList { get; set; }

    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    [Parameter] public string UserId { get; set; }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog.Cancel();

    private async void OnValidSubmit(EditContext context)
    {
        userModel.Id = UserId;

        var result = await ChangeUserPassword();

        success = !result.IsError;

        if (result.ValidationErrors != null && result.ValidationErrors.Any())
            errorList = result.ValidationErrors.ToArray();

        if (result.ResponseException != null && result.ResponseException.ValidationErrors.Any())
            errorList = result.ResponseException.ValidationErrors.ToArray();

        if (errorList.Any())
            foreach (var item in errorList)
                DisplaySnackbar(item.Name + ": " + item.Reason, Severity.Error);

        StateHasChanged();

        if (success.HasValue && success.Value)
        {
            DisplaySnackbar("Success", Severity.Success);
            Submit();
        }
        else DisplaySnackbar("Error", Severity.Error);
    }

    private async Task<AutoWrapperResponseModel<string>> ChangeUserPassword()
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
        HttpResponseMessage response = await HttpClient.PatchAsync("User/Password", new StringContent(JsonConvert.SerializeObject(userModel), Encoding.UTF8, "application/json"));
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AutoWrapperResponseModel<string>>(responseContent);
    }

    private void DisplaySnackbar(string message, Severity severity)
    {
        Snackbar.Configuration.SnackbarVariant = Variant.Filled;
        Snackbar.Add(message, severity);
    }

    private async Task<string> GetToken() => await GetFromLocalStorage(nameof(ObjectName.Token));
    private async Task<string> GetFromLocalStorage(string key) => await LocalStorage.GetItemAsync<string>(key);
}