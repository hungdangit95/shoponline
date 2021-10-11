namespace SharedKernel.Models
{
    public interface IModifierTrackingEntity
    {
        string CreatedById { get; }
        string LastUpdatedById { get; }
    }

    public abstract class ModifierTrackingEntity : DateTrackingEntity, IModifierTrackingEntity
    {
        public string CreatedBy { get; private set; }
        public string CreatedById { get; private set; }

        public string LastUpdatedBy { get; private set; }
        public string LastUpdatedById { get; private set; }

        public void MarkCreated(string authorId, string authorName)
        {
            CreatedBy = authorName;
            LastUpdatedBy = authorName;
            CreatedById = authorId;
            LastUpdatedById = authorId;
        }

        public void MarkModified(string authorId, string authorName)
        {
            LastUpdatedBy = authorName;
            LastUpdatedById = authorId;
        }
    }
}
