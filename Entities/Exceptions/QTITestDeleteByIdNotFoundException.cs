namespace Entities.Exceptions;

public class QTITestDeleteByIdNotFoundException(Guid id) : NotFoundException($"QTITest with id {id} doesn't exist in the database");
