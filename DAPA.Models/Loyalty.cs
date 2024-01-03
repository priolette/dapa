using System.Text.Json.Serialization;

namespace DAPA.Models;

public class Loyalty
{
    public int ID { get; set; }
    
    public string? Start_date { get; set; }
    
    public int Discount { get; set; }
}