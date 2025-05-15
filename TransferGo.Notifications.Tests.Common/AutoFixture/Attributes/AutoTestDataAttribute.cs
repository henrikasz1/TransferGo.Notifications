using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Tests.Common.AutoFixture.Attributes;

public class AutoTestDataAttribute : AutoDataAttribute
{
    public AutoTestDataAttribute() : base(() => new Fixture()
        .Customize(new AutoMoqCustomization { ConfigureMembers = true }))
    {
    }
}