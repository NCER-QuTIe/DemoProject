namespace Entities.Exceptions.Feedback;

public class ExternalTestByNameNotAccessibleException(string name) : NotFoundException($"QTITest with id {name} can't be accessed by you.");
