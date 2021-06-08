// You need to export an object to set up your config
// Go to https://hardhat.org/config/ to learn more
require("@nomiclabs/hardhat-waffle");

let config = require("./secrets.json");

/**
 * @type import('hardhat/config').HardhatUserConfig
 */
module.exports = {
  solidity: "0.6.12",
  networks: {
    development: {
      url: "http://127.0.0.1:8545",
    },
    kovan: {
      url: `https://kovan.infura.io/v3/${config.projectId}`,
      accounts: {
        mnemonic: config.mnemonic,
      },
      chainId: 42,
      gas: 1230000,
    },
  },
};
