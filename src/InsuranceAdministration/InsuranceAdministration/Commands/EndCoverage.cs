using System;
using Orleankka.Meta;

namespace InsuranceAdministration.Commands
{
    [Serializable]
    class EndCoverage : Command
    {
        public Guid Id { get; }
        public DateTime EndDate { get; }

        public EndCoverage(Guid id, DateTime endDate)
        {
            Id = id;
            EndDate = endDate;
        }
    }
}