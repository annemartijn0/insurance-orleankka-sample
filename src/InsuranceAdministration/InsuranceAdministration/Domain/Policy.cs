using System;
using System.Collections.Generic;
using System.Linq;
using InsuranceAdministration.Commands;
using InsuranceAdministration.DomainEvents;
using InsuranceAdministration.Queries;
using Orleankka.Meta;

namespace InsuranceAdministration.Domain
{
    class Policy : EventSourcedActor, IPolicy
    {
        public bool Active { get; private set; }
        public string PolicyHolder { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Coverage> Coverages { get; set; } = new List<Coverage>();

        IEnumerable<Event> Handle(CreatePolicy cmd)
        {
            if (Active)
            {
                throw new InvalidOperationException($"Policy with id {Id} has been already created");
            }

            yield return new PolicyCreated(cmd.PolicyHolder, cmd.StartDate, cmd.EndDate, cmd.Coverages);
        }

        IEnumerable<Event> Handle(EndCoverage cmd)
        {
            if (Coverages.All(c => c.Id != cmd.Id))
            {
                throw new InvalidOperationException($"Coverage with id {cmd.Id} does not exist");
            }
            yield return new CoverageEnded(cmd.Id, cmd.EndDate);
        }

        void On(PolicyCreated created)
        {
            StartDate = created.StartDate;
            EndDate = created.EndDate;
            Coverages = created.Coverages;
            PolicyHolder = created.PolicyHolder;
            Active = true;
        }

        void On(CoverageEnded coverageEnded)
        {
            var coverage = Coverages.First(c => c.Id == coverageEnded.Id);
            coverage.EndDate = coverageEnded.EndDate;
        }

        PolicyDetails Handle(GetDetails query)
        {
            return new PolicyDetails(Active, PolicyHolder, StartDate, EndDate, Coverages);
        }
    }
}
