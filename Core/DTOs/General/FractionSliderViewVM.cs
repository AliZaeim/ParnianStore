using DataLayer.Entities.Supplementary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.General
{
    public class FractionSliderViewVM
    {
        public List<FSImage> FSImages { get; set; }
        public List<FSText> FSTexts { get; set; }
    }
}
