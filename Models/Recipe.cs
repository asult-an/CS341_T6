using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavorFlaveProto.Models
{
    internal class Recipe
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        List<Ingredient> Ingredients { get; set; }

        public TimeSpan TimeToPrepare { get; set; }



    }
}
