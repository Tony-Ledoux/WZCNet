using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WZCNet.src.Domain.Interfaces;

namespace WZCNet.src.Domain.Entities;

public class BaseEntity: ISoftDeletable, IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
    public DateTime? UpdatedAt {get;set;}
    public DateTime? DeletedAt {get;set;}
}
