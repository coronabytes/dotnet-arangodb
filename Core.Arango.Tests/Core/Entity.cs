using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Tests.Core
{
    public class Entity
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Id { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Key { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Revision { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }
    }

    public class Client
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Id { get; set; }
        public string Key { get; set; }
        public string Revision { get; set; }
        public string Name { get; set; }
    }

    public class Project
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Id { get; set; }
        public string Key { get; set; }
        public string Revision { get; set; }
        public string Name { get; set; }
        public string ClientKey { get; set; }
        public string ParentKey { get; set; }
        public double Budget { get; set; }
    }

    public class Activity
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Id { get; set; }
        public string Key { get; set; }
        public string Revision { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public decimal Revenue { get; set; }
        public Guid ProjectKey { get; set; }
        public List<Note> Notes { get; set; }
    }

    public class Note
    {
        public DateTime CreatedOn { get; set; }
        public string Text { get; set; }
    }
}