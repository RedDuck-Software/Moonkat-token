// You need to export an object to set up your config
// Go to https://hardhat.org/config/ to learn more
require("@nomiclabs/hardhat-waffle")

/**
 * @type import('hardhat/config').HardhatUserConfig
 */
module.exports = {
  solidity: "0.8.4",
  networks: {
    development: {
      url: "http://127.0.0.1:8545",
    },
  },
};
