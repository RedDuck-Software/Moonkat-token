using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using Contracts.Contracts.IPancakeFactory.ContractDefinition;

namespace Contracts.Contracts.IPancakeFactory
{
    public partial class IPancakeFactoryService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, IPancakeFactoryDeployment iPancakeFactoryDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<IPancakeFactoryDeployment>().SendRequestAndWaitForReceiptAsync(iPancakeFactoryDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, IPancakeFactoryDeployment iPancakeFactoryDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<IPancakeFactoryDeployment>().SendRequestAsync(iPancakeFactoryDeployment);
        }

        public static async Task<IPancakeFactoryService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, IPancakeFactoryDeployment iPancakeFactoryDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, iPancakeFactoryDeployment, cancellationTokenSource);
            return new IPancakeFactoryService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public IPancakeFactoryService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AllPairsQueryAsync(AllPairsFunction allPairsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AllPairsFunction, string>(allPairsFunction, blockParameter);
        }

        
        public Task<string> AllPairsQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var allPairsFunction = new AllPairsFunction();
                allPairsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<AllPairsFunction, string>(allPairsFunction, blockParameter);
        }

        public Task<BigInteger> AllPairsLengthQueryAsync(AllPairsLengthFunction allPairsLengthFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AllPairsLengthFunction, BigInteger>(allPairsLengthFunction, blockParameter);
        }

        
        public Task<BigInteger> AllPairsLengthQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AllPairsLengthFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> CreatePairRequestAsync(CreatePairFunction createPairFunction)
        {
             return ContractHandler.SendRequestAsync(createPairFunction);
        }

        public Task<TransactionReceipt> CreatePairRequestAndWaitForReceiptAsync(CreatePairFunction createPairFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createPairFunction, cancellationToken);
        }

        public Task<string> CreatePairRequestAsync(string tokenA, string tokenB)
        {
            var createPairFunction = new CreatePairFunction();
                createPairFunction.TokenA = tokenA;
                createPairFunction.TokenB = tokenB;
            
             return ContractHandler.SendRequestAsync(createPairFunction);
        }

        public Task<TransactionReceipt> CreatePairRequestAndWaitForReceiptAsync(string tokenA, string tokenB, CancellationTokenSource cancellationToken = null)
        {
            var createPairFunction = new CreatePairFunction();
                createPairFunction.TokenA = tokenA;
                createPairFunction.TokenB = tokenB;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createPairFunction, cancellationToken);
        }

        public Task<string> FeeToQueryAsync(FeeToFunction feeToFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<FeeToFunction, string>(feeToFunction, blockParameter);
        }

        
        public Task<string> FeeToQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<FeeToFunction, string>(null, blockParameter);
        }

        public Task<string> FeeToSetterQueryAsync(FeeToSetterFunction feeToSetterFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<FeeToSetterFunction, string>(feeToSetterFunction, blockParameter);
        }

        
        public Task<string> FeeToSetterQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<FeeToSetterFunction, string>(null, blockParameter);
        }

        public Task<string> GetPairQueryAsync(GetPairFunction getPairFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetPairFunction, string>(getPairFunction, blockParameter);
        }

        
        public Task<string> GetPairQueryAsync(string tokenA, string tokenB, BlockParameter blockParameter = null)
        {
            var getPairFunction = new GetPairFunction();
                getPairFunction.TokenA = tokenA;
                getPairFunction.TokenB = tokenB;
            
            return ContractHandler.QueryAsync<GetPairFunction, string>(getPairFunction, blockParameter);
        }

        public Task<string> SetFeeToRequestAsync(SetFeeToFunction setFeeToFunction)
        {
             return ContractHandler.SendRequestAsync(setFeeToFunction);
        }

        public Task<TransactionReceipt> SetFeeToRequestAndWaitForReceiptAsync(SetFeeToFunction setFeeToFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFeeToFunction, cancellationToken);
        }

        public Task<string> SetFeeToRequestAsync(string returnValue1)
        {
            var setFeeToFunction = new SetFeeToFunction();
                setFeeToFunction.ReturnValue1 = returnValue1;
            
             return ContractHandler.SendRequestAsync(setFeeToFunction);
        }

        public Task<TransactionReceipt> SetFeeToRequestAndWaitForReceiptAsync(string returnValue1, CancellationTokenSource cancellationToken = null)
        {
            var setFeeToFunction = new SetFeeToFunction();
                setFeeToFunction.ReturnValue1 = returnValue1;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFeeToFunction, cancellationToken);
        }

        public Task<string> SetFeeToSetterRequestAsync(SetFeeToSetterFunction setFeeToSetterFunction)
        {
             return ContractHandler.SendRequestAsync(setFeeToSetterFunction);
        }

        public Task<TransactionReceipt> SetFeeToSetterRequestAndWaitForReceiptAsync(SetFeeToSetterFunction setFeeToSetterFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFeeToSetterFunction, cancellationToken);
        }

        public Task<string> SetFeeToSetterRequestAsync(string returnValue1)
        {
            var setFeeToSetterFunction = new SetFeeToSetterFunction();
                setFeeToSetterFunction.ReturnValue1 = returnValue1;
            
             return ContractHandler.SendRequestAsync(setFeeToSetterFunction);
        }

        public Task<TransactionReceipt> SetFeeToSetterRequestAndWaitForReceiptAsync(string returnValue1, CancellationTokenSource cancellationToken = null)
        {
            var setFeeToSetterFunction = new SetFeeToSetterFunction();
                setFeeToSetterFunction.ReturnValue1 = returnValue1;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFeeToSetterFunction, cancellationToken);
        }
    }
}
