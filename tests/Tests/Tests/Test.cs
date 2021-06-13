using Contracts.Contracts.IPancakePair;
using Contracts.Contracts.IPancakeRouter02;
using Contracts.Contracts.Test.ContractDefinition;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexTypes;
using System;
using System.Threading.Tasks;
using Tests.DTOs.Events;
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

            var amountToSend = await testContractService.MaxTxAmountQueryAsync() / 10;


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


            var amountToSend = await testContractService.MaxTxAmountQueryAsync() / 10; // 0,01 of total


            var amountTokenDesiredToPutIn = 10000000000;

            await testContractService.ApproveRequestAndWaitForReceiptAsync(routerService.ContractHandler.ContractAddress, amountTokenDesiredToPutIn);

            await routerService.AddLiquidityETHRequestAndWaitForReceiptAsync(new Contracts.Contracts.IPancakeRouter02.ContractDefinition.AddLiquidityETHFunction
            {
                Token = testContractService.ContractHandler.ContractAddress,
                AmountTokenDesired = amountTokenDesiredToPutIn,
                AmountTokenMin = 1,
                To = deplSerivice.Account.Address,
                AmountETHMin = 100000000000000,
                AmountToSend = 100000000000000,
                Deadline = DateTimeOffset.Now.AddYears(10).ToUnixTimeSeconds()
            });

            var initReserves = await pairService.GetReservesQueryAsync();

            await testContractService.TransferRequestAndWaitForReceiptAsync(recipient: accounts[1].Address, amount: amountToSend);

            await testContractService.TransferRequestAndWaitForReceiptAsync(
                recipient: testContractService.ContractHandler.ContractAddress,
                amount: await testContractService.MinTokenNumberToSellQueryAsync() * 2
            );

            bool shouldSell = await testContractService.BalanceOfQueryAsync(testContractService.ContractHandler.ContractAddress) >= await testContractService.MinTokenNumberToSellQueryAsync();


            Assert.True(shouldSell, "shouldShell must be true");


            deplSerivice.UpdateWeb3(accounts[1], DeploymentService.PrivateLocalNetworkUrl);

            testContractService = new Contracts.Contracts.Test.TestService(deplSerivice.Web3, testContractService.ContractHandler.ContractAddress);

            var tx = await testContractService.TransferRequestAndWaitForReceiptAsync(recipient: accounts[2].Address, amount: amountToSend);

            var eventHandler = deplSerivice.Web3.Eth.GetEvent<SwapAndLiquifyEventDTO>(testContractService.ContractHandler.ContractAddress);

            foreach (var ev in await eventHandler.GetAllChanges(eventHandler.CreateFilterInput()))
                output.WriteLine($"CLAIM BNB EVENT\nEth received: {ev.Event.EthReceived},Tokens in liqudity: {ev.Event.TokensIntoLiqudity}, Tokens Swaped: {ev.Event.TokensSwapped}");

            var isZero = new HexBigInteger(testContractService.ContractHandler.ContractAddress).Value < new HexBigInteger((await routerService.WETHQueryAsync())).Value;

            var newReserves = await pairService.GetReservesQueryAsync();

            var initReserve = isZero ? initReserves.Reserve0 : initReserves.Reserve1;
            var newReserve = isZero ? newReserves.Reserve0 : newReserves.Reserve1;

            Assert.True(newReserve > initReserve, "Updated reserves must be bigger than init");
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

            var amountToSend = await testContractService.MaxTxAmountQueryAsync() / 10;

            await testContractService.DisruptiveTransferRequestAndWaitForReceiptAsync(
                new DisruptiveTransferFunction
                {
                    Amount = amountToSend,
                    Recipient = addressTo,
                    AmountToSend = 2000000000000000000
                });

            Assert.False(await testContractService.IsExcludedFromRewardQueryAsync(addressTo), $"Expected that {addressTo} is not excluded from reward");

            await deplSerivice.Web3.Client.SendRequestAsync("evm_increaseTime", null, DateTimeOffset.Now.AddDays(8).ToUnixTimeSeconds());

            deplSerivice.UpdateWeb3(accounts[1], DeploymentService.PrivateLocalNetworkUrl);

            testContractService = new Contracts.Contracts.Test.TestService(deplSerivice.Web3, testContractService.ContractHandler.ContractAddress);

            var initBalance = (await deplSerivice.Web3.Eth.GetBalance.SendRequestAsync(deplSerivice.Account.Address)).Value;

            var txClaim = await testContractService.ClaimBNBRewardRequestAndWaitForReceiptAsync(new ClaimBNBRewardFunction() { GasPrice = 0 });


            var eventHandler = deplSerivice.Web3.Eth.GetEvent<ClaimBNBSuccessfullyEventDTO>(testContractService.ContractHandler.ContractAddress);


            foreach (var ev in await eventHandler.GetAllChanges(eventHandler.CreateFilterInput()))
                output.WriteLine($"CLAIM BNB EVENT\nRecipient: {ev.Event.Recipient},BNBSended: {ev.Event.EthReceived}, Next availible: {ev.Event.NextAvailableClaimDate}");

            var bnbBalanceAfterRewardClaimed = (await deplSerivice.Web3.Eth.GetBalance.SendRequestAsync(deplSerivice.Account.Address)).Value;



            output.WriteLine("Current address: " + deplSerivice.Account.Address);
            output.WriteLine("Init BNB balance: " + initBalance);
            output.WriteLine("New BNB balance: " + bnbBalanceAfterRewardClaimed);

            Assert.True(bnbBalanceAfterRewardClaimed > initBalance, "Expected that bnb balance will be bigger after reward claimed");
        }
    }
}
