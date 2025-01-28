namespace PairUpCore.Exceptions.Role;

public class RoleNotFoundException : NotFoundException
{
    public override string Message => "Role not found.";

    public RoleNotFoundException()
    {
    }

    protected RoleNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public RoleNotFoundException(string? message) : base(message)
    {
    }

    public RoleNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}