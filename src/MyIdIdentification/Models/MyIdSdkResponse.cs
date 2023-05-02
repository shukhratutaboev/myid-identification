using System.Text.Json.Serialization;

namespace MyIdIdentification.Models;

public class MyIdSdkResponse
{
    [JsonPropertyName("profile")]
    public MyIdProfileResponse Profile { get; set; }
}