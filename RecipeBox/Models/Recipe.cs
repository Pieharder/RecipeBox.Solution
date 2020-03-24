using System.Collections.Generic;

namespace RecipeBox.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.Categories = new HashSet<CategoryItem>();
        }

        public int CategoryId { get; set; }
        public string Description { get; set; }
        public virtual ApplicationUser User { get; set; }

        public ICollection<CategoryItem> Categories { get;}
    }
}