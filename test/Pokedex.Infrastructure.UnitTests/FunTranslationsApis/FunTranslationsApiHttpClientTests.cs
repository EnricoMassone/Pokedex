using AutoFixture.Xunit2;
using FluentAssertions;
using Pokedex.Domain.Abstractions;
using Pokedex.Infrastructure.FunTranslationsApis;
using RichardSzalay.MockHttp;

namespace Pokedex.Infrastructure.UnitTests.FunTranslationsApis;

public sealed class FunTranslationsApiHttpClientTests
{
  [Fact]
  public async Task ApplyYodaTranslationAsync_Throws_ArgumentNullException_When_Text_Is_Null()
  {
    // ARRANGE
    var json = Helpers.ReadTestApiResponseFile(fileName: "YodaTranslationApiResponse.json");

    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.When("yoda")
            .Respond("application/json", json);

    using var httpClient = mockHttp.ToHttpClient();

    var sut = new FunTranslationsApiHttpClient(httpClient);

    // ACT
    var exception = await Assert.ThrowsAsync<ArgumentNullException>(
      () => sut.ApplyYodaTranslationAsync(null!, CancellationToken.None)
    );

    // ASSERT
    exception.ParamName.Should().Be("text");
  }

  [Theory]
  [AutoData]
  public async Task ApplyYodaTranslationAsync_Returns_Success_Result_When_FunTranslationsApi_Returns_200(Text text)
  {
    // ARRANGE
    var json = Helpers.ReadTestApiResponseFile(fileName: "YodaTranslationApiResponse.json");

    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.Expect(HttpMethod.Post, "https://server.example.com/translate/yoda")
            .Respond("application/json", json);

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

    var sut = new FunTranslationsApiHttpClient(httpClient);

    // ACT
    var result = await sut.ApplyYodaTranslationAsync(text, CancellationToken.None);

    // ASSERT
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeTrue();

    // check result value
    var translation = result.Value;

    translation.Contents.Should().NotBeNull();
    translation.Contents.Translated.Should().Be("Lost a planet,  master obiwan has.");

    // check executed HTTP requests
    mockHttp.VerifyNoOutstandingExpectation();
  }
}
