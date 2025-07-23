using OzdamarDepo.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzdamarDepo.Domain.Genel_Ayarlar
{
    public class AppSetting:Entity
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public AppSettingValueType ValueType { get; set; }

    }
}
