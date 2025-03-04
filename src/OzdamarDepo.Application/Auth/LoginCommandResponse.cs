using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzdamarDepo.Application.Auth
{
    public sealed record LoginCommandResponse
    {
        public string AccessToken { get; set; } = default!;

    }
}
