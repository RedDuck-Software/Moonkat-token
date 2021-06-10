using Contracts.Contracts.IPancakePair;
using Contracts.Contracts.IPancakeRouter02;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class Test : TestBase
    {
        public Test(ITestOutputHelper outputHelper) : base(outputHelper) { }

        [Fact]
        public async Task moonkat_contract_deployment()
        {
            var deplSerivice = new DeploymentService(accounts[0], DeploymentService.PrivateLocalNetworkUrl);

            await DeployMoonkatConract(deplSerivice);
        }

        [Fact]
        public async Task transfer_from_owner_without_fee()
        {
            var deplSerivice = new DeploymentService(accounts[0], DeploymentService.PrivateLocalNetworkUrl);

            var testContractService = await DeployMoonkatConract(deplSerivice);

            var addressTo = accounts[1].Address;

            var (_, balanceSender, balanceReceiver) = await GetInfoOfAccountsBalances(testContractService, deplSerivice.Account.Address, addressTo);

            var amountToSend = balanceSender / 10;


            var txTransferReceipt = await testContractService.TransferRequestAndWaitForReceiptAsync(recipient: addressTo, amount: amountToSend);

            output.WriteLine($"Tx hash of transfer: {txTransferReceipt.TransactionHash}");

            var newBalanceOfReceiver = await testContractService.BalanceOfQueryAsync(addressTo);

            output.WriteLine($"New recepient balance: {newBalanceOfReceiver}");

            Assert.Equal(balanceReceiver + amountToSend, newBalanceOfReceiver);
        }


        [Fact]
        public async Task transfer_from_address_to_address_with_6pocentage_fee()
        {
            var deplSerivice = new DeploymentService(accounts[0], DeploymentService.PrivateLocalNetworkUrl);

            var testContractService = await DeployMoonkatConract(deplSerivice);


            var addressFrom = deplSerivice.Account.Address;
            var addressTo = accounts[1].Address;


            var (_, balanceSender, balanceReceiver) = await GetInfoOfAccountsBalances(testContractService, addressFrom, addressTo);

            var amountToSend = await testContractService.MaxTxAmountQueryAsync() / 10;

            await testContractService.TransferRequestAndWaitForReceiptAsync(recipient: addressTo, amount: amountToSend);


            addressFrom = accounts[1].Address;
            addressTo = accounts[2].Address;


            // these addresses are not MoonKat maintainers, so thay need to not be excluded from 6% fee
            Assert.False(await testContractService.IsExcludedFromFeeQueryAsync(addressFrom), $"Expected that {addressFrom} is not excluded from fee");
            Assert.False(await testContractService.IsExcludedFromFeeQueryAsync(addressTo), $"Expected that {addressTo} is not excluded from fee");

            (_, balanceSender, _) = await GetInfoOfAccountsBalances(testContractService, addressFrom, addressTo);


            output.WriteLine($"Base balance of sender is: {balanceSender}");

            amountToSend = balanceSender;

            deplSerivice.UpdateWeb3(accounts[1], DeploymentService.PrivateLocalNetworkUrl);

            testContractService = new Contracts.Contracts.Test.TestService(deplSerivice.Web3, testContractService.ContractHandler.ContractAddress);

            await testContractService.TransferRequestAndWaitForReceiptAsync(recipient: addressTo, amount: amountToSend);


            var newBalanceOfReceiver = await testContractService.BalanceOfQueryAsync(addressTo);

            var fee = (amountToSend * 6) / 100;

            var expectedBalanceIncludingTax = amountToSend - fee; // 6% fee substructed

            Assert.Equal(expectedBalanceIncludingTax, newBalanceOfReceiver);
        }

        [Fact]
        public async Task transfer_with_6percent_fee_and_check_liquidity()
        {
            var deplSerivice = new DeploymentService(accounts[0], DeploymentService.PrivateLocalNetworkUrl);

            var testContractService = await DeployMoonkatConract(deplSerivice);

            var routerService = new IPancakeRouter02Service(deplSerivice.Web3, SwapRouterAddress);

            var wbnbAddress = await routerService.WETHQueryAsync();

            var pairAddress = await testContractService.PancakePairQueryAsync();

            var pairService = new IPancakePairService(deplSerivice.Web3, pairAddress);

            var initReserves = await pairService.GetReservesQueryAsync();

            var amountToSend = await testContractService.MaxTxAmountQueryAsync() / 10; // 0,01 of total


            await testContractService.TransferRequestAndWaitForReceiptAsync(recipient: accounts[1].Address, amount: amountToSend);


            deplSerivice.UpdateWeb3(accounts[1], DeploymentService.PrivateLocalNetworkUrl);

            testContractService = new Contracts.Contracts.Test.TestService(deplSerivice.Web3, testContractService.ContractHandler.ContractAddress);

            await testContractService.TransferRequestAndWaitForReceiptAsync(recipient: accounts[2].Address, amount: amountToSend);


            var newBalanceOfReceiver = await testContractService.BalanceOfQueryAsync(accounts[2].Address);

            var fee = (amountToSend * 6) / 100;

            var liqFee = (fee / 3) * 2;

            var newReserves = await pairService.GetReservesQueryAsync();

            Assert.True(initReserves.Reserve1 > newReserves.Reserve1, "Excpected,that new reserves will be increased");
        }

        [Fact]
        public async Task disruptive_transfer()
        {
            var deplSerivice = new DeploymentService(accounts[0], DeploymentService.PrivateLocalNetworkUrl);

            var testContractService = await DeployMoonkatConract(deplSerivice);

            var addressFrom = deplSerivice.Account.Address;
            var addressTo = accounts[1].Address;

            var (_, balanceSender, _) = await GetInfoOfAccountsBalances(testContractService, addressFrom, addressTo);

            var amountToSend = await testContractService.MaxTxAmountQueryAsync() / 10;

            await testContractService.TransferRequestAndWaitForReceiptAsync(recipient: addressTo, amount: amountToSend);

            deplSerivice.UpdateWeb3(accounts[1], DeploymentService.PrivateLocalNetworkUrl);

            addressTo = accounts[2].Address;

            testContractService = new Contracts.Contracts.Test.TestService(deplSerivice.Web3, testContractService.ContractHandler.ContractAddress);


            await testContractService.DisruptiveTransferRequestAndWaitForReceiptAsync(
                new Contracts.Contracts.Test.ContractDefinition.DisruptiveTransferFunction
                {
                    Amount = amountToSend,
                    Recipient = addressTo,
                    AmountToSend = 2000000000000000000
                });

            Assert.Equal(await testContractService.BalanceOfQueryAsync(addressTo), amountToSend);
        }

        [Fact]
        public async Task claim_bnb_reward()
        {
            var deplSerivice = new DeploymentService(accounts[0], DeploymentService.PrivateLocalNetworkUrl);

            var testContractService = await DeployMoonkatConract(deplSerivice);


            var addressFrom = deplSerivice.Account.Address;
            var addressTo = accounts[1].Address;

            var (_, balanceSender, _) = await GetInfoOfAccountsBalances(testContractService, addressFrom, addressTo);

            var amountToSend = balanceSender / 10;

            await testContractService.TransferRequestAndWaitForReceiptAsync(recipient: addressTo, amount: amountToSend);

            Assert.False(await testContractService.IsExcludedFromRewardQueryAsync(addressTo), $"Expected that {addressTo} is not excluded from reward");

            var baseBnbBalance = (await deplSerivice.Web3.Eth.GetBalance.SendRequestAsync(addressTo)).Value;

            await deplSerivice.Web3.Client.SendRequestAsync("evm_increaseTime", null, DateTimeOffset.Now.AddDays(8).ToUnixTimeSeconds());

            deplSerivice.UpdateWeb3(accounts[1], DeploymentService.PrivateLocalNetworkUrl);

            testContractService = new Contracts.Contracts.Test.TestService(deplSerivice.Web3, testContractService.ContractHandler.ContractAddress);

            await testContractService.ClaimBNBRewardRequestAndWaitForReceiptAsync();

            var bnbBalanceAfterRewardClaimed = (await deplSerivice.Web3.Eth.GetBalance.SendRequestAsync(deplSerivice.Account.Address)).Value;

            output.WriteLine("New BNB balance: " + bnbBalanceAfterRewardClaimed);

            Assert.NotEqual(bnbBalanceAfterRewardClaimed, baseBnbBalance);
            Assert.True(bnbBalanceAfterRewardClaimed > baseBnbBalance, "Expected that bnb balance will be bigger after reward claimed");
        }
    }
}
