using AutoFixture.Xunit2;
using FluentAssertions;
using Pokedex.Domain.Abstractions;
using Pokedex.Infrastructure.FunTranslationsApis;
using RichardSzalay.MockHttp;
using System.Net;

namespace Pokedex.Infrastructure.UnitTests.FunTranslationsApis;

public sealed class FunTranslationsApiHttpClientTests
{
  [Fact]
  public async Task ApplyYodaTranslationAsync_Throws_ArgumentNullException_When_Text_Is_Null()
  {
    // ARRANGE
    var json = Helpers.ReadTestApiResponseFile(fileName: "YodaTranslationApiResponse.json");

    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.When(HttpMethod.Post, "https://server.example.com/translate/yoda")
            .Respond("application/json", json);

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

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

  [Theory]
  [AutoData]
  public async Task ApplyYodaTranslationAsync_Returns_Failure_Result_When_FunTranslationsApi_Returns_429(Text text)
  {
    // ARRANGE
    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.Expect(HttpMethod.Post, "https://server.example.com/translate/yoda")
            .Respond(HttpStatusCode.TooManyRequests);

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

    var sut = new FunTranslationsApiHttpClient(httpClient);

    // ACT
    var result = await sut.ApplyYodaTranslationAsync(text, CancellationToken.None);

    // ASSERT
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeFalse();

    // check error
    var error = result.Error;

    error.Should().Be(FunTranslationsApiErrors.TooManyRequests);

    // check executed HTTP requests
    mockHttp.VerifyNoOutstandingExpectation();
  }

  [Theory]
  [AutoData]
  public async Task ApplyYodaTranslationAsync_Returns_Failure_Result_When_FunTranslationsApi_Returns_200_With_Null_Json_Content(Text text)
  {
    // ARRANGE
    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.Expect(HttpMethod.Post, "https://server.example.com/translate/yoda")
            .Respond("application/json", "null");

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

    var sut = new FunTranslationsApiHttpClient(httpClient);

    // ACT
    var result = await sut.ApplyYodaTranslationAsync(text, CancellationToken.None);

    // ASSERT
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeFalse();

    // check error
    var error = result.Error;

    error.Should().Be(FunTranslationsApiErrors.NullResponseJsonContent);

    // check executed HTTP requests
    mockHttp.VerifyNoOutstandingExpectation();
  }

  [Theory]
  [InlineAutoData(HttpStatusCode.BadRequest)]
  [InlineAutoData(HttpStatusCode.Unauthorized)]
  [InlineAutoData(HttpStatusCode.InternalServerError)]
  public async Task ApplyYodaTranslationAsync_Throws_HttpRequestException_When_FunTranslationsApi_Returns_Non_Success_Status_Code(
    HttpStatusCode statusCode,
    Text text)
  {
    // ARRANGE
    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.Expect(HttpMethod.Post, "https://server.example.com/translate/yoda")
            .Respond(statusCode);

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

    var sut = new FunTranslationsApiHttpClient(httpClient);

    // ACT
    _ = await Assert.ThrowsAsync<HttpRequestException>(
      () => sut.ApplyYodaTranslationAsync(text, CancellationToken.None)
    );

    // ASSERT
    mockHttp.VerifyNoOutstandingExpectation();
  }

  [Fact]
  public async Task ApplyShakespeareTranslationAsync_Throws_ArgumentNullException_When_Text_Is_Null()
  {
    // ARRANGE
    var json = Helpers.ReadTestApiResponseFile(fileName: "ShakespeareTranslationApiResponse.json");

    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.When(HttpMethod.Post, "https://server.example.com/translate/shakespeare")
            .Respond("application/json", json);

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

    var sut = new FunTranslationsApiHttpClient(httpClient);

    // ACT
    var exception = await Assert.ThrowsAsync<ArgumentNullException>(
      () => sut.ApplyShakespeareTranslationAsync(null!, CancellationToken.None)
    );

    // ASSERT
    exception.ParamName.Should().Be("text");
  }

  [Theory]
  [AutoData]
  public async Task ApplyShakespeareTranslationAsync_Returns_Success_Result_When_FunTranslationsApi_Returns_200(Text text)
  {
    // ARRANGE
    var json = Helpers.ReadTestApiResponseFile(fileName: "ShakespeareTranslationApiResponse.json");

    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.Expect(HttpMethod.Post, "https://server.example.com/translate/shakespeare")
            .Respond("application/json", json);

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

    var sut = new FunTranslationsApiHttpClient(httpClient);

    // ACT
    var result = await sut.ApplyShakespeareTranslationAsync(text, CancellationToken.None);

    // ASSERT
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeTrue();

    // check result value
    var translation = result.Value;

    translation.Contents.Should().NotBeNull();
    translation.Contents.Translated.Should().Be("Thee did giveth mr. Tim a hearty meal,  but unfortunately what he did doth englut did maketh him kicketh the bucket.");

    // check executed HTTP requests
    mockHttp.VerifyNoOutstandingExpectation();
  }

  [Theory]
  [AutoData]
  public async Task ApplyShakespeareTranslationAsync_Returns_Failure_Result_When_FunTranslationsApi_Returns_429(Text text)
  {
    // ARRANGE
    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.Expect(HttpMethod.Post, "https://server.example.com/translate/shakespeare")
            .Respond(HttpStatusCode.TooManyRequests);

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

    var sut = new FunTranslationsApiHttpClient(httpClient);

    // ACT
    var result = await sut.ApplyShakespeareTranslationAsync(text, CancellationToken.None);

    // ASSERT
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeFalse();

    // check error
    var error = result.Error;

    error.Should().Be(FunTranslationsApiErrors.TooManyRequests);

    // check executed HTTP requests
    mockHttp.VerifyNoOutstandingExpectation();
  }

  [Theory]
  [AutoData]
  public async Task ApplyShakespeareTranslationAsync_Returns_Failure_Result_When_FunTranslationsApi_Returns_200_With_Null_Json_Content(Text text)
  {
    // ARRANGE
    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.Expect(HttpMethod.Post, "https://server.example.com/translate/shakespeare")
            .Respond("application/json", "null");

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

    var sut = new FunTranslationsApiHttpClient(httpClient);

    // ACT
    var result = await sut.ApplyShakespeareTranslationAsync(text, CancellationToken.None);

    // ASSERT
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeFalse();

    // check error
    var error = result.Error;

    error.Should().Be(FunTranslationsApiErrors.NullResponseJsonContent);

    // check executed HTTP requests
    mockHttp.VerifyNoOutstandingExpectation();
  }

  [Theory]
  [InlineAutoData(HttpStatusCode.BadRequest)]
  [InlineAutoData(HttpStatusCode.Unauthorized)]
  [InlineAutoData(HttpStatusCode.InternalServerError)]
  public async Task ApplyShakespeareTranslationAsync_Throws_HttpRequestException_When_FunTranslationsApi_Returns_Non_Success_Status_Code(
    HttpStatusCode statusCode,
    Text text)
  {
    // ARRANGE
    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.Expect(HttpMethod.Post, "https://server.example.com/translate/shakespeare")
            .Respond(statusCode);

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

    var sut = new FunTranslationsApiHttpClient(httpClient);

    // ACT
    _ = await Assert.ThrowsAsync<HttpRequestException>(
      () => sut.ApplyShakespeareTranslationAsync(text, CancellationToken.None)
    );

    // ASSERT
    mockHttp.VerifyNoOutstandingExpectation();
  }
}
