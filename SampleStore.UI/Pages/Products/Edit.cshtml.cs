using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SampleStore.Data;
using SampleStore.Services.Identity.Constants;
using SampleStore.Data.Entities.Domain;
using SampleStore.Data.Extensions;

namespace SampleStore.UI.Pages.Products;

[Authorize(Roles = Roles.Admin)]
public class EditModel : PageModelBase
{
    private readonly IQueryMaterializer _queryMaterializer;

    private readonly IUnitOfWork _unitOfWork;

    public EditModel(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _queryMaterializer = queryMaterializer ?? throw new ArgumentNullException(nameof(queryMaterializer));
    }

    [BindProperty]
    public Product? Product { get; set; }

    [BindProperty]
    [Display(Name = "Photo")]
    public IFormFile? PhotoFile { get; set; }

    public override string Title => "Edit";

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Product = await _unitOfWork.GetRepository<Product>().FindById(id.Value, _queryMaterializer);

        if (Product == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Guid? oldPhotoId = null;
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
            oldPhotoId = Product!.PhotoId;
            Product.PhotoId = photo.Id;
        }

        await _unitOfWork.Update(Product!, _queryMaterializer);

        try
        {
            await _unitOfWork.SaveChanges();
        }
        catch (ConcurrencyException)
        {
            if (!await ProductExists(Product!.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        if (oldPhotoId is { } photoIdToRemove)
        {
            var oldPhoto = await _unitOfWork.GetRepository<Photo>().FindById(photoIdToRemove, _queryMaterializer);
            if (oldPhoto != null)
            {
                _unitOfWork.GetRepository<Photo>().Remove(oldPhoto);
                await _unitOfWork.SaveChanges();
            }
        }

        return RedirectToPage("./Index");
    }

    private async Task<bool> ProductExists(Guid id)
    {
        var productQuery = _unitOfWork.GetRepository<Product>().Query.Where(p => p.Id == id);
        return await _queryMaterializer.Any(productQuery);
    }
}