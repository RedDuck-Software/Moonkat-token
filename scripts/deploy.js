const { ethers } = require("hardhat");

async function deployMoonKat() {
  const MoonKat = await ethers.getContractFactory("MoonKat");
  return await MoonKat.deploy(
    "0x9Ac64Cc6e4415144C455BD8E4837Fea55603e5c3",
    (Date.now() / 1000) | 0,
    10000
  );
}

async function main() {
  var moonKat = await deployMoonKat();
  console.log(
    "MoonKat contract deployed to: '\x1b[36m%s\x1b[0m'",
    moonKat.address
  );
}

main()
  .then(() => process.exit(0))
  .catch((error) => {
    console.error(error);
    process.exit(1);
  });
