namespace Entities.Exceptions.ExternalTest;

public class ExternalTestByNameNotFoundException(string name) : NotFoundException($"ExternalTest with id {name} doesn't exist in the database");
