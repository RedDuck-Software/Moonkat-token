//SPDX-License-Identifier: Unlicensed
pragma solidity ^0.6.12;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";

contract SampleERC20 is ERC20 {
    constructor() ERC20("", "") {
        _mint(msg.sender, 10 ** 10);
    }
}
