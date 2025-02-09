namespace Entities.Exceptions.Feedback;

public class ExternalTestByNameNotFoundException(string name) : NotFoundException($"ExternalTest with id {name} doesn't exist in the database");
