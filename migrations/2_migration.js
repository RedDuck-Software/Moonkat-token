const Test = artifacts.require("Test");
const Utils = artifacts.require("Utils");
const PreSaleFactory = artifacts.require("PreSaleFactory");

module.exports = async function (deployer, network, accounts) {
  await deployer.deploy(Utils); // deploy Utils library
  await deployer.link(Utils, Test); // link it to Test contract

  const swapRouterAddressMainnet = "0x7a250d5630B4cF539739dF2C5dAcb4c659F2488D";
  const swapRouterAddressTestnet = "0xD99D1c33F9fC3444f8101754aBC46c52416550D1";

  await deployer.deploy(
    Test,
    swapRouterAddressTestnet // address of swapRouter - needs to be changed
  ); // deploy test contract

  let testInstance = await Test.deployed();

  await deployer.deploy(PreSaleFactory, Test.address); // deploy factory

  await testInstance.excludeFromFee(PreSaleFactory.address);

  await testInstance.transfer(
    PreSaleFactory.address,
    (await testInstance.balanceOf(accounts[0])).divn(10) // div + round
  ); // transfer 10% of total supply to factory
};
