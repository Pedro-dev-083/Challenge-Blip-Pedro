using static System.Net.WebRequestMethods;

namespace Challenge_Blip.WebApi.Models;

public class Repository
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Html_url { get; set; }
    public string? Image_background { get; set; } = "https://blipchallenge.s3.sa-east-1.amazonaws.com/repository.png";
}
