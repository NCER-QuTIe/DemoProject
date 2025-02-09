namespace Entities.Exceptions.QTITest;

public class QTITestByNameNotFoundException(string name) : NotFoundException($"QTITest with id {name} doesn't exist in the database");
