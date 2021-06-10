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

namespace Contracts.Contracts.Ownable.ContractDefinition
{


    public partial class OwnableDeployment : OwnableDeploymentBase
    {
        public OwnableDeployment() : base(BYTECODE) { }
        public OwnableDeployment(string byteCode) : base(byteCode) { }
    }

    public class OwnableDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public OwnableDeploymentBase() : base(BYTECODE) { }
        public OwnableDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class GeUnlockTimeFunction : GeUnlockTimeFunctionBase { }

    [Function("geUnlockTime", "uint256")]
    public class GeUnlockTimeFunctionBase : FunctionMessage
    {

    }

    public partial class LockFunction : LockFunctionBase { }

    [Function("lock")]
    public class LockFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "time", 1)]
        public virtual BigInteger Time { get; set; }
    }

    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }

    public partial class RenounceOwnershipFunction : RenounceOwnershipFunctionBase { }

    [Function("renounceOwnership")]
    public class RenounceOwnershipFunctionBase : FunctionMessage
    {

    }

    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("address", "newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }

    public partial class UnlockFunction : UnlockFunctionBase { }

    [Function("unlock")]
    public class UnlockFunctionBase : FunctionMessage
    {

    }

    public partial class OwnershipTransferredEventDTO : OwnershipTransferredEventDTOBase { }

    [Event("OwnershipTransferred")]
    public class OwnershipTransferredEventDTOBase : IEventDTO
    {
        [Parameter("address", "previousOwner", 1, true )]
        public virtual string PreviousOwner { get; set; }
        [Parameter("address", "newOwner", 2, true )]
        public virtual string NewOwner { get; set; }
    }

    public partial class GeUnlockTimeOutputDTO : GeUnlockTimeOutputDTOBase { }

    [FunctionOutput]
    public class GeUnlockTimeOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class OwnerOutputDTO : OwnerOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }






}
