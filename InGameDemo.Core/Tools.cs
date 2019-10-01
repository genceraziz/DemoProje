using InGameDemo.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InGameDemo.Core
{
    public static class Tools
    {
        public static string GenerateCategories(IEnumerable<CategoryViewForDto> categories)
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine("<ul id=\"my-categories\">");

            foreach (var category in categories.Where(x => x.IsParent && !x.ParentId.HasValue))
            {
                html.AppendLine("<li>");
                html.AppendLine("<a href=\"Product/"+ category.Id +"\"><span class=\"glyphicon glyphicon-chevron-right\"></span>" + category.Name + "</a>");
                if (categories.Any(a => a.ParentId == category.Id))
                {
                    html.AppendLine(GenerateSubCategory(categories, category));
                }

                html.AppendLine("</li>");
            }

            html.AppendLine("</ul>");

            return html.ToString();
        }

        private static string GenerateSubCategory(IEnumerable<CategoryViewForDto> categories, CategoryViewForDto parentCategory)
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine("<ul class=\"my-sub-categories\">");

            foreach (var category in categories.Where(x => x.ParentId == parentCategory.Id))
            {
                var icon = category.IsParent ? "<span class=\"glyphicon glyphicon-chevron-right\"></span>" : "<span class=\"glyphicon glyphicon-minus\"></span>";
                html.AppendLine("<li>");
                html.AppendLine("<a href=\"Product/"+ category.Id +"\">" + icon + category.Name + "</a>");
                if (categories.Any(a => a.ParentId == category.Id))
                {
                    html.AppendLine(GenerateSubCategory(categories, category));
                }

                html.AppendLine("</li>");
            }

            html.AppendLine("</ul>");

            return html.ToString();
        }
    }
}
