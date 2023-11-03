using DataLayer.Entities.Store;
using System.Collections.Generic;

namespace Core.DTOs.General
{
    public class PackageGroupVM
    {
        public List<PackageGroup> PackageGroups { get; set; }
        public string Type { get; set; }
    }
}
