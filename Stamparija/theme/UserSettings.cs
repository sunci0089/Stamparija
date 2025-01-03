using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Stamparija.theme
{
    [Serializable]
    public class UserSettings
    {
        public string FontSize { get; set; } //small, medium, large
        public string Theme { get; set; } //light, dark
        public string Language { get; set; } //eng, srp
    }
}
