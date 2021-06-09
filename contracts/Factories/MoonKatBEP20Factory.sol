//SPDX-License-Identifier: Unlicensed

pragma solidity ^0.6.12;
import "../MoonKat.sol";

contract MoonKatBEP20Factory { 
    function createInstance(address payable swapRouterAddress) public returns(address) {
        Test t = new Test(swapRouterAddress);
        return address(t);
    }
}