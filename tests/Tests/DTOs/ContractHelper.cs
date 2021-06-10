using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Contracts.Contracts.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Contracts.Contracts.Test.ContractDefinition;
using Nethereum.ABI.FunctionEncoding;
using Contracts.Contracts.Utils.ContractDefinition;
using System.Text.RegularExpressions;

namespace Tests.DTOs
{
    public class ContractHelper
    {
        public Web3 Web3 { get; }

        public Account Account { get; }

        public ContractHelper(Web3 web3, Account account = null) =>
            (this.Web3, this.Account) = (web3, account);



        public async Task<TestService> DeployTestContract(string pancakeRouterAddress)
        {
            var deployedLibrary = await DeployUtilsLibrary();

            var deployment = new TestDeployment(LinkLibraryTo(TestDeployment.BYTECODE, deployedLibrary.ContractAddress)) { RouterAddress = pancakeRouterAddress };

            return await TestService.DeployContractAndGetServiceAsync(Web3, deployment);
        }

        public async Task<TransactionReceipt> DeployUtilsLibrary()
        {
            var depl = new UtilsDeployment();

            return await Contracts.Contracts.Utils.UtilsService.DeployContractAndWaitForReceiptAsync(Web3, depl);
        }



        /// <returns>New ByteCode for a contract</returns>
        private string LinkLibraryTo(string byteCode, string libraryAddress)
        {
            var rx = new Regex(_libraryRegex);

            foreach (Match match in rx.Matches(byteCode))
                byteCode = byteCode.Replace(match.Groups[0].Value, libraryAddress.Replace("0x", ""));

            return byteCode;
        }


        private string _libraryRegex => @"__\$(.*?)\$__";
    }
}
