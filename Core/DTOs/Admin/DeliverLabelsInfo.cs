using DataLayer.Entities.Supplementary;
using System.Collections.Generic;

namespace Core.DTOs.Admin
{
    public class DeliverLabelsInfo
    {
        public List<DeliverLabel> DeliverLabels { get; set; }
        public InitialInfo InitialInfo { get; set; }
        public string Filter { get; set; }
    }
}
