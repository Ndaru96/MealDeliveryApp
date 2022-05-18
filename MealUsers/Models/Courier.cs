using System;
using System.Collections.Generic;

namespace UserService.Models
{
    public partial class Courier
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Cost { get; set; }
    }
}
