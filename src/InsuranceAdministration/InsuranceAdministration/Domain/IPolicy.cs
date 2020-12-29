using Orleankka;
using Orleans;

namespace InsuranceAdministration.Domain
{
    internal interface IPolicy : IActorGrain, IGrainWithStringKey { }
}