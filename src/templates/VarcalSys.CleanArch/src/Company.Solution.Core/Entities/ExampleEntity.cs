namespace Company.Solution.Core.Entities;

public class ExampleEntity : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    private ExampleEntity() { }

    public static ExampleEntity Create(string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new ExampleEntity { Name = name, Description = description };
    }

    public void Update(string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        Description = description;
        SetUpdatedAt();
    }
}
