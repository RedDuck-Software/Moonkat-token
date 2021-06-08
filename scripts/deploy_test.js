const { ethers } = require("hardhat");

async function deployPreSaler(tokenAddress) {
  const PreSaler = await ethers.getContractFactory("PreSaler");
  return await PreSaler.deploy(
    "0x70997970c51812dc3a010c7d01b50e0d17dc79c8",
    Date.now() / 1000 | 0,
    10000,
    tokenAddress
  );
}

async function deploySampeERC20() {
  const erc20 = await ethers.getContractFactory("SampleERC20");
  return await erc20.deploy();
}

async function main() {
  var ercToken = await deploySampeERC20();
  console.log("ERC20 token deployed to: '\x1b[36m%s\x1b[0m'", ercToken.address);

  var saler = await deployPreSaler(ercToken.address);
  console.log(
    "PreSaler contract deployed to: '\x1b[36m%s\x1b[0m'",
    saler.address
  );
}

main()
  .then(() => process.exit(0))
  .catch((error) => {
    console.error(error);
    process.exit(1);
  });