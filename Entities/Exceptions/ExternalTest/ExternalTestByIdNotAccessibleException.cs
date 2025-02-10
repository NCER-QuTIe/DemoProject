namespace Entities.Exceptions.ExternalTest;

public class ExternalTestByIdNotAccessibleException(Guid id) : NotFoundException($"ExternalTest with id {id} can't be accessed by you.");
