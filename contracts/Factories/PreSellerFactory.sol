//SPDX-License-Identifier: Unlicensed

pragma solidity ^0.6.12;
import "../PreSeller.sol";

contract PreSellerFactory {
    function createInstance(
        address payable _moneyTransferTo,
        uint256 _saleStart,
        uint256 _saleDuration,
        address _tokenOnSale
    ) public returns (address) {
        PreSeller t =
            new PreSeller(
                _moneyTransferTo,
                _saleStart,
                _saleDuration,
                _tokenOnSale
            );

        return address(t);
    }
}
