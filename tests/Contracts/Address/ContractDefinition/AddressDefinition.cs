using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace Contracts.Contracts.Address.ContractDefinition
{


    public partial class AddressDeployment : AddressDeploymentBase
    {
        public AddressDeployment() : base(BYTECODE) { }
        public AddressDeployment(string byteCode) : base(byteCode) { }
    }

    public class AddressDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60566023600b82828239805160001a607314601657fe5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea2646970667358221220a87ebf499f0b5d86c5bb0ea350ac94bb55a61edfb8decc3fe43d080f5921ffb964736f6c634300060c0033";
        public AddressDeploymentBase() : base(BYTECODE) { }
        public AddressDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
