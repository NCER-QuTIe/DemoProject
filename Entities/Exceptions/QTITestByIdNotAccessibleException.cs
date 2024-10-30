namespace Entities.Exceptions;

public class QTITestByIdNotAccessibleException(Guid id) : NotFoundException($"QTITest with id {id} can't be accessed by you.");
