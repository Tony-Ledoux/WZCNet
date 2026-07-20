namespace WZCNet.src.Domain.Interfaces;
public interface ISoftDeletable
{
    DateTime? DeletedAt {get;set;}
}