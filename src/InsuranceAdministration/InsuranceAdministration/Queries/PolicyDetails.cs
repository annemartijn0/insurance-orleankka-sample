using System;
using System.Collections.Generic;
using InsuranceAdministration.Domain;

namespace InsuranceAdministration.Queries
{
    [Serializable]
    class PolicyDetails
    {
        public bool Active { get; }
        public string PolicyHolder { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public List<Coverage> Coverages { get; }

        public PolicyDetails(bool active, string policyHolder, DateTime startDate, DateTime endDate, List<Coverage> coverages)
        {
            Active = active;
            PolicyHolder = policyHolder;
            StartDate = startDate;
            EndDate = endDate;
            Coverages = coverages;
        }
    }
}