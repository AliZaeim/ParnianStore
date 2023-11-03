using DataLayer.Entities.Supplementary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.General
{
    public class MyQuestionsVM
    {
        public List<FAQ> FAQs { get; set; }
        public List<ContactMessage> ContactMessages { get; set; }
    }
}
