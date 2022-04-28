using System;

namespace SweepingBlade.Infrastructure.Domain.Tests;

public class PersonNameChanged : IEvent<PersonNameChanged>
{
    public string NewName { get; }
    public string OldName { get; }

    public PersonNameChanged(string oldName, string newName)
    {
        OldName = oldName ?? throw new ArgumentNullException(nameof(oldName));
        NewName = newName ?? throw new ArgumentNullException(nameof(newName));
    }
}