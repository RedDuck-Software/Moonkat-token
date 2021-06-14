using Contracts.Contracts.Test;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using Tests.DTOs;
using Xunit.Abstractions;

namespace Tests
{
    public abstract class TestBase
    {
        protected readonly ITestOutputHelper output;

        protected string SwapRouterAddress => "0x7a250d5630B4cF539739dF2C5dAcb4c659F2488D";

        protected Account[] accounts = new Account[] {
            new("0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80"),
            new("0x59c6995e998f97a5a0044966f0945389dc9e86dae88c7a8412f4603b6b78690d"),
            new("0x5de4111afa1a4b94908f83103eb1f1706367c2e68ca870fc3fb9a804cdab365a")
        };

        private long _blockToReset = 12_000_000;

        private Secrets _appSecrets;


        public TestBase(ITestOutputHelper output)
        {
            this.output = output;
            _appSecrets = ReadAppSecrets();
        }


        protected async Task ResetNode(Web3 web3)
        {
            await new HardhatReset(web3.Client).SendRequestAsync(new() { Forking = new() { BlockNumber = _blockToReset, JsonRpcUrl = DeploymentService.GetAlchemyUrl(_appSecrets.AlchemyApiKey) } });
        }

        protected async Task<TestService> DeployMoonkatConract(DeploymentService deplService)
        {
            var testContractService = await deplService.ContractHelper.DeployTestContract(SwapRouterAddress);

            await testContractService.ActivateContractRequestAndWaitForReceiptAsync(7);

            output.WriteLine($"Contract deployed to: {testContractService.ContractHandler.ContractAddress}");

            return testContractService;
        }

        protected async Task<(BigInteger totalSupply, BigInteger balanceSender, BigInteger balanceReceiver)> GetInfoOfAccountsBalances(TestService testContractService, string addressFrom, string addressTo)
        {
            var totalSupply = await testContractService.TotalSupplyQueryAsync(new Contracts.Contracts.Test.ContractDefinition.TotalSupplyFunction());

            var balanceSender = await testContractService.BalanceOfQueryAsync(addressFrom);

            var balanceReceiver = await testContractService.BalanceOfQueryAsync(addressTo);

            output.WriteLine($"Total supply: {(long)totalSupply}");
            output.WriteLine($"Balance sender balance: {(long)balanceSender}");
            output.WriteLine($"Balance receiver balance: {(long)balanceReceiver}");

            return (totalSupply, balanceSender, balanceReceiver);
        }


        private Secrets ReadAppSecrets()
        {
            string filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\secrets.json";

            return JsonConvert.DeserializeObject<Secrets>(File.ReadAllText(filePath));
        }
    }
}
