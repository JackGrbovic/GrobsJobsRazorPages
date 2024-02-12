using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrobsJobsRazorPages.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string ErrorMessage { get; set; }
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public void OnGet(string errorMessage)
    {
        if (errorMessage != null){
            ErrorMessage = errorMessage;
        }
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}

