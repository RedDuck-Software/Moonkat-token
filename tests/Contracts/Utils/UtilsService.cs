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
using Contracts.Contracts.Utils.ContractDefinition;

namespace Contracts.Contracts.Utils
{
    public partial class UtilsService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, UtilsDeployment utilsDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<UtilsDeployment>().SendRequestAndWaitForReceiptAsync(utilsDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, UtilsDeployment utilsDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<UtilsDeployment>().SendRequestAsync(utilsDeployment);
        }

        public static async Task<UtilsService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, UtilsDeployment utilsDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, utilsDeployment, cancellationTokenSource);
            return new UtilsService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public UtilsService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> CalculateBNBRewardQueryAsync(CalculateBNBRewardFunction calculateBNBRewardFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CalculateBNBRewardFunction, BigInteger>(calculateBNBRewardFunction, blockParameter);
        }

        
        public Task<BigInteger> CalculateBNBRewardQueryAsync(BigInteger tTotal, BigInteger currentBalance, BigInteger currentBNBPool, BigInteger winningDoubleRewardPercentage, BigInteger totalSupply, string ofAddress, BlockParameter blockParameter = null)
        {
            var calculateBNBRewardFunction = new CalculateBNBRewardFunction();
                calculateBNBRewardFunction.TTotal = tTotal;
                calculateBNBRewardFunction.CurrentBalance = currentBalance;
                calculateBNBRewardFunction.CurrentBNBPool = currentBNBPool;
                calculateBNBRewardFunction.WinningDoubleRewardPercentage = winningDoubleRewardPercentage;
                calculateBNBRewardFunction.TotalSupply = totalSupply;
                calculateBNBRewardFunction.OfAddress = ofAddress;
            
            return ContractHandler.QueryAsync<CalculateBNBRewardFunction, BigInteger>(calculateBNBRewardFunction, blockParameter);
        }

        public Task<BigInteger> CalculateTopUpClaimQueryAsync(CalculateTopUpClaimFunction calculateTopUpClaimFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CalculateTopUpClaimFunction, BigInteger>(calculateTopUpClaimFunction, blockParameter);
        }

        
        public Task<BigInteger> CalculateTopUpClaimQueryAsync(BigInteger currentRecipientBalance, BigInteger basedRewardCycleBlock, BigInteger threshHoldTopUpRate, BigInteger amount, BlockParameter blockParameter = null)
        {
            var calculateTopUpClaimFunction = new CalculateTopUpClaimFunction();
                calculateTopUpClaimFunction.CurrentRecipientBalance = currentRecipientBalance;
                calculateTopUpClaimFunction.BasedRewardCycleBlock = basedRewardCycleBlock;
                calculateTopUpClaimFunction.ThreshHoldTopUpRate = threshHoldTopUpRate;
                calculateTopUpClaimFunction.Amount = amount;
            
            return ContractHandler.QueryAsync<CalculateTopUpClaimFunction, BigInteger>(calculateTopUpClaimFunction, blockParameter);
        }
    }
}
