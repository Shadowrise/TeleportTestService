using System.Collections.Generic;
using System.Threading.Tasks;
using AirportsService.Business.Airports.Queries.GetDistance;
using FluentAssertions;
using Xunit;

namespace AirportsService.Tests.Business.Airports;

public class GetDistanceValidatorTests
{
    private const string _validIataCode = "LED";
    public static IEnumerable<object[]> InvalidIataCodes =>
        new []
        {
            new object[] { null },
            new object[] { "" },
            new object[] { "led" },
            new object[] { "123" },
            new object[] { "LeD" },
            new object[] { "LEDD" }
        };
    
    [Theory]
    [InlineData(_validIataCode)]
    async Task GetDistanceValidator_ValidationOkForValidCodes(string code)
    {
        var validator = new GetDistanceValidator();
        var data = new GetDistance()
        {
            Code1 = code,
            Code2 = code
        };

        var res = await validator.ValidateAsync(data);
        
        res.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [MemberData(nameof(InvalidIataCodes))]
    async Task GetDistanceValidator_ValidationFailsWhenCode1HasInvalidFormat(string code1)
    {
        var validator = new GetDistanceValidator();
        var data = new GetDistance()
        {
            Code1 = code1,
            Code2 = _validIataCode
        };

        var res = await validator.ValidateAsync(data);
        
        res.IsValid.Should().BeFalse();
    }
    
    [Theory]
    [MemberData(nameof(InvalidIataCodes))]
    async Task GetDistanceValidator_ValidationFailsWhenCode2HasInvalidFormat(string code2)
    {
        var validator = new GetDistanceValidator();
        var data = new GetDistance()
        {
            Code1 = _validIataCode,
            Code2 = code2
        };

        var res = await validator.ValidateAsync(data);
        
        res.IsValid.Should().BeFalse();
    }
}