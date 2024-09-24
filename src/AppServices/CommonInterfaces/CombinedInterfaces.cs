namespace AirWeb.AppServices.CommonInterfaces;

public interface ICloseableAndDeletable : IDeletable, ICloseable;

public interface IHasOwnerAndDeletable : IHasOwner, IDeletable;
