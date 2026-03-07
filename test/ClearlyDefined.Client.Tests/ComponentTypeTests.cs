namespace ClearlyDefined.Client.Tests;

using AwesomeAssertions;
using ClearlyDefined.Schema;

public sealed class ComponentTypeTests
{
    [Theory]
    [MemberData(nameof(AllComponentTypes))]
    public void ToApiString_DoesNotThrow_ForAllComponentTypes(ComponentType type)
    {
        var result = type.ToApiString();

        _ = result.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(AllComponentProviders))]
    public void ToApiString_DoesNotThrow_ForAllComponentProviders(ComponentProvider provider)
    {
        var result = provider.ToApiString();

        _ = result.Should().NotBeNull();
    }

    public static TheoryData<ComponentType> AllComponentTypes()
    {
        var data = new TheoryData<ComponentType>();
        foreach (var value in Enum.GetValues<ComponentType>())
        {
            data.Add(value);
        }

        return data;
    }

    public static TheoryData<ComponentProvider> AllComponentProviders()
    {
        var data = new TheoryData<ComponentProvider>();
        foreach (var value in Enum.GetValues<ComponentProvider>())
        {
            data.Add(value);
        }

        return data;
    }
}
