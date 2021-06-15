//SPDX-License-Identifier: Unlicensed
pragma solidity ^0.6.12;

import "../PreSale.sol";
import "../MoonKat.sol";

contract PreSaleFactory {
    address[] public instances;

    MKAT public mkat;

    constructor(address payable _mkatAddress) public { 
        mkat = MKAT(_mkatAddress);
    }

    function createInstance(
        address payable _moneyTransferTo,
        uint256 _saleStart,
        uint256 _saleDuration,
        address _tokenOnSale,
        address[] memory whitelist
    ) public returns (address) {
        PreSale instance =
            new PreSale(
                _moneyTransferTo,
                _saleStart,
                _saleDuration,
                _tokenOnSale,
                whitelist
            );

        // transfers 10% of totalSupply to preSale contract
        mkat.transfer(
            address(instance),
            mkat.balanceOf(address(this))
        );

        instances.push(address(instance));
        return address(instance);
    }
}
