const Factory = artifacts.require("MoonKatFactory");
const Utils = artifacts.require("Utils");


module.exports = async function (deployer) {
     await deployer.deploy(Utils);
    var lib = Utils.deployed();
    
    await Factory.detectNetwork();

    await Factory.link('Utils', lib.address);
    // await deployer.link(Utils, Factory);
    await Factory.deploy();
};