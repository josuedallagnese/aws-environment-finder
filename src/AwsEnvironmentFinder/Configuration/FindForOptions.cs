using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwsEnvironmentFinder.Configuration
{
    public class FindForOptions
    {
        public const string SectionName = "FindFor";

        [Required, MinLength(1)]
        public List<string> Keys { get; set; } = new List<string>();
    }
}
