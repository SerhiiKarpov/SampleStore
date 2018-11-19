namespace SampleStore.UI.Pages.Products
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using SampleStore.Common;
    using SampleStore.Data;
    using SampleStore.Data.Entities.Domain;
    using SampleStore.Data.Extensions;

    /// <summary>
    /// Class encapsulating edit model.
    /// </summary>
    /// <seealso cref="PageModel" />
    public class EditModel : PageModelBase
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
        /// Initializes a new instance of the <see cref="EditModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="queryMaterializer">The query materializer.</param>
        public EditModel(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer)
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
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get
            {
                return "Edit";
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

            Product = await _unitOfWork.GetRepository<Product>().FindById(id.Value, _queryMaterializer);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        /// <summary>
        /// Called when [post asynchronous].
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _unitOfWork.GetRepository<Product>().Update(Product, _queryMaterializer);

            try
            {
                await _unitOfWork.SaveChanges();
            }
            catch (ConcurrencyException)
            {
                if (!await ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        /// <summary>
        /// Products the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if product exists, <c>false</c> otherwise.</returns>
        private async Task<bool> ProductExists(Guid id)
        {
            var productQuery = _unitOfWork.GetRepository<Product>().Query.Where(p => p.Id == id);
            return await _queryMaterializer.Any(productQuery);
        }

        #endregion Methods
    }
}