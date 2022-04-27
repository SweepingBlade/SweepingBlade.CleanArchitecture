namespace SweepingBlade.Infrastructure.Domain.Tests;

public class Person : Entity
{
    private string _name;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value, OnNameChanged);
    }

    private void OnNameChanged(string propertyName, string oldValue, string newValue)
    {
        Invoke(new PersonNameChanged(oldValue, newValue));
    }
}