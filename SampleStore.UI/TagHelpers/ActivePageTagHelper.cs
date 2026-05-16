using System;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SampleStore.UI.TagHelpers;

[HtmlTargetElement(Attributes = "active-page")]
public class ActivePageTagHelper : TagHelper
{
    private const string ActiveClass = "active";
    private const string ActivePageDelimiter = ",";
    private const string ClassAttributeName = "class";
    private const string ClassDelimiter = " ";

    public string ActivePage { get; set; } = string.Empty;

    [ViewContext]
    public ViewContext ViewContext { get; set; } = null!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var currentPage = Path.GetFileNameWithoutExtension(ViewContext.ActionDescriptor.DisplayName);
        var activePages = ActivePage.Split(ActivePageDelimiter, StringSplitOptions.RemoveEmptyEntries);
        if (activePages.All(activePage => !string.Equals(currentPage, activePage, StringComparison.OrdinalIgnoreCase)))
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
}