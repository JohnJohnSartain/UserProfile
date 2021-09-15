using Interface.Entities;
using Microsoft.AspNetCore.Components;

namespace Interface.Components.User;

public partial class RolesCheckboxes
{
    [Parameter] public string[] UsersRoles { get; set; }

    private bool IsGodAccount { get; set; }
    private bool IsServiceAccount { get; set; }
    private bool IsAdministratorAccount { get; set; }
    private bool IsUserAccount { get; set; }

    protected override void OnParametersSet()
    {
        if (UsersRoles != null)
        {
            IsGodAccount = UsersRoles.Contains(Role.God);
            IsServiceAccount = UsersRoles.Contains(Role.Service);
            IsAdministratorAccount = UsersRoles.Contains(Role.Administrator);
            IsUserAccount = UsersRoles.Contains(Role.User);
        }
    }
}