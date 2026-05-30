using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

using SampleStore.Services.Identity;
using SampleStore.UI.Pages;
using SampleStore.UI.ViewModels.Identity;

namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage;

public class IndexModel(
    IUserManager userManager,
    ISignInManager signInManager,
    IEmailSender emailSender) : PageModelBase
{
    private readonly IEmailSender _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
    private readonly ISignInManager _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    private readonly IUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    [BindProperty]
    public AccountViewModel? Input { get; set; }

    public bool IsEmailConfirmed { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }

    public override string Title => "Profile";

    public string? Username { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        Username = user.FullName;

        Input = new AccountViewModel
        {
            Name = user.FullName,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };

        IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (Input!.Name != user.FullName)
        {
            user.FullName = Input.Name;
        }

        if (Input.DateOfBirth != user.DateOfBirth)
        {
            user.DateOfBirth = Input.DateOfBirth;
        }

        if (Input.Email != user.Email)
        {
            var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
            if (!setEmailResult.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
            }
        }

        if (Input.PhoneNumber != user.PhoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }
        }

        await _userManager.UpdateAsync(user);

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostSendVerificationEmailAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
            pageHandler: null,
            values: new { user.Id, code },
            protocol: Request.Scheme);
        await _emailSender.SendEmailAsync(
            user.Email,
            "Confirm your email",
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.");

        StatusMessage = "Verification email sent. Please check your email.";
        return RedirectToPage();
    }
}