namespace TaskFlowApp.ValueObjects;

public class DueDate
{
    public DateTime Deadline { get; }

    public DueDate(DateTime value, bool isPastDueDateAllowed = false)
    {
        if (!isPastDueDateAllowed)
        {
            if (value.Date < DateTime.Today)
            {
                Console.WriteLine("Due date cannot be in the past");
                return;
            }
            Deadline = value;
        }
        else
        {
            Deadline = value;
        }
    }
    public override string ToString()
    {
        return Deadline.ToShortDateString();
    }


}
