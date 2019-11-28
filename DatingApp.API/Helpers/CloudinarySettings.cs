using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using DatingApp.Domain.Services;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace DatingApp.API.Helpers
{
    public class CloudinarySettings
    {
        public string CloudName { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}