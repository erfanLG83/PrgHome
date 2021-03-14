using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrgHome.Web.Models
{
    public class ModelValid
    {
        public ModelValid()
        {
            Errors = new List<string>();
            Valid = true;
        }
        public List<string> Errors { get; set; }
        public bool Valid { get; set; }
    }
}
