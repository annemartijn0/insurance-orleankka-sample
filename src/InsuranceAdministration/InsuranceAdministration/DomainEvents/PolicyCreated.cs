using System;
using System.Collections.Generic;
using InsuranceAdministration.Domain;
using Orleankka.Meta;

namespace InsuranceAdministration.DomainEvents
{
    [Serializable]
    class PolicyCreated : Event
    {
        public string PolicyHolder { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public List<Coverage> Coverages { get; }

        public PolicyCreated(string policyHolder, DateTime startDate, DateTime endDate, List<Coverage> coverages)
        {
            PolicyHolder = policyHolder;
            StartDate = startDate;
            EndDate = endDate;
            Coverages = coverages;
        }
    }
}
