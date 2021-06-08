//SPDX-License-Identifier: Unlicensed
pragma solidity ^0.6.12;

import {IBEP20, Test, SafeMath} from "./MoonKat.sol";
import {PreSeller} from "./PreSeller.sol";

contract MoonKatFactory {
    using SafeMath for uint256;

    constructor() public {}

    function createMoonKat(address payable _swapRouterAddress, uint _preSaleStartDate,uint _preSaleDuration) public {
        Test moonKat = new Test(_swapRouterAddress);

        IBEP20 bep = IBEP20(address(moonKat));

        PreSeller preSeller = new PreSeller(msg.sender, _preSaleStartDate, _preSaleDuration, address(moonKat));

        uint256 preSellAmount = bep.balanceOf(address(this)).div(100).mul(10);

        bep.transfer(
            address(preSeller),
            preSellAmount
        );
    }
}
