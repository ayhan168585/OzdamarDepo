using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace OzdamarDepo.Application.Genel_Ayarlar
{
    public sealed record AppSettingUpdateCommand(string Key, string Value) : IRequest<Result<string>>;

}
