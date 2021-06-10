using Nethereum.ABI.FunctionEncoding.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tests.DTOs.Events
{
 
    [Event("Transfer")]
    class ClaimBNBSuccessfullyEvent
    {
        [Parameter("address", "recipient")]
        public string Recipient { get; set; }

        [Parameter("uint256", "ethReceived")]
        public string EthReceived { get; set; }

        [Parameter("uint256", "nextAvailableClaimDate")]
        public BigInteger NextAvailableClaimDate { get; set; }
    }
}
