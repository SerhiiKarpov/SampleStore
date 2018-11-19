namespace SampleStore.UI.TagHelpers
{
    using System;
    using System.IO;
    using System.Text;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    /// <summary>
    /// Class encapsulating active page tag helper.
    /// </summary>
    /// <seealso cref="TagHelper" />
    [HtmlTargetElement(Attributes = "active-page")]
    public class ActivePageTagHelper : TagHelper
    {
        #region Fields

        /// <summary>
        /// The active class
        /// </summary>
        private const string ActiveClass = "active";

        /// <summary>
        /// The class attribute name
        /// </summary>
        private const string ClassAttributeName = "class";

        /// <summary>
        /// The class delimiter
        /// </summary>
        private const string ClassDelimiter = " ";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the active page.
        /// </summary>
        /// <value>
        /// The active page.
        /// </value>
        public string ActivePage
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the view context.
        /// </summary>
        /// <value>
        /// The view context.
        /// </value>
        [ViewContext]
        public ViewContext ViewContext
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Synchronously executes the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" /> with the given <paramref name="context" /> and
        /// <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var currentPage = Path.GetFileNameWithoutExtension(ViewContext.ActionDescriptor.DisplayName);
            if (!string.Equals(currentPage, ActivePage, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var classValue = new StringBuilder();
            if (context.AllAttributes.TryGetAttribute(ClassAttributeName, out var classAttribute))
            {
                classValue.Append(classAttribute.Value.ToString());
            }

            if (classValue.Length > 0)
            {
                classValue.Append(ClassDelimiter);
            }

            classValue.Append(ActiveClass);
            output.Attributes.SetAttribute(ClassAttributeName, classValue.ToString());
        }

        #endregion Methods
    }
}