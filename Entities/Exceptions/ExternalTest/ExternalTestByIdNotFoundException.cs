namespace Entities.Exceptions.ExternalTest;

public class ExternalTestByIdNotFoundException(Guid id) : NotFoundException($"ExternalTest with id {id} doesn't exist in the database");
