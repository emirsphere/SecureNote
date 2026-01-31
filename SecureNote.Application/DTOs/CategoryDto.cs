using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureNote.Application.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; } = string.Empty;
    }
    
    public class UpdateCategoryRequest
    {
        public string CategoryName { get; set; } = string.Empty;
    }
}
