//SPDX-License-Identifier: Unlicensed

pragma solidity ^0.6.12;

import "@openzeppelin/contracts/token/ERC20/IERC20.sol";

import {
    SafeMath as OZSafeMath
} from "@openzeppelin/contracts/math/SafeMath.sol";

contract PreSaler {
    using OZSafeMath for uint256;

    string public termsAndConditions =
        "By interacting with this contract, I confirm I am not a US citizen. I agree to be bound by the terms found at [termsAndConditionUrl]";

    IERC20 public tokenOnSale;
    uint256 public saleStart;
    uint256 public saleDuration;
    address payable moneyTransferTo;

    uint256 public minBNBAmount = 10**17; // 0.1 ether
    uint256 public maxBNBAmount = 3 ether;

    uint256 public oneTokenPriceInBNB = 4000000000000;

    modifier onlyInActive() {
        require(!_isSalePeriodEnd(), "Sale is not active");
        _;
    }

    modifier onlyInUnactive() {
        require(_isSalePeriodEnd(), "Sale is still active");
        _;
    }

    constructor(
        // address, to where all BNB from buy method will be forwarded.
        // Also, all remaining tokens, that were didn`t sold
        // Will be transfered to this address
        address payable _moneyTransferTo,
        uint256 _saleStart,
        uint256 _saleDuration,
        address _tokenOnSale
    ) public {
        require(_saleStart != 0, "Sale start should be greater than 0");
        require(_saleDuration != 0, "Sale duration should be greater than 0");
        require(
            _tokenOnSale != address(0),
            "You should provide valid token address"
        );
        require(
            _moneyTransferTo != address(0),
            "You should provide valid forwarded address"
        );

        tokenOnSale = IERC20(_tokenOnSale);
        saleStart = _saleStart;
        saleDuration = _saleDuration;
        moneyTransferTo = _moneyTransferTo;
    }

    function buy() public payable onlyInActive {
        require(msg.value > 0, "Value > 0 must be send with transaction");
        require(
            msg.value > minBNBAmount,
            "Value must be bigger then minBNBAmount"
        );
        require(
            msg.value < maxBNBAmount,
            "Value must be less then maxBNBAmount"
        );

        tokenOnSale.transfer(msg.sender, _calculateTokenAmount(msg.value));
        _sendValue(moneyTransferTo, msg.value);
    }

    function withdrawRemainderTokens() public onlyInUnactive {
        tokenOnSale.transfer(
            moneyTransferTo,
            tokenOnSale.balanceOf(address(this))
        );
    }

    function getCurentTime() public view returns (uint256) {
        return block.timestamp;
    }

    function calculateTokenAmount(uint256 _bnbAmount)
        public
        view
        returns (uint256)
    {
        return _calculateTokenAmount(_bnbAmount);
    }

    function _calculateTokenAmount(uint256 _bnbAmount)
        private
        view
        returns (uint256)
    {
        return _bnbAmount.div(oneTokenPriceInBNB);
    }

    function _isSalePeriodEnd() private view returns (bool) {
        require(saleStart < block.timestamp, "Sale didn`t started yet");
        return saleStart + saleDuration < block.timestamp;
    }

    function _sendValue(address payable to, uint256 amount) private {
        require(address(this).balance >= amount, "Insufficient balance");
        (bool success, ) = to.call{value: amount}("");
        require(success, "Unable to send value, recipient may have reverted");
    }
}
