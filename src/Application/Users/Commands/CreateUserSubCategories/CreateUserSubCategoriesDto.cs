using System.Collections.Generic;

namespace Attender.Server.Application.Users.Commands.CreateUserSubCategories
{
    public class CreateUserSubCategoriesDto
    {
        public IEnumerable<int> SubCategoryIds { get; set; } = new List<int>();
    }
}
