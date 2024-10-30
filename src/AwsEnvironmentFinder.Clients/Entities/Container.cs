using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwsEnvironmentFinder.Clients.Entities
{
    public class Container
    {
        public string Cluster { get; }
        public string ImageRepositoryName { get; }
        public string ImageTag { get; }
        public Dictionary<string, string> Variables { get; }
        public Dictionary<string, string> Results { get; }
        public bool HasResults => Results.Count > 0;

        public Container(string cluster, Amazon.ECS.Model.ContainerDefinition definition)
        {
            ImageRepositoryName = GetImageRepositoryName(definition.Image);
            ImageTag = GetImageTag(definition.Image);
            Cluster = cluster;
            Variables = definition.Environment.ToDictionary(k => k.Name, v => v.Value);
            Results = new Dictionary<string, string>();
        }

        public void Inspect(string key)
        {
            foreach (var variable in Variables)
            {
                if (variable.Value.Contains(key, StringComparison.InvariantCultureIgnoreCase))
                {
                    Results.Add(variable.Key, variable.Value);
                }
            }
        }

        public string GetResults()
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine($"{ImageRepositoryName}: {ImageTag}");
            sb.AppendLine("Results:");

            foreach (var variable in Results)
            {
                sb.AppendLine($"                {variable.Key}: {variable.Value}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string GetImageRepositoryName(string imageUri)
        {
            var image = imageUri[(imageUri.IndexOf("/") + 1)..];
            return image.Split(":")[0];
        }

        private static string GetImageTag(string imageUri)
        {
            var image = imageUri[(imageUri.IndexOf("/") + 1)..];
            return image.Split(":")[1];
        }
    }
}
