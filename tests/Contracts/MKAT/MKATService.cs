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
using Contracts.Contracts.MKAT.ContractDefinition;

namespace Contracts.Contracts.MKAT
{
    public partial class MKATService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, MKATDeployment mKATDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<MKATDeployment>().SendRequestAndWaitForReceiptAsync(mKATDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, MKATDeployment mKATDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<MKATDeployment>().SendRequestAsync(mKATDeployment);
        }

        public static async Task<MKATService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, MKATDeployment mKATDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, mKATDeployment, cancellationTokenSource);
            return new MKATService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public MKATService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> LiquidityFeeQueryAsync(LiquidityFeeFunction liquidityFeeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<LiquidityFeeFunction, BigInteger>(liquidityFeeFunction, blockParameter);
        }

        
        public Task<BigInteger> LiquidityFeeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<LiquidityFeeFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> MaxTxAmountQueryAsync(MaxTxAmountFunction maxTxAmountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MaxTxAmountFunction, BigInteger>(maxTxAmountFunction, blockParameter);
        }

        
        public Task<BigInteger> MaxTxAmountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MaxTxAmountFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> TaxFeeQueryAsync(TaxFeeFunction taxFeeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TaxFeeFunction, BigInteger>(taxFeeFunction, blockParameter);
        }

        
        public Task<BigInteger> TaxFeeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TaxFeeFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> ActivateContractRequestAsync(ActivateContractFunction activateContractFunction)
        {
             return ContractHandler.SendRequestAsync(activateContractFunction);
        }

        public Task<TransactionReceipt> ActivateContractRequestAndWaitForReceiptAsync(ActivateContractFunction activateContractFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(activateContractFunction, cancellationToken);
        }

        public Task<string> ActivateContractRequestAsync(BigInteger rewardCycleBlock)
        {
            var activateContractFunction = new ActivateContractFunction();
                activateContractFunction.RewardCycleBlock = rewardCycleBlock;
            
             return ContractHandler.SendRequestAsync(activateContractFunction);
        }

        public Task<TransactionReceipt> ActivateContractRequestAndWaitForReceiptAsync(BigInteger rewardCycleBlock, CancellationTokenSource cancellationToken = null)
        {
            var activateContractFunction = new ActivateContractFunction();
                activateContractFunction.RewardCycleBlock = rewardCycleBlock;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(activateContractFunction, cancellationToken);
        }

        public Task<BigInteger> AllowanceQueryAsync(AllowanceFunction allowanceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        
        public Task<BigInteger> AllowanceQueryAsync(string owner, string spender, BlockParameter blockParameter = null)
        {
            var allowanceFunction = new AllowanceFunction();
                allowanceFunction.Owner = owner;
                allowanceFunction.Spender = spender;
            
            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        public Task<string> ApproveRequestAsync(ApproveFunction approveFunction)
        {
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(ApproveFunction approveFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<string> ApproveRequestAsync(string spender, BigInteger amount)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(string spender, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> BalanceOfQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Account = account;
            
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public Task<bool> BlacklistQueryAsync(BlacklistFunction blacklistFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BlacklistFunction, bool>(blacklistFunction, blockParameter);
        }

        
        public Task<bool> BlacklistQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var blacklistFunction = new BlacklistFunction();
                blacklistFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<BlacklistFunction, bool>(blacklistFunction, blockParameter);
        }

        public Task<BigInteger> CalculateBNBRewardQueryAsync(CalculateBNBRewardFunction calculateBNBRewardFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CalculateBNBRewardFunction, BigInteger>(calculateBNBRewardFunction, blockParameter);
        }

        
        public Task<BigInteger> CalculateBNBRewardQueryAsync(string ofAddress, BlockParameter blockParameter = null)
        {
            var calculateBNBRewardFunction = new CalculateBNBRewardFunction();
                calculateBNBRewardFunction.OfAddress = ofAddress;
            
            return ContractHandler.QueryAsync<CalculateBNBRewardFunction, BigInteger>(calculateBNBRewardFunction, blockParameter);
        }

        public Task<string> ClaimBNBRewardRequestAsync(ClaimBNBRewardFunction claimBNBRewardFunction)
        {
             return ContractHandler.SendRequestAsync(claimBNBRewardFunction);
        }

        public Task<string> ClaimBNBRewardRequestAsync()
        {
             return ContractHandler.SendRequestAsync<ClaimBNBRewardFunction>();
        }

        public Task<TransactionReceipt> ClaimBNBRewardRequestAndWaitForReceiptAsync(ClaimBNBRewardFunction claimBNBRewardFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimBNBRewardFunction, cancellationToken);
        }

        public Task<TransactionReceipt> ClaimBNBRewardRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<ClaimBNBRewardFunction>(null, cancellationToken);
        }

        public Task<byte> DecimalsQueryAsync(DecimalsFunction decimalsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(decimalsFunction, blockParameter);
        }

        
        public Task<byte> DecimalsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(null, blockParameter);
        }

        public Task<string> DecreaseAllowanceRequestAsync(DecreaseAllowanceFunction decreaseAllowanceFunction)
        {
             return ContractHandler.SendRequestAsync(decreaseAllowanceFunction);
        }

        public Task<TransactionReceipt> DecreaseAllowanceRequestAndWaitForReceiptAsync(DecreaseAllowanceFunction decreaseAllowanceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(decreaseAllowanceFunction, cancellationToken);
        }

        public Task<string> DecreaseAllowanceRequestAsync(string spender, BigInteger subtractedValue)
        {
            var decreaseAllowanceFunction = new DecreaseAllowanceFunction();
                decreaseAllowanceFunction.Spender = spender;
                decreaseAllowanceFunction.SubtractedValue = subtractedValue;
            
             return ContractHandler.SendRequestAsync(decreaseAllowanceFunction);
        }

        public Task<TransactionReceipt> DecreaseAllowanceRequestAndWaitForReceiptAsync(string spender, BigInteger subtractedValue, CancellationTokenSource cancellationToken = null)
        {
            var decreaseAllowanceFunction = new DecreaseAllowanceFunction();
                decreaseAllowanceFunction.Spender = spender;
                decreaseAllowanceFunction.SubtractedValue = subtractedValue;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(decreaseAllowanceFunction, cancellationToken);
        }

        public Task<string> DeliverRequestAsync(DeliverFunction deliverFunction)
        {
             return ContractHandler.SendRequestAsync(deliverFunction);
        }

        public Task<TransactionReceipt> DeliverRequestAndWaitForReceiptAsync(DeliverFunction deliverFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deliverFunction, cancellationToken);
        }

        public Task<string> DeliverRequestAsync(BigInteger tAmount)
        {
            var deliverFunction = new DeliverFunction();
                deliverFunction.TAmount = tAmount;
            
             return ContractHandler.SendRequestAsync(deliverFunction);
        }

        public Task<TransactionReceipt> DeliverRequestAndWaitForReceiptAsync(BigInteger tAmount, CancellationTokenSource cancellationToken = null)
        {
            var deliverFunction = new DeliverFunction();
                deliverFunction.TAmount = tAmount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deliverFunction, cancellationToken);
        }

        public Task<BigInteger> DisruptiveCoverageFeeQueryAsync(DisruptiveCoverageFeeFunction disruptiveCoverageFeeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DisruptiveCoverageFeeFunction, BigInteger>(disruptiveCoverageFeeFunction, blockParameter);
        }

        
        public Task<BigInteger> DisruptiveCoverageFeeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DisruptiveCoverageFeeFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> DisruptiveTransferRequestAsync(DisruptiveTransferFunction disruptiveTransferFunction)
        {
             return ContractHandler.SendRequestAsync(disruptiveTransferFunction);
        }

        public Task<TransactionReceipt> DisruptiveTransferRequestAndWaitForReceiptAsync(DisruptiveTransferFunction disruptiveTransferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(disruptiveTransferFunction, cancellationToken);
        }

        public Task<string> DisruptiveTransferRequestAsync(string recipient, BigInteger amount)
        {
            var disruptiveTransferFunction = new DisruptiveTransferFunction();
                disruptiveTransferFunction.Recipient = recipient;
                disruptiveTransferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(disruptiveTransferFunction);
        }

        public Task<TransactionReceipt> DisruptiveTransferRequestAndWaitForReceiptAsync(string recipient, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var disruptiveTransferFunction = new DisruptiveTransferFunction();
                disruptiveTransferFunction.Recipient = recipient;
                disruptiveTransferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(disruptiveTransferFunction, cancellationToken);
        }

        public Task<BigInteger> DisruptiveTransferEnabledFromQueryAsync(DisruptiveTransferEnabledFromFunction disruptiveTransferEnabledFromFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DisruptiveTransferEnabledFromFunction, BigInteger>(disruptiveTransferEnabledFromFunction, blockParameter);
        }

        
        public Task<BigInteger> DisruptiveTransferEnabledFromQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DisruptiveTransferEnabledFromFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> ExcludeFromBlackListRequestAsync(ExcludeFromBlackListFunction excludeFromBlackListFunction)
        {
             return ContractHandler.SendRequestAsync(excludeFromBlackListFunction);
        }

        public Task<TransactionReceipt> ExcludeFromBlackListRequestAndWaitForReceiptAsync(ExcludeFromBlackListFunction excludeFromBlackListFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(excludeFromBlackListFunction, cancellationToken);
        }

        public Task<string> ExcludeFromBlackListRequestAsync(string addressToExclude)
        {
            var excludeFromBlackListFunction = new ExcludeFromBlackListFunction();
                excludeFromBlackListFunction.AddressToExclude = addressToExclude;
            
             return ContractHandler.SendRequestAsync(excludeFromBlackListFunction);
        }

        public Task<TransactionReceipt> ExcludeFromBlackListRequestAndWaitForReceiptAsync(string addressToExclude, CancellationTokenSource cancellationToken = null)
        {
            var excludeFromBlackListFunction = new ExcludeFromBlackListFunction();
                excludeFromBlackListFunction.AddressToExclude = addressToExclude;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(excludeFromBlackListFunction, cancellationToken);
        }

        public Task<string> ExcludeFromFeeRequestAsync(ExcludeFromFeeFunction excludeFromFeeFunction)
        {
             return ContractHandler.SendRequestAsync(excludeFromFeeFunction);
        }

        public Task<TransactionReceipt> ExcludeFromFeeRequestAndWaitForReceiptAsync(ExcludeFromFeeFunction excludeFromFeeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(excludeFromFeeFunction, cancellationToken);
        }

        public Task<string> ExcludeFromFeeRequestAsync(string account)
        {
            var excludeFromFeeFunction = new ExcludeFromFeeFunction();
                excludeFromFeeFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(excludeFromFeeFunction);
        }

        public Task<TransactionReceipt> ExcludeFromFeeRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var excludeFromFeeFunction = new ExcludeFromFeeFunction();
                excludeFromFeeFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(excludeFromFeeFunction, cancellationToken);
        }

        public Task<string> ExcludeLaunchpadFromFeesAndMaxTxRequestAsync(ExcludeLaunchpadFromFeesAndMaxTxFunction excludeLaunchpadFromFeesAndMaxTxFunction)
        {
             return ContractHandler.SendRequestAsync(excludeLaunchpadFromFeesAndMaxTxFunction);
        }

        public Task<TransactionReceipt> ExcludeLaunchpadFromFeesAndMaxTxRequestAndWaitForReceiptAsync(ExcludeLaunchpadFromFeesAndMaxTxFunction excludeLaunchpadFromFeesAndMaxTxFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(excludeLaunchpadFromFeesAndMaxTxFunction, cancellationToken);
        }

        public Task<string> ExcludeLaunchpadFromFeesAndMaxTxRequestAsync(string launchpadAddress)
        {
            var excludeLaunchpadFromFeesAndMaxTxFunction = new ExcludeLaunchpadFromFeesAndMaxTxFunction();
                excludeLaunchpadFromFeesAndMaxTxFunction.LaunchpadAddress = launchpadAddress;
            
             return ContractHandler.SendRequestAsync(excludeLaunchpadFromFeesAndMaxTxFunction);
        }

        public Task<TransactionReceipt> ExcludeLaunchpadFromFeesAndMaxTxRequestAndWaitForReceiptAsync(string launchpadAddress, CancellationTokenSource cancellationToken = null)
        {
            var excludeLaunchpadFromFeesAndMaxTxFunction = new ExcludeLaunchpadFromFeesAndMaxTxFunction();
                excludeLaunchpadFromFeesAndMaxTxFunction.LaunchpadAddress = launchpadAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(excludeLaunchpadFromFeesAndMaxTxFunction, cancellationToken);
        }

        public Task<BigInteger> GetRewardCycleBlockQueryAsync(GetRewardCycleBlockFunction getRewardCycleBlockFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRewardCycleBlockFunction, BigInteger>(getRewardCycleBlockFunction, blockParameter);
        }

        
        public Task<BigInteger> GetRewardCycleBlockQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRewardCycleBlockFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> IncludeInFeeRequestAsync(IncludeInFeeFunction includeInFeeFunction)
        {
             return ContractHandler.SendRequestAsync(includeInFeeFunction);
        }

        public Task<TransactionReceipt> IncludeInFeeRequestAndWaitForReceiptAsync(IncludeInFeeFunction includeInFeeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(includeInFeeFunction, cancellationToken);
        }

        public Task<string> IncludeInFeeRequestAsync(string account)
        {
            var includeInFeeFunction = new IncludeInFeeFunction();
                includeInFeeFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(includeInFeeFunction);
        }

        public Task<TransactionReceipt> IncludeInFeeRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var includeInFeeFunction = new IncludeInFeeFunction();
                includeInFeeFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(includeInFeeFunction, cancellationToken);
        }

        public Task<string> IncludeInMaxTxLimitationRequestAsync(IncludeInMaxTxLimitationFunction includeInMaxTxLimitationFunction)
        {
             return ContractHandler.SendRequestAsync(includeInMaxTxLimitationFunction);
        }

        public Task<TransactionReceipt> IncludeInMaxTxLimitationRequestAndWaitForReceiptAsync(IncludeInMaxTxLimitationFunction includeInMaxTxLimitationFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(includeInMaxTxLimitationFunction, cancellationToken);
        }

        public Task<string> IncludeInMaxTxLimitationRequestAsync(string account)
        {
            var includeInMaxTxLimitationFunction = new IncludeInMaxTxLimitationFunction();
                includeInMaxTxLimitationFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(includeInMaxTxLimitationFunction);
        }

        public Task<TransactionReceipt> IncludeInMaxTxLimitationRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var includeInMaxTxLimitationFunction = new IncludeInMaxTxLimitationFunction();
                includeInMaxTxLimitationFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(includeInMaxTxLimitationFunction, cancellationToken);
        }

        public Task<string> IncreaseAllowanceRequestAsync(IncreaseAllowanceFunction increaseAllowanceFunction)
        {
             return ContractHandler.SendRequestAsync(increaseAllowanceFunction);
        }

        public Task<TransactionReceipt> IncreaseAllowanceRequestAndWaitForReceiptAsync(IncreaseAllowanceFunction increaseAllowanceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(increaseAllowanceFunction, cancellationToken);
        }

        public Task<string> IncreaseAllowanceRequestAsync(string spender, BigInteger addedValue)
        {
            var increaseAllowanceFunction = new IncreaseAllowanceFunction();
                increaseAllowanceFunction.Spender = spender;
                increaseAllowanceFunction.AddedValue = addedValue;
            
             return ContractHandler.SendRequestAsync(increaseAllowanceFunction);
        }

        public Task<TransactionReceipt> IncreaseAllowanceRequestAndWaitForReceiptAsync(string spender, BigInteger addedValue, CancellationTokenSource cancellationToken = null)
        {
            var increaseAllowanceFunction = new IncreaseAllowanceFunction();
                increaseAllowanceFunction.Spender = spender;
                increaseAllowanceFunction.AddedValue = addedValue;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(increaseAllowanceFunction, cancellationToken);
        }

        public Task<bool> IsExcludedFromFeeQueryAsync(IsExcludedFromFeeFunction isExcludedFromFeeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsExcludedFromFeeFunction, bool>(isExcludedFromFeeFunction, blockParameter);
        }

        
        public Task<bool> IsExcludedFromFeeQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var isExcludedFromFeeFunction = new IsExcludedFromFeeFunction();
                isExcludedFromFeeFunction.Account = account;
            
            return ContractHandler.QueryAsync<IsExcludedFromFeeFunction, bool>(isExcludedFromFeeFunction, blockParameter);
        }

        public Task<bool> IsExcludedFromRewardQueryAsync(IsExcludedFromRewardFunction isExcludedFromRewardFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsExcludedFromRewardFunction, bool>(isExcludedFromRewardFunction, blockParameter);
        }

        
        public Task<bool> IsExcludedFromRewardQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var isExcludedFromRewardFunction = new IsExcludedFromRewardFunction();
                isExcludedFromRewardFunction.Account = account;
            
            return ContractHandler.QueryAsync<IsExcludedFromRewardFunction, bool>(isExcludedFromRewardFunction, blockParameter);
        }

        public Task<BigInteger> MinTokenNumberToSellQueryAsync(MinTokenNumberToSellFunction minTokenNumberToSellFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MinTokenNumberToSellFunction, BigInteger>(minTokenNumberToSellFunction, blockParameter);
        }

        
        public Task<BigInteger> MinTokenNumberToSellQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MinTokenNumberToSellFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> NameQueryAsync(NameFunction nameFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameter);
        }

        
        public Task<string> NameQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> NextAvailableClaimDateQueryAsync(NextAvailableClaimDateFunction nextAvailableClaimDateFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NextAvailableClaimDateFunction, BigInteger>(nextAvailableClaimDateFunction, blockParameter);
        }

        
        public Task<BigInteger> NextAvailableClaimDateQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var nextAvailableClaimDateFunction = new NextAvailableClaimDateFunction();
                nextAvailableClaimDateFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<NextAvailableClaimDateFunction, BigInteger>(nextAvailableClaimDateFunction, blockParameter);
        }

        public Task<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public Task<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public Task<string> PancakePairQueryAsync(PancakePairFunction pancakePairFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PancakePairFunction, string>(pancakePairFunction, blockParameter);
        }

        
        public Task<string> PancakePairQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PancakePairFunction, string>(null, blockParameter);
        }

        public Task<string> PancakeRouterQueryAsync(PancakeRouterFunction pancakeRouterFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PancakeRouterFunction, string>(pancakeRouterFunction, blockParameter);
        }

        
        public Task<string> PancakeRouterQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PancakeRouterFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> ReflectionFromTokenQueryAsync(ReflectionFromTokenFunction reflectionFromTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ReflectionFromTokenFunction, BigInteger>(reflectionFromTokenFunction, blockParameter);
        }

        
        public Task<BigInteger> ReflectionFromTokenQueryAsync(BigInteger tAmount, bool deductTransferFee, BlockParameter blockParameter = null)
        {
            var reflectionFromTokenFunction = new ReflectionFromTokenFunction();
                reflectionFromTokenFunction.TAmount = tAmount;
                reflectionFromTokenFunction.DeductTransferFee = deductTransferFee;
            
            return ContractHandler.QueryAsync<ReflectionFromTokenFunction, BigInteger>(reflectionFromTokenFunction, blockParameter);
        }

        public Task<string> RenounceOwnershipRequestAsync(RenounceOwnershipFunction renounceOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(renounceOwnershipFunction);
        }

        public Task<string> RenounceOwnershipRequestAsync()
        {
             return ContractHandler.SendRequestAsync<RenounceOwnershipFunction>();
        }

        public Task<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(RenounceOwnershipFunction renounceOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceOwnershipFunction, cancellationToken);
        }

        public Task<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceOwnershipFunction>(null, cancellationToken);
        }

        public Task<BigInteger> RewardCycleBlockQueryAsync(RewardCycleBlockFunction rewardCycleBlockFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RewardCycleBlockFunction, BigInteger>(rewardCycleBlockFunction, blockParameter);
        }

        
        public Task<BigInteger> RewardCycleBlockQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RewardCycleBlockFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> RewardThresholdQueryAsync(RewardThresholdFunction rewardThresholdFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RewardThresholdFunction, BigInteger>(rewardThresholdFunction, blockParameter);
        }

        
        public Task<BigInteger> RewardThresholdQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RewardThresholdFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> SetLiquidityFeePercentRequestAsync(SetLiquidityFeePercentFunction setLiquidityFeePercentFunction)
        {
             return ContractHandler.SendRequestAsync(setLiquidityFeePercentFunction);
        }

        public Task<TransactionReceipt> SetLiquidityFeePercentRequestAndWaitForReceiptAsync(SetLiquidityFeePercentFunction setLiquidityFeePercentFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setLiquidityFeePercentFunction, cancellationToken);
        }

        public Task<string> SetLiquidityFeePercentRequestAsync(BigInteger liquidityFee)
        {
            var setLiquidityFeePercentFunction = new SetLiquidityFeePercentFunction();
                setLiquidityFeePercentFunction.LiquidityFee = liquidityFee;
            
             return ContractHandler.SendRequestAsync(setLiquidityFeePercentFunction);
        }

        public Task<TransactionReceipt> SetLiquidityFeePercentRequestAndWaitForReceiptAsync(BigInteger liquidityFee, CancellationTokenSource cancellationToken = null)
        {
            var setLiquidityFeePercentFunction = new SetLiquidityFeePercentFunction();
                setLiquidityFeePercentFunction.LiquidityFee = liquidityFee;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setLiquidityFeePercentFunction, cancellationToken);
        }

        public Task<string> SetMaxTxPercentRequestAsync(SetMaxTxPercentFunction setMaxTxPercentFunction)
        {
             return ContractHandler.SendRequestAsync(setMaxTxPercentFunction);
        }

        public Task<TransactionReceipt> SetMaxTxPercentRequestAndWaitForReceiptAsync(SetMaxTxPercentFunction setMaxTxPercentFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMaxTxPercentFunction, cancellationToken);
        }

        public Task<string> SetMaxTxPercentRequestAsync(BigInteger maxTxPercent)
        {
            var setMaxTxPercentFunction = new SetMaxTxPercentFunction();
                setMaxTxPercentFunction.MaxTxPercent = maxTxPercent;
            
             return ContractHandler.SendRequestAsync(setMaxTxPercentFunction);
        }

        public Task<TransactionReceipt> SetMaxTxPercentRequestAndWaitForReceiptAsync(BigInteger maxTxPercent, CancellationTokenSource cancellationToken = null)
        {
            var setMaxTxPercentFunction = new SetMaxTxPercentFunction();
                setMaxTxPercentFunction.MaxTxPercent = maxTxPercent;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMaxTxPercentFunction, cancellationToken);
        }

        public Task<string> SetTaxFeePercentRequestAsync(SetTaxFeePercentFunction setTaxFeePercentFunction)
        {
             return ContractHandler.SendRequestAsync(setTaxFeePercentFunction);
        }

        public Task<TransactionReceipt> SetTaxFeePercentRequestAndWaitForReceiptAsync(SetTaxFeePercentFunction setTaxFeePercentFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTaxFeePercentFunction, cancellationToken);
        }

        public Task<string> SetTaxFeePercentRequestAsync(BigInteger taxFee)
        {
            var setTaxFeePercentFunction = new SetTaxFeePercentFunction();
                setTaxFeePercentFunction.TaxFee = taxFee;
            
             return ContractHandler.SendRequestAsync(setTaxFeePercentFunction);
        }

        public Task<TransactionReceipt> SetTaxFeePercentRequestAndWaitForReceiptAsync(BigInteger taxFee, CancellationTokenSource cancellationToken = null)
        {
            var setTaxFeePercentFunction = new SetTaxFeePercentFunction();
                setTaxFeePercentFunction.TaxFee = taxFee;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTaxFeePercentFunction, cancellationToken);
        }

        public Task<BigInteger> SwapAndLiquifyAvailableFromQueryAsync(SwapAndLiquifyAvailableFromFunction swapAndLiquifyAvailableFromFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SwapAndLiquifyAvailableFromFunction, BigInteger>(swapAndLiquifyAvailableFromFunction, blockParameter);
        }

        
        public Task<BigInteger> SwapAndLiquifyAvailableFromQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SwapAndLiquifyAvailableFromFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> SymbolQueryAsync(SymbolFunction symbolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(symbolFunction, blockParameter);
        }

        
        public Task<string> SymbolQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(null, blockParameter);
        }

        public Task<string> TemporarilyFreezeSwapAndLiquifyRequestAsync(TemporarilyFreezeSwapAndLiquifyFunction temporarilyFreezeSwapAndLiquifyFunction)
        {
             return ContractHandler.SendRequestAsync(temporarilyFreezeSwapAndLiquifyFunction);
        }

        public Task<TransactionReceipt> TemporarilyFreezeSwapAndLiquifyRequestAndWaitForReceiptAsync(TemporarilyFreezeSwapAndLiquifyFunction temporarilyFreezeSwapAndLiquifyFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(temporarilyFreezeSwapAndLiquifyFunction, cancellationToken);
        }

        public Task<string> TemporarilyFreezeSwapAndLiquifyRequestAsync(BigInteger time)
        {
            var temporarilyFreezeSwapAndLiquifyFunction = new TemporarilyFreezeSwapAndLiquifyFunction();
                temporarilyFreezeSwapAndLiquifyFunction.Time = time;
            
             return ContractHandler.SendRequestAsync(temporarilyFreezeSwapAndLiquifyFunction);
        }

        public Task<TransactionReceipt> TemporarilyFreezeSwapAndLiquifyRequestAndWaitForReceiptAsync(BigInteger time, CancellationTokenSource cancellationToken = null)
        {
            var temporarilyFreezeSwapAndLiquifyFunction = new TemporarilyFreezeSwapAndLiquifyFunction();
                temporarilyFreezeSwapAndLiquifyFunction.Time = time;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(temporarilyFreezeSwapAndLiquifyFunction, cancellationToken);
        }

        public Task<BigInteger> ThreshHoldTopUpRateQueryAsync(ThreshHoldTopUpRateFunction threshHoldTopUpRateFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ThreshHoldTopUpRateFunction, BigInteger>(threshHoldTopUpRateFunction, blockParameter);
        }

        
        public Task<BigInteger> ThreshHoldTopUpRateQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ThreshHoldTopUpRateFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> TokenFromReflectionQueryAsync(TokenFromReflectionFunction tokenFromReflectionFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokenFromReflectionFunction, BigInteger>(tokenFromReflectionFunction, blockParameter);
        }

        
        public Task<BigInteger> TokenFromReflectionQueryAsync(BigInteger rAmount, BlockParameter blockParameter = null)
        {
            var tokenFromReflectionFunction = new TokenFromReflectionFunction();
                tokenFromReflectionFunction.RAmount = rAmount;
            
            return ContractHandler.QueryAsync<TokenFromReflectionFunction, BigInteger>(tokenFromReflectionFunction, blockParameter);
        }

        public Task<BigInteger> TotalFeesQueryAsync(TotalFeesFunction totalFeesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalFeesFunction, BigInteger>(totalFeesFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalFeesQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalFeesFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> TotalSupplyQueryAsync(TotalSupplyFunction totalSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalSupplyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> TransferRequestAsync(TransferFunction transferFunction)
        {
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(TransferFunction transferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferRequestAsync(string recipient, BigInteger amount)
        {
            var transferFunction = new TransferFunction();
                transferFunction.Recipient = recipient;
                transferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(string recipient, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFunction = new TransferFunction();
                transferFunction.Recipient = recipient;
                transferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<BigInteger> TransferActivatedFromQueryAsync(TransferActivatedFromFunction transferActivatedFromFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TransferActivatedFromFunction, BigInteger>(transferActivatedFromFunction, blockParameter);
        }

        
        public Task<BigInteger> TransferActivatedFromQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TransferActivatedFromFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> TransferFromRequestAsync(TransferFromFunction transferFromFunction)
        {
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(TransferFromFunction transferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public Task<string> TransferFromRequestAsync(string sender, string recipient, BigInteger amount)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.Sender = sender;
                transferFromFunction.Recipient = recipient;
                transferFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(string sender, string recipient, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.Sender = sender;
                transferFromFunction.Recipient = recipient;
                transferFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(TransferOwnershipFunction transferOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(TransferOwnershipFunction transferOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(string newOwner)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(string newOwner, CancellationTokenSource cancellationToken = null)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }
    }
}
