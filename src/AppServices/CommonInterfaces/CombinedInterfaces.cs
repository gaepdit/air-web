namespace AirWeb.AppServices.CommonInterfaces;

public interface IIsClosedAndIsDeleted : IIsDeleted, IIsClosed;

public interface IHasOwnerAndDeletable : IHasOwner, IDeletable;
