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

namespace Contracts.Contracts.SafeMath.ContractDefinition
{


    public partial class SafeMathDeployment : SafeMathDeploymentBase
    {
        public SafeMathDeployment() : base(BYTECODE) { }
        public SafeMathDeployment(string byteCode) : base(byteCode) { }
    }

    public class SafeMathDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60566023600b82828239805160001a607314601657fe5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea26469706673582212206ce572fd965bd5b123e0ca52b77bb8ccd8945a508ea2e7781cfe7bbb633daf3964736f6c634300060c0033";
        public SafeMathDeploymentBase() : base(BYTECODE) { }
        public SafeMathDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
