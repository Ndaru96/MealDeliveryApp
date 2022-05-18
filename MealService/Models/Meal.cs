using System;
using System.Collections.Generic;

namespace MealService.Models
{
    public partial class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Stock { get; set; }
        public int Price { get; set; }
    }
}
