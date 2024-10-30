namespace Entities.Exceptions;

public class QTITestByNameNotFoundException(string name) : NotFoundException($"QTITest with id {name} doesn't exist in the database");
