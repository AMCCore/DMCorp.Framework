using System.ComponentModel.DataAnnotations;

namespace DMCorp.Framework.Basics.DAL;

public interface IEntityBase
{
    [Key]
    Guid Id { get; set; }

    long LastUpdateTick { get; set; }
}