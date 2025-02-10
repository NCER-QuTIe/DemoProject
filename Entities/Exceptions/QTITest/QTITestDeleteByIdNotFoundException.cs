namespace Entities.Exceptions.QTITest;

public class QTITestDeleteByIdNotFoundException(Guid id) : NotFoundException($"QTITest with id {id} doesn't exist in the database");
