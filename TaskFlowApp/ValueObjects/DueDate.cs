namespace TaskFlowApp.ValueObjects;

public record DueDate
{
    public DateTime Deadline { get; init; }

    public DueDate(DateTime value, bool isPastDueDateAllowed = false)
    {
        if (!isPastDueDateAllowed && value.Date < DateTime.Today)
        {
            throw new ArgumentException("Due date cannot be in past");
        }
        
        Deadline = value;
    }
    public override string ToString()
    {
        return Deadline.ToShortDateString();
    }


}
