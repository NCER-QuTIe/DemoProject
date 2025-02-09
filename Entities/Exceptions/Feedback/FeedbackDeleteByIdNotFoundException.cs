namespace Entities.Exceptions.Feedback;

public class FeedbackDeleteByIdNotFoundException(Guid id) : NotFoundException($"Feedback with id {id} doesn't exist in the database");
