namespace Contacts.WebUi.Models
{
    public class Settings : ISettings
    {
        public string ApiRootUri { get; set; }
        public string ContactBlobStorageAccount { get; set; }
        public string ContactImageContainerName { get; set; }
        public string ContactImageUrl { get; set; }
    }
}