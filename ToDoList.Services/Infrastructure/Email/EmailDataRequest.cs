using Newtonsoft.Json;
using SendGrid.Helpers.Mail;

namespace ToDoList.Services.Infrastructure.Email;

public class EmailDataRequest
{
    public EmailDataRequest()
    {
        Year = DateTime.Now.Year.ToString();
    }

    [JsonProperty("heading")]
    public string Heading { get; set; }
    [JsonProperty("body")]
    public string Body { get; set; }
    [JsonProperty("url")]
    public string URL { get; set; }

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("buttonText")]

    public string ButtonText { get; set; }
    [JsonProperty("htmlTemplate")]
    public string HtmlTemplate { get; set; }
    [JsonProperty("year")]
    public string Year { get; set; }
    [JsonProperty("schoolName")]
    public string AppName { get; set; }
    public EmailAddress From { get; set; }
    public EmailAddress To { get; set; }
}