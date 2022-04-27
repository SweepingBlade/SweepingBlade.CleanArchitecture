namespace SweepingBlade.Infrastructure.Domain;

public delegate void PropertyChangedDelegate<in T>(string propertyName, T oldValue, T newValue);