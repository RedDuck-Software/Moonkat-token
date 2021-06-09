//SPDX-License-Identifier: Unlicensed
pragma solidity ^0.6.12;

import {IBEP20, Test, SafeMath} from "../MoonKat.sol";
import {PreSeller} from "../PreSeller.sol";
import "./MoonKatBEP20Factory.sol";
import "./PreSellerFactory.sol";


contract MoonKatMegaFactory {
    using SafeMath for uint256;

    MoonKatBEP20Factory public bep20Factory;
    PreSellerFactory public preSellerFactory;

    constructor(address _bep20Factory, address _preSaleFactory) public {
        bep20Factory = MoonKatBEP20Factory(_bep20Factory);
        preSellerFactory = PreSellerFactory(_preSaleFactory);
    }

    function createInstance(address payable _swapRouterAddress, uint _preSaleStartDate,uint _preSaleDuration) public {
        Test moonKat = Test(payable(bep20Factory.createInstance(_swapRouterAddress)));

        PreSeller preSeller = PreSeller(preSellerFactory.createInstance(msg.sender, _preSaleStartDate, _preSaleDuration, address(moonKat)));

        uint256 preSellAmount = moonKat.balanceOf(address(this)).div(100).mul(10);

        moonKat.transfer(
            address(preSeller),
            preSellAmount
        );
    }
}
