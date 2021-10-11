using System;

namespace SharedKernel.Models
{
    public interface IDateTracking:IEntity
    {
        DateTimeOffset CreatedDate { get; }
        DateTimeOffset LastUpdatedDate { get; }
    }

    public abstract class DateTrackingEntity : Entity, IDateTracking
    {
        public DateTimeOffset CreatedDate { get; private set; }
        public DateTimeOffset LastUpdatedDate { get; private set; }
    }
}
