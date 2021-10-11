using MediatR;
using System.Collections.Generic;

namespace SharedKernel.Models
{
    public interface IChangeTrackingEntity
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }
        void ClearDomainEvents();
        void AddDomainEvent(INotification eventItem);
    }
}
