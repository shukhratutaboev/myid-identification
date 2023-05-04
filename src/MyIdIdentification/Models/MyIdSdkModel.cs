using System.Text.Json.Serialization;

namespace MyIdIdentification.Models;

public class MyIdSdkModel
{
    [JsonPropertyName("profile")]
    public MyIdProfileModel Profile { get; set; }
}