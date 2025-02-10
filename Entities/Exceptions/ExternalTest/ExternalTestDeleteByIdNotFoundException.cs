namespace Entities.Exceptions.ExternalTest;

public class ExternalTestDeleteByIdNotFoundException(Guid id) : NotFoundException($"ExternalTest with id {id} doesn't exist in the database");
