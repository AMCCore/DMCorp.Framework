namespace DMCorp.Framework.Basics.DAL;

public interface ISoftDeleteEntity : IEntityBase
{
    bool IsDeleted { get; set; }
}
