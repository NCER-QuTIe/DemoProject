namespace Entities.Exceptions;

public class QTITestByIdNotFoundException(Guid id) : NotFoundException($"QTITest with id {id} doesn't exist in the database");
