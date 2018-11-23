namespace SampleStore.UI.Pages.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using SampleStore.Common.Extensions;
    using SampleStore.Data;
    using SampleStore.Data.Entities.Domain;
    using SampleStore.Data.Extensions;

    /// <summary>
    /// Class encapsulating delete model.
    /// </summary>
    /// <seealso cref="PageModel" />
    public class DeleteModel : PageModelBase
    {
        #region Fields

        /// <summary>
        /// The query materializer
        /// </summary>
        private readonly IQueryMaterializer _queryMaterializer;

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="queryMaterializer">The query materializer.</param>
        public DeleteModel(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer)
        {
            _unitOfWork = unitOfWork.ThrowIfArgumentIsNull(nameof(unitOfWork));
            _queryMaterializer = queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        [BindProperty]
        public Product Product
        {
            get; set;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public override string Title
        {
            get
            {
                return "Delete";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productQuery = _unitOfWork.GetRepository<Product>().Query.Where(m => m.Id == id);
            Product = await _queryMaterializer.FirstOrDefault(productQuery);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        /// <summary>
        /// Called when [post asynchronous].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var repository = _unitOfWork.GetRepository<Product>();
            Product = await repository.FindById(id.Value, _queryMaterializer);

            if (Product != null)
            {
                repository.Remove(Product);
                await _unitOfWork.SaveChanges();
            }

            return RedirectToPage("./Index");
        }

        #endregion Methods
    }
}