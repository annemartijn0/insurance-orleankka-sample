using System;
namespace InsuranceAdministration.Domain
{
    internal class Coverage
    {
        public string CoverageCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SelfInsurance SelfInsurance { get; set; }
        public string InsuredPerson { get; set; }
        public Guid Id { get; set; }
    }
}