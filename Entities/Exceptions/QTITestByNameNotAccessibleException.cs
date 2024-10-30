namespace Entities.Exceptions;

public class QTITestByNameNotAccessibleException(string name) : NotFoundException($"QTITest with id {name} can't be accessed by you.");
