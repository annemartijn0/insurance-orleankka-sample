using System;
using System.Collections.Generic;
using InsuranceAdministration.Domain;
using Orleankka.Meta;

namespace InsuranceAdministration.Commands
{
    [Serializable]
    class CreatePolicy : Command
    {
        public string PolicyHolder { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Coverage> Coverages { get; set; } = new List<Coverage>();
    }
}