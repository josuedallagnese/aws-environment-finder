using System.ComponentModel.DataAnnotations;

namespace AwsEnvironmentFinder.Configuration
{
    public class AwsCredentialOptions
    {
        public const string SectionName = "AwsCredential";

        [Required]
        public string Region { get; set; }
        [Required]
        public string AccessKey { get; set; }
        [Required]
        public string SecretKey { get; set; }
    }
}
