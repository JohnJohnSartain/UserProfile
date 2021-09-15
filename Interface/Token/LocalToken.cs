using Blazored.LocalStorage;

namespace Interface.Token;

public interface ILocalToken
{
    Task RemoveToken();
    Task SetToken(string token);
    Task<string> GetTokenAsync();
}

public class LocalToken : ILocalToken
{
    private readonly ILocalStorageService _localStorageService;
    private string NameOfToken = nameof(ObjectName.Token);

    public LocalToken(ILocalStorageService localStorageService) => _localStorageService = localStorageService;

    public async Task RemoveToken() => await _localStorageService.RemoveItemAsync(NameOfToken);
    public async Task SetToken(string token) => await _localStorageService.SetItemAsync<string>(NameOfToken, token);
    public async Task<string> GetTokenAsync() => await _localStorageService.GetItemAsync<string>(NameOfToken);
}