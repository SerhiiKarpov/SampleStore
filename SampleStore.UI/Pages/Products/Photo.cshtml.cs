using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SampleStore.Data;
using SampleStore.Data.Entities.Domain;
using SampleStore.Data.Extensions;

namespace SampleStore.UI.Pages.Products;

[AllowAnonymous]
public class PhotoModel : PageModelBase
{
    private readonly IQueryMaterializer _queryMaterializer;

    private readonly IUnitOfWork _unitOfWork;

    public PhotoModel(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _queryMaterializer = queryMaterializer ?? throw new ArgumentNullException(nameof(queryMaterializer));
    }

    public override string Title => "Photo";

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var photo = await _unitOfWork.GetRepository<Photo>().FindById(id, _queryMaterializer);

        if (photo?.Image == null)
        {
            return NotFound();
        }

        return File(photo.Image, photo.MimeType);
    }
}
