namespace SampleStore.UI.Pages.Products
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using SampleStore.Common;
    using SampleStore.Data;
    using SampleStore.Data.Entities.Domain;
    using SampleStore.Data.Extensions;

    /// <summary>
    /// Class encapsulating create model.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    public class CreateModel : PageModelBase
    {
        #region Fields

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork.ThrowIfArgumentIsNull(nameof(unitOfWork)) ;
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
                return "Create";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when [get].
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult OnGet()
        {
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

            _unitOfWork.GetRepository<Product>().Add(Product);
            await _unitOfWork.SaveChanges();

            return RedirectToPage("./Index");
        }

        #endregion Methods
    }
}