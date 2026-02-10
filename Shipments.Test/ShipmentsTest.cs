using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shipments.Tests;

public class ApiSmokeTests
{
    private TestWebApplicationFactory _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _factory = new TestWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task GET_shipments_should_return_200()
    {
        var response = await _client.GetAsync("/api/shipments");

        var body = await response.Content.ReadAsStringAsync();

        TestContext.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
        TestContext.WriteLine(body);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task POST_shipments_should_return_201_and_created_payload()
    {
        var json = """
        {
        "referenceNumber": "REF-TEST-001",
        "sender": "Test Sender",
        "recipient": "Test Recipient"
        }
        """;

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/shipments", content);

        var body = await response.Content.ReadAsStringAsync();

        TestContext.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
        TestContext.WriteLine(body);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
}