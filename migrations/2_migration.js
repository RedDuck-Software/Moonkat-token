//const Factory = artifacts.require("MoonKatFactory");
const Test = artifacts.require("Test");
const Utils = artifacts.require("Utils");


module.exports = async function (deployer) {
    await deployer.deploy(Utils);
    await deployer.link(Utils, Test);
    await deployer.deploy(Test, "0x7a250d5630B4cF539739dF2C5dAcb4c659F2488D");
};