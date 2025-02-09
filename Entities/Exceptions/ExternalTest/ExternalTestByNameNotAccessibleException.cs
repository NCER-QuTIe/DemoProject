namespace Entities.Exceptions.ExternalTest;

public class ExternalTestByNameNotAccessibleException(string name) : NotFoundException($"ExternalTest with id {name} can't be accessed by you.");
