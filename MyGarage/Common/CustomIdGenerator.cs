using MyGarage.Interfaces;

namespace MyGarage.Common;

public class CustomIdGenerator : IIdGenerator
{
    public Guid NewGuid()
    {
        return Guid.NewGuid();
    }
}