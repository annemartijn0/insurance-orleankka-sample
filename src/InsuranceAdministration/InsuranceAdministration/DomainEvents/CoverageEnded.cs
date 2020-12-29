using System;
using Orleankka.Meta;

namespace InsuranceAdministration.DomainEvents
{
    [Serializable]
    class CoverageEnded : Event
    {
        public Guid Id { get; }
        public DateTime EndDate { get; }

        public CoverageEnded(Guid id, DateTime endDate)
        {
            Id = id;
            EndDate = endDate;
        }
    }
}