using System.Collections.Generic;

namespace Attender.Server.Application.SubCategories.Commands.CreateUserSubCategories
{
    public class CreateUserSubCategoriesDto
    {
        public ICollection<int> SubCategoryIds { get; set; } = new List<int>();
    }
}
