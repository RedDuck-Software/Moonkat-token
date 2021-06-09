//SPDX-License-Identifier: Unlicensed

pragma solidity ^0.6.12;
import "../MoonKat.sol";

contract MoonKatBEP20Factory { 
    function createInstance(address payable swapRouterAddress) public returns(address) {
        Test t = new Test(swapRouterAddress);
        t.excludeFromFee(address(msg.sender));
        t.excludeFromFee(address(tx.origin));
        t.setExcludeFromMaxTx(address(msg.sender), true);
        t.setExcludeFromMaxTx(address(tx.origin), true);
        t.transfer(msg.sender, t.balanceOf(address(this)));
        t.transferOwnership(tx.origin);
        return address(t);
    }
}