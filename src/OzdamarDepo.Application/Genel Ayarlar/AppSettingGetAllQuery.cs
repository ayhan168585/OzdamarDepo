using MediatR;
using OzdamarDepo.Domain.Genel_Ayarlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzdamarDepo.Application.Genel_Ayarlar
{
    public sealed record AppSettingGetAllQuery() : IRequest<List<AppSetting>>;
}
