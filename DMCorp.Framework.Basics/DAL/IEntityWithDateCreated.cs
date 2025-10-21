namespace DMCorp.Framework.Basics.DAL;

public interface IEntityWithDateCreated : IEntityBase
{
    DateTimeOffset DateCreated { get; set; }
}