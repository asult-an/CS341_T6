using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavorFlaveProto.Models
{
    internal class Ingredient
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public int[] Tags { get; }

        // todo: add method for add/removing tags
        

        public Ingredient(string Id, string Name, int[] tags) { 
        
        }

    }
}
