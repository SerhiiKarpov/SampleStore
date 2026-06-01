using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SampleStore.Common.Extensions;
using SampleStore.Services.Identity.Constants;
using SampleStore.Data;
using SampleStore.Data.Entities.Domain;
using SampleStore.Data.Extensions;

namespace SampleStore.UI.Pages.Products;

[Authorize(Roles = Roles.Admin)]
public class CreateModel : PageModelBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)) ;
    }

    [BindProperty]
    public Product? Product { get; set; }

    [BindProperty]
    [Display(Name = "Photo")]
    public IFormFile? PhotoFile { get; set; }

    public override string Title => "Create";

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (PhotoFile is { Length: > 0 })
        {
            using var memoryStream = new MemoryStream();
            await PhotoFile.CopyToAsync(memoryStream);

            var photo = new Photo
            {
                Image = memoryStream.ToArray(),
                MimeType = PhotoFile.ContentType,
            };

            _unitOfWork.GetRepository<Photo>().Add(photo);
            Product!.PhotoId = photo.Id;
        }

        _unitOfWork.GetRepository<Product>().Add(Product!);
        await _unitOfWork.SaveChanges();

        return RedirectToPage("./Index");
    }
}