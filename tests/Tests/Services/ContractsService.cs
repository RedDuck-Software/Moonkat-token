using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Tests.DTOs;

namespace Tests
{
    public class ContractsService
    {
        public static string DefaultDeploymentUrl => PrivateLocalNetworkUrl;

        public static string PrivateLocalNetworkUrl { get; set; } = "http://127.0.0.1:8545/";

        public static string PublicTestNodeUrl { get; set; } = "https://data-seed-prebsc-1-s1.binance.org:8545/";

        public static string GetAlchemyUrl(string privateKey) => $@"https://eth-mainnet.alchemyapi.io/v2/{privateKey}";
        
        public ContractHelper ContractHelper { get; set; }

        
        public Web3 Web3 { get; private set; }

        public Account Account { get; private set; }


        public ContractsService(Account account, string url)
        {
            Account = account;
            Web3 = new Web3(account:account,url: url);
            ContractHelper = new ContractHelper(Web3, account);
        }

        public ContractsService(string url)
        {
            Web3 = new Web3(url: url);
            ContractHelper = new ContractHelper(Web3);
        }

        public void UpdateWeb3(Account account , string url)
        {
            Account = account;
            Web3 = new Web3(account: account, url: url);
            ContractHelper = new ContractHelper(Web3, account);
        }
    }
}
