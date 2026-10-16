using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HeroKit.Tests.Samples;

// Calls the sample API in samples/HeroKit.Samples.WebApi so the sample stays correct.
public class WebApiSampleTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task People_ShouldBindTcknAndRejectInvalidNumbers()
    {
        JsonElement person = await _client.GetFromJsonAsync<JsonElement>("/people/10000000146");

        Assert.Equal("10*******46", person.GetProperty("id").GetString());
        Assert.Equal(HttpStatusCode.BadRequest, (await _client.GetAsync("/people/10000000147")).StatusCode);
    }

    [Fact]
    public async Task Provinces_ShouldListDistricts()
    {
        JsonElement province = await _client.GetFromJsonAsync<JsonElement>("/provinces/istanbul");

        Assert.Equal(34, province.GetProperty("code").GetInt32());
        Assert.Equal(39, province.GetProperty("districts").GetArrayLength());
        Assert.Equal(HttpStatusCode.NotFound, (await _client.GetAsync("/provinces/atlantis")).StatusCode);
    }

    [Fact]
    public async Task Amounts_ShouldReadTurkishAmounts()
    {
        JsonElement amount = await _client.GetFromJsonAsync<JsonElement>("/amounts?text=" + Uri.EscapeDataString("₺1.234,56"));

        Assert.Equal(1234.56m, amount.GetProperty("amount").GetDecimal());
        Assert.Equal("bin iki yüz otuz dört Türk lirası elli altı kuruş", amount.GetProperty("words").GetString());
        Assert.Equal(HttpStatusCode.BadRequest, (await _client.GetAsync("/amounts?text=1.5")).StatusCode);
    }

    [Theory]
    [InlineData("89504E470D0A1A0A0000000D49484452", "photo.png", HttpStatusCode.OK, null)]
    [InlineData("4D5A90000300000004000000FFFF0000", "photo.png", HttpStatusCode.BadRequest, "DangerousContent")]
    [InlineData("89504E470D0A1A0A0000000D49484452", "photo.php.png", HttpStatusCode.BadRequest, "DoubleExtension")]
    [InlineData("504B0304140000000800000021000000", "archive.zip", HttpStatusCode.BadRequest, "NotAllowed")]
    public async Task Documents_ShouldValidateUploads(string hex, string fileName, HttpStatusCode status, string? rejection)
    {
        using var form = new MultipartFormDataContent();
        var file = new ByteArrayContent(Convert.FromHexString(hex.PadRight(128, '0')));
        file.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
        form.Add(file, "file", fileName);

        HttpResponseMessage response = await _client.PostAsync("/documents", form);

        Assert.Equal(status, response.StatusCode);
        if (rejection is not null)
            Assert.Equal(rejection, (await response.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("rejection").GetString());
    }
}
