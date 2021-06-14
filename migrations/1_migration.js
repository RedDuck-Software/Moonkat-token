const argv = require('minimist')(process.argv.slice(2));

if(argv['swap'] === undefined) { 
  console.log("Swap router address didn`t specified using --swap flag");
  process.exit(1)
}

const Test = artifacts.require("Test");
const Utils = artifacts.require("Utils");
const PreSaleFactory = artifacts.require("PreSaleFactory");

module.exports = async function (deployer, network, accounts) {
  await deployer.deploy(Utils); // deploy Utils library
  await deployer.link(Utils, Test); // link it to Test contract

  // const swapRouterAddressMainnet = "0x05fF2B0DB69458A0750badebc4f9e13aDd608C7F";
  // const swapRouterAddressTestnet = "0x9Ac64Cc6e4415144C455BD8E4837Fea55603e5c3";

  await deployer.deploy(
    Test,
    argv['swap'] // address of swapRouter - needs to be specified using --swap
  ); // deploy test contract

  let testInstance = await Test.deployed();

  await deployer.deploy(PreSaleFactory, Test.address); // deploy factory

  await testInstance.excludeFromFee(PreSaleFactory.address);

  await testInstance.transfer(
    PreSaleFactory.address,
    (await testInstance.balanceOf(accounts[0])).divn(10) // div + round
  ); // transfer 10% of total supply to factory
};
