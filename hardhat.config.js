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
    bscTestnet: {
      url: `https://data-seed-prebsc-1-s1.binance.org:8545`,
      accounts: {
        mnemonic: config.mnemonic,
      },
      chainId: 97,
      gas: 12300000,
    },
  },
};
