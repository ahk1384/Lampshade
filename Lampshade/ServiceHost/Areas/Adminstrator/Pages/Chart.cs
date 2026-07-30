using Newtonsoft.Json;

namespace ServiceHost.Areas.Adminstrator.Pages;

public class Chart
{
    public Chart()
    {
        labels = new List<string>();
        Items = new List<chartItem>();
    }

    public List<String> labels { get; set; }
    public List<chartItem> Items { get; set; }
}

public class chartItem
{
    [JsonProperty(PropertyName = "borderSkipped")]
    public bool borderSkipped = false;

    [JsonProperty(PropertyName = "label")] public string Label { get; set; }

    [JsonProperty(PropertyName = "data")] public List<double> Data { get; set; }

    [JsonProperty(PropertyName = "backgroundColor")]
    public string[] BackgroundColor { get; set; }

    [JsonProperty(PropertyName = "borderColor")]
    public string[] BorderColor { get; set; }

    [JsonProperty(PropertyName = "fill")] public bool fill { get; set; }

    [JsonProperty(PropertyName = "tension")]
    public double tension { get; set; }

    [JsonProperty(PropertyName = "borderWidth")]
    public int borderWidth { get; set; } = 2;

    [JsonProperty(PropertyName = "borderRadius")]
    public int borderRadius { get; set; } = 5;
}