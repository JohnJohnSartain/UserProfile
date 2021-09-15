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

public partial class ProfilePhotoDialog
{
    [Inject] ILocalStorageService LocalStorage { get; set; }

    [Inject] HttpClient HttpClient { get; set; }

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

        success = result.IsError;

        if (result.ValidationErrors != null && result.ValidationErrors.Any())
            errorList = result.ValidationErrors.ToArray();

        StateHasChanged();

        if (success.HasValue && success.Value)
            Submit();
    }

    private async Task<AutoWrapperResponseModel<string>> ChangeUserPassword()
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
        HttpResponseMessage response = await HttpClient.PatchAsync("User/Password", new StringContent(JsonConvert.SerializeObject(userModel), Encoding.UTF8, "application/json"));
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AutoWrapperResponseModel<string>>(responseContent);
    }

    private async Task<string> GetToken() => await GetFromLocalStorage(nameof(ObjectName.Token));
    private async Task<string> GetFromLocalStorage(string key) => await LocalStorage.GetItemAsync<string>(key);
}